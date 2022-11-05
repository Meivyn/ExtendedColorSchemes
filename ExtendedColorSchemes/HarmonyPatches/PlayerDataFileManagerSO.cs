using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace ExtendedColorSchemes.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerDataFileManagerSO), "Save")]
    internal class SavePatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Replaces the color schemes loop by our own to pass in the custom type.
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Ldc_I4_0),
                    new CodeMatch(OpCodes.Stloc_S),
                    new CodeMatch(OpCodes.Br))
                .ThrowIfInvalid("Couldn't match remove condition")
                .RemoveInstructions(37)
                .Insert(new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(PlayerData), nameof(PlayerData.colorSchemesSettings))),
                    new CodeInstruction(OpCodes.Ldloca_S, 2),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SavePatch), nameof(GetColorSchemes))))
                .InstructionEnumeration();
        }

        private static void Prefix()
        {
            Plugin.Log.Warn("Save");
        }

        private static void Postfix()
        {
            var filePath = $"{Application.persistentDataPath}/PlayerData.dat";
            var backupFilePath = $"{Application.persistentDataPath}/PlayerData_{DateTime.Now:yyyy-MM-dd}.dat.bak";

            try
            {
                File.Copy(filePath, backupFilePath, true);

                // Only keep the last 5 most recent backups to stop file spam.
                Directory.EnumerateFiles(Application.persistentDataPath, "PlayerData_*")
                    .OrderByDescending(File.GetCreationTime)
                    .Skip(5)
                    .Do(File.Delete);
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e);
            }
        }

        // TODO: Also run something similar on LoadFromCurrentVersion (SetColorSchemeForId)
        private static void GetColorSchemes(ColorSchemesSettings colorSchemesSettings, ref List<PlayerSaveData.ColorScheme> list)
        {
            for (var i = 0; i < colorSchemesSettings.GetNumberOfColorSchemes(); i++)
            {
                ColorScheme colorScheme = colorSchemesSettings.GetColorSchemeForIdx(i);
                if (colorScheme.isEditable)
                {
                    ColorSchemeWithEditableName? savedColorScheme = Plugin.Config.colorSchemes.FirstOrDefault(x => x.colorSchemeId == colorScheme.colorSchemeId);
                    ColorSchemeWithEditableName colorSchemeWithEditableName = new(
                        savedColorScheme?.ColorSchemeName ?? string.Empty,
                        colorScheme.colorSchemeId,
                        colorScheme.saberAColor,
                        colorScheme.saberBColor,
                        colorScheme.environmentColor0,
                        colorScheme.environmentColor1,
                        colorScheme.obstaclesColor,
                        colorScheme.supportsEnvironmentColorBoost,
                        colorScheme.environmentColor0Boost,
                        colorScheme.environmentColor1Boost);
                    list.Add(colorSchemeWithEditableName);
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerDataFileManagerSO), "LoadFromCurrentVersion")]
    internal class LoadFromCurrentVersionPatch
    {
        public static bool PlayerDataIsCorrupted;
        public static bool IsCalledByLoadFromCurrentVersion;

        private static void Prefix()
        {
            Plugin.Log.Warn("LoadFromCurrentVersion");
            IsCalledByLoadFromCurrentVersion = true;
        }

        private static void Postfix(PlayerData __result, PlayerSaveData playerSaveData)
        {
            IsCalledByLoadFromCurrentVersion = false;

            if (__result == null)
            {
                return;
            }

            var colorSchemesSettings = playerSaveData.localPlayers[0].colorSchemesSettings;

            if (ColorSchemesSettingsConstructor.DefaultColorSchemes.All(x => x.colorSchemeId != colorSchemesSettings.selectedColorSchemeId)
                || colorSchemesSettings.colorSchemes.Count != ColorSchemesSettingsConstructor.NumberOfDefaultUserColorSchemes)
            {
                PlayerDataIsCorrupted = true;
                Plugin.Log.Critical("Player data is corrupted! Please don't update the game or uninstall ExtendedColorSchemes before restoring one of the backups that have been stored in this directory: \"%userprofile%\\AppData\\LocalLow\\Hyperbolic Magnetism\\Beat Saber\", otherwise data can be lost.");
            }
        }
    }
}
