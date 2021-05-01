using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedColorSchemes.HarmonyPatches
{
    internal class HarmonyPatches
    {
        [HarmonyPatch(typeof(ColorSchemesSettings), MethodType.Constructor, new[] { typeof(ColorScheme[]) })]
        private class PColorSchemesSettings
        {
            internal static void Postfix(List<ColorScheme> ____colorSchemesList, string ____selectedColorSchemeId)
            {
                if (Plugin.Config.SelectedColorSchemeId == null)
                    Plugin.Config.SelectedColorSchemeId = ____selectedColorSchemeId;

                var isOutdated = Plugin.Config.ColorSchemesList.Any(x => x._colorSchemeNameLocalizationKey == "Default");

                if (Plugin.Config.ColorSchemesList.Count == 0 || isOutdated)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        Utils.ExtendedColorScheme oldColorScheme = null;

                        if (Plugin.Config.ColorSchemesList.Count > i)
                            oldColorScheme = Plugin.Config.ColorSchemesList[i];

                        var newColorScheme = new Utils.ExtendedColorScheme
                        {
                            _colorSchemeId = $"User{i + 4}",
                            _colorSchemeNameLocalizationKey = $"CUSTOM_{i + 4}_COLOR_SCHEME",
                            _isEditable = oldColorScheme != null ? oldColorScheme._isEditable : ____colorSchemesList[0].isEditable,
                            _saberAColor = oldColorScheme != null ? oldColorScheme._saberAColor : ____colorSchemesList[0].saberAColor,
                            _saberBColor = oldColorScheme != null ? oldColorScheme._saberBColor : ____colorSchemesList[0].saberBColor,
                            _environmentColor0 = oldColorScheme != null ? oldColorScheme._environmentColor0 : ____colorSchemesList[0].environmentColor0,
                            _environmentColor1 = oldColorScheme != null ? oldColorScheme._environmentColor1 : ____colorSchemesList[0].environmentColor1,
                            _supportsEnvironmentColorBoost = oldColorScheme != null ? oldColorScheme._supportsEnvironmentColorBoost : ____colorSchemesList[0].supportsEnvironmentColorBoost,
                            _environmentColor0Boost = oldColorScheme != null ? oldColorScheme._environmentColor0Boost : ____colorSchemesList[0].environmentColor0Boost,
                            _environmentColor1Boost = oldColorScheme != null ? oldColorScheme._environmentColor1Boost : ____colorSchemesList[0].environmentColor1Boost,
                            _obstaclesColor = oldColorScheme != null ? oldColorScheme._obstaclesColor : ____colorSchemesList[0].obstaclesColor,
                        };

                        if (oldColorScheme != null)
                            Plugin.Config.ColorSchemesList.Remove(oldColorScheme);

                        Plugin.Config.ColorSchemesList.Insert(i, newColorScheme);
                    }

                    Plugin.Config.Changed();
                }
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "GetNumberOfColorSchemes")]
        private class PGetNumberOfColorSchemes
        {
            internal static void Postfix(ref int __result)
            {
                // Prevents the game from saving our color schemes
                if (Utils.IsCallByMethod("Save"))
                    return;

                __result += Plugin.Config.ColorSchemesList.Count;
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "GetColorSchemeForIdx")]
        private class PGetColorSchemeForIdx
        {
            internal static bool Prefix(ref ColorScheme __result, ref int idx)
            {
                // Prevents the game from saving our color schemes,
                // we should only save the base color schemes
                if (Utils.IsCallByMethod("Save"))
                    return true;

                // Makes sure our color schemes are inserted at the fourth position
                // so it follows the base custom ones
                if (idx < 4)
                    return true;
                else if (idx >= Plugin.Config.ColorSchemesList.Count + 4)
                {
                    idx -= Plugin.Config.ColorSchemesList.Count;
                    return true;
                }

                __result = Plugin.Config.ColorSchemesList[idx - 4].ToColorScheme();

                return false;
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "GetColorSchemeForId")]
        private class PGetColorSchemeForId
        {
            internal static void Postfix(ref ColorScheme __result, string id)
            {
                var extendedColorScheme = Plugin.Config.ColorSchemesList.FirstOrDefault(x => x._colorSchemeId == id);

                if (extendedColorScheme != null)
                {
                    __result = extendedColorScheme.ToColorScheme();
                }
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "GetSelectedColorScheme")]
        private class PGetSelectedColorScheme
        {
            internal static bool Prefix(ref ColorScheme __result)
            {
                var extendedColorScheme = Plugin.Config.ColorSchemesList.FirstOrDefault(x => x._colorSchemeId == Plugin.Config.SelectedColorSchemeId);

                if (extendedColorScheme != null)
                {
                    __result = extendedColorScheme.ToColorScheme();
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "GetSelectedColorSchemeIdx")]
        private class PGetSelectedColorSchemeIdx
        {
            internal static void Postfix(ref int __result)
            {
                for (int i = 0; i < Plugin.Config.ColorSchemesList.Count; i++)
                {
                    if (Plugin.Config.ColorSchemesList[i]._colorSchemeId == Plugin.Config.SelectedColorSchemeId)
                    {
                        __result = i + 4;
                        return;
                    }
                }

                // Returns the base color schemes
                if (__result >= 4)
                    __result += Plugin.Config.ColorSchemesList.Count;
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "SetColorSchemeForId")]
        private class PSetColorSchemeForId
        {
            internal static void Prefix(ColorScheme colorScheme)
            {
                if (Plugin.Config.ColorSchemesList.Any(x => x._colorSchemeId == colorScheme.colorSchemeId))
                {
                    for (int i = 0; i < Plugin.Config.ColorSchemesList.Count; i++)
                    {
                        if (Plugin.Config.ColorSchemesList[i]._colorSchemeId == colorScheme.colorSchemeId)
                        {
                            Plugin.Config.ColorSchemesList[i] = Utils.ToExtendedColorScheme(colorScheme);
                            return;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "selectedColorSchemeId", MethodType.Getter)]
        private class PSelectedColorSchemeIdGet
        {
            internal static void Postfix(ref string __result)
            {
                // Prevents the game from saving our color schemes selection
                if (Utils.IsCallByMethod("Save"))
                    return;

                __result = Plugin.Config.SelectedColorSchemeId;
            }
        }

        [HarmonyPatch(typeof(ColorSchemesSettings), "selectedColorSchemeId", MethodType.Setter)]
        private class PSelectedColorSchemeIdSet
        {
            internal static bool Prefix(string value, Dictionary<string, ColorScheme> ____colorSchemesDict)
            {
                bool existsInCollection = Plugin.Config.ColorSchemesList.Any(x => x._colorSchemeId == value);

                // Makes sure the color scheme exist, probably a bit overkill,
                // but at least not messing up the game's save file
                if (!existsInCollection && !____colorSchemesDict.ContainsKey(value))
                    return false;

                // Prevents the game from overriding our selected color scheme at launch time
                if (Utils.IsCallByMethod("LoadFromCurrentVersion"))
                    return true;

                Plugin.Config.SelectedColorSchemeId = value;

                if (existsInCollection)
                    return false;

                return true;
            }
        }
    }
}
