using UnityEngine;

namespace ExtendedColorSchemes
{
    public class ColorSchemeWithEditableName : PlayerSaveData.ColorScheme
    {
        public string colorSchemeName;

        public ColorSchemeWithEditableName(string colorSchemeName, string colorSchemeId, Color saberAColor, Color saberBColor, Color environmentColor0, Color environmentColor1, Color obstaclesColor)
            : base(colorSchemeId, saberAColor, saberBColor, environmentColor0, environmentColor1, obstaclesColor)
        {
            this.colorSchemeName = colorSchemeName;
        }
    }
}
