using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace ExtendedColorSchemes.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerSaveData.ColorSchemesSettings), MethodType.Constructor, typeof(bool), typeof(string), typeof(List<PlayerSaveData.ColorScheme>))]
    internal class PlayerSaveDataColorSchemesSettings
    {
        internal static void Prefix(ref string selectedColorSchemeId, ref List<PlayerSaveData.ColorScheme> colorSchemes)
        {
            //if (ColorSchemesSettingsPatches.CollectionIsInvalid) return;

            // Since patch 1.16.0, game is saving on startup because of PlayerDataModel being disposed by SiraUtil,
            // ColorSchemesSettings isn't called yet so our list is null.
            if (ColorSchemesSettingsConstructor.DefaultColorSchemes == null) return;

            List<PlayerSaveData.ColorScheme> defaultColorSchemes = new(ColorSchemesSettingsConstructor.NumberOfDefaultUserColorSchemes);
            List<ColorSchemeWithEditableName> savedColorSchemes = new(colorSchemes.Count - ColorSchemesSettingsConstructor.NumberOfDefaultUserColorSchemes);
            string selectedColorScheme = selectedColorSchemeId;

            for (var i = 0; i < colorSchemes.Count; i++)
            {
                if (i < ColorSchemesSettingsConstructor.NumberOfDefaultUserColorSchemes)
                {
                    defaultColorSchemes.Add(colorSchemes[i]);

                    continue;
                }

                savedColorSchemes.Add((ColorSchemeWithEditableName)colorSchemes[i]);
            }

            Plugin.Config.colorSchemes = savedColorSchemes;
            Plugin.Config.selectedColorSchemeId = selectedColorSchemeId;
            Plugin.Log.Info($"PlayerSaveData: Plugin.Config.selectedColorSchemeId={selectedColorSchemeId}");

            if (ColorSchemesSettingsConstructor.DefaultColorSchemes.All(x => x.colorSchemeId != selectedColorScheme))
            {
                selectedColorSchemeId = ColorSchemesSettingsConstructor.DefaultSelectedColorSchemeId ?? "User0";
                Plugin.Log.Info($"PlayerSaveData: selectedColorSchemeId={ColorSchemesSettingsConstructor.DefaultSelectedColorSchemeId}");
            }

            colorSchemes = defaultColorSchemes;
        }
    }
}
