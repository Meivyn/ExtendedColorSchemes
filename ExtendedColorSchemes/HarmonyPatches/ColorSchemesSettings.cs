using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HarmonyLib;
using Polyglot;

namespace ExtendedColorSchemes.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorSchemesSettings), MethodType.Constructor, typeof(ColorScheme[]))]
    internal class ColorSchemesSettingsConstructor
    {
        public static int NumberOfDefaultUserColorSchemes;
        public static string? DefaultSelectedColorSchemeId;
        public static ColorScheme[]? DefaultColorSchemes;

        private const int NumberOfColorSchemesToAddByDefault = 8;
        private static readonly int NumberOfColorSchemesToAdd = Plugin.Config.colorSchemes.Count > 0 ? Plugin.Config.colorSchemes.Count : NumberOfColorSchemesToAddByDefault;

        internal static void Prefix(ref ColorScheme[] colorSchemes)
        {
            DefaultColorSchemes = colorSchemes;
            NumberOfDefaultUserColorSchemes = colorSchemes.Count(x => x.isEditable);
            string translation = Localization.Get("CUSTOM_0_COLOR_SCHEME");
            var regex = new Regex(@"\d+", RegexOptions.None);

            List<ColorScheme> colorSchemesList = colorSchemes.ToList();

            for (var i = 0; i < NumberOfColorSchemesToAdd; i++)
            {
                ColorSchemeWithEditableName? savedColorScheme = Plugin.Config.colorSchemes.Count > i ? Plugin.Config.colorSchemes[i] : null;
                colorSchemesList.Insert(i + NumberOfDefaultUserColorSchemes, new ColorScheme(
                    $"User{i + NumberOfDefaultUserColorSchemes}",
                    $"_UNLOCALIZED_",
                    true,
                    !string.IsNullOrWhiteSpace(savedColorScheme?.colorSchemeName) ? savedColorScheme?.colorSchemeName : regex.Replace(translation, (i + NumberOfDefaultUserColorSchemes).ToString()),
                    colorSchemes[0].isEditable,
                    savedColorScheme?.saberAColor ?? colorSchemes[0].saberAColor,
                    savedColorScheme?.saberBColor ?? colorSchemes[0].saberBColor,
                    savedColorScheme?.environmentColor0 ?? colorSchemes[0].environmentColor0,
                    savedColorScheme?.environmentColor1 ?? colorSchemes[0].environmentColor1,
                    colorSchemes[0].supportsEnvironmentColorBoost,
                    colorSchemes[0].environmentColor0Boost,
                    colorSchemes[0].environmentColor1Boost,
                    savedColorScheme?.obstaclesColor ?? colorSchemes[0].obstaclesColor));
            }

            if (Plugin.Config.selectedColorSchemeId != null && colorSchemesList.All(x => x.colorSchemeId != Plugin.Config.selectedColorSchemeId))
            {
                Plugin.Log.Warn($"Selected color scheme doesn't exist in the collection, restoring default value \"{DefaultSelectedColorSchemeId}\".");
                Plugin.Config.selectedColorSchemeId = DefaultSelectedColorSchemeId;
            }

            colorSchemes = colorSchemesList.ToArray();
        }
    }

    //[HarmonyPatch(typeof(ColorSchemesSettings), "selectedColorSchemeId", MethodType.Setter)]
    internal class ColorSchemesSettingsSelectedColorSchemeIdSetter
    {
        //[HarmonyPriority(Priority.HigherThanNormal)]
        internal static void Prefix(ref string? value)
        {
            if (LoadFromCurrentVersionPatch.PlayerDataIsCorrupted)
            {
                Plugin.Log.Error("Player data is corrupted, aborting.");
                return;
            }

            string? colorSchemeId = value;
            if (ColorSchemesSettingsConstructor.DefaultColorSchemes != null && ColorSchemesSettingsConstructor.DefaultColorSchemes.Any(x => x.colorSchemeId == colorSchemeId))
            {
                ColorSchemesSettingsConstructor.DefaultSelectedColorSchemeId = value;
            }

            if (!LoadFromCurrentVersionPatch.IsCalledByLoadFromCurrentVersion)
            {
                Plugin.Log.Info("Not called by LoadFromCurrentVersion, aborting.");
                return;
            }

            Plugin.Log.Info($"DefaultSelectedColorSchemeId={value}");

            if (Plugin.Config.selectedColorSchemeId == null)
            {
                Plugin.Config.selectedColorSchemeId = value;
                Plugin.Log.Info($"Plugin.Config.selectedColorSchemeId={value}");
            }
            else
            {
                value = Plugin.Config.selectedColorSchemeId;
                Plugin.Log.Info($"value={Plugin.Config.selectedColorSchemeId}");
            }
        }
    }
}
