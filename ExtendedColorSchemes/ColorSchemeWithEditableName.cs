using UnityEngine;

namespace ExtendedColorSchemes
{
    public class ColorSchemeWithEditableName : PlayerSaveData.ColorScheme
    {
        public string colorSchemeName;
        public bool supportsEnvironmentColorBoost;
        public Color environmentColor0Boost;
        public Color environmentColor1Boost;

        public ColorSchemeWithEditableName(string colorSchemeName, string colorSchemeId, Color saberAColor, Color saberBColor, Color environmentColor0, Color environmentColor1, Color obstaclesColor, bool supportsEnvironmentColorBoost, Color environmentColor0Boost, Color environmentColor1Boost)
            : base(colorSchemeId, saberAColor, saberBColor, environmentColor0, environmentColor1, obstaclesColor)
        {
            this.colorSchemeName = colorSchemeName;
            this.supportsEnvironmentColorBoost = supportsEnvironmentColorBoost;
            this.environmentColor0Boost = environmentColor0Boost;
            this.environmentColor1Boost = environmentColor1Boost;
        }
    }
}
