using System;
using System.Collections.Generic;
using IPA.Config.Data;
using IPA.Config.Stores;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace ExtendedColorSchemes.Converters
{
    internal class ColorSchemeWithEditableNameConverter : ValueConverter<ColorSchemeWithEditableName>
    {
        public override ColorSchemeWithEditableName? FromValue(Value? value, object parent)
        {
            if (value is not Map map)
            {
                return null;
            }
            if (!map.ContainsKey("colorSchemeName"))
            {
                map["colorSchemeName"] = Value.Text(string.Empty);
            }
            if (map["colorSchemeName"] is not Text colorSchemeName)
            {
                throw new ArgumentException("colorSchemeName must be a string!");
            }
            if (map["colorSchemeId"] is not Text colorSchemeId)
            {
                throw new ArgumentException("colorSchemeId must be a string!");
            }

            return new ColorSchemeWithEditableName(colorSchemeName.Value,
                colorSchemeId.Value,
                CustomValueTypeConverter<Color>.Deserialize(map["saberAColor"], parent),
                CustomValueTypeConverter<Color>.Deserialize(map["saberBColor"], parent),
                CustomValueTypeConverter<Color>.Deserialize(map["environmentColor0"], parent),
                CustomValueTypeConverter<Color>.Deserialize(map["environmentColor1"], parent),
                CustomValueTypeConverter<Color>.Deserialize(map["obstaclesColor"], parent));
        }

        public override Value? ToValue(ColorSchemeWithEditableName? obj, object parent)
        {
            if (obj == null)
            {
                return null;
            }

            return Value.From(new Dictionary<string, Value?>
            {
                {"colorSchemeName", Value.From(obj.colorSchemeName)},
                {"colorSchemeId", Value.From(obj.colorSchemeId)},
                {"saberAColor", CustomValueTypeConverter<Color>.Serialize(obj.saberAColor)},
                {"saberBColor", CustomValueTypeConverter<Color>.Serialize(obj.saberBColor)},
                {"environmentColor0", CustomValueTypeConverter<Color>.Serialize(obj.environmentColor0)},
                {"environmentColor1", CustomValueTypeConverter<Color>.Serialize(obj.environmentColor1)},
                {"obstaclesColor", CustomValueTypeConverter<Color>.Serialize(obj.obstaclesColor)}
            });
        }
    }

}
