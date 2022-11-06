using UnityEngine;

namespace ExtendedColorSchemes;

public class ColorSchemeWithEditableName : PlayerSaveData.ColorScheme
{
    public readonly string ColorSchemeName;
    public readonly bool SupportsEnvironmentColorBoost;

    public ColorSchemeWithEditableName(string colorSchemeName, string colorSchemeId, Color saberAColor, Color saberBColor, Color environmentColor0, Color environmentColor1, Color obstaclesColor, bool supportsEnvironmentColorBoost, Color environmentColor0Boost, Color environmentColor1Boost)
        : base(colorSchemeId, saberAColor, saberBColor, environmentColor0, environmentColor1, obstaclesColor, environmentColor0Boost, environmentColor1Boost)
    {
        ColorSchemeName = colorSchemeName;
        SupportsEnvironmentColorBoost = supportsEnvironmentColorBoost;
    }
}