# ExtendedColorSchemes

ExtendedColorSchemes is a Beat Saber mod that allows the user to have many more customizable color schemes than currently available in the game. There are 8 additional color schemes added by default, but you can add (or delete some) by carefully editing the configuration file here: `Beat Saber\UserData\ExtendedColorSchemes.json`.

Here's a quick summary of the config:


| Property                | Description                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
|-------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `selectedColorSchemeId` | Should avoid to edit, that is used to save the last selected color scheme.                                                                                                                                                                                                                                                                                                                                                                                           |
| `colorSchemes`          | That's where the color schemes are saved. You can set a customizable `colorSchemeName` and edit the color values (`r`, `g`, and `b`) more precisely than using the color picker. `a` is used for transparency, hence that should stay to `1`. Keep in mind that those values have to be in base 1, so divide RGB values by 255.<br/><br/>For example, if you want to use a color of RGB(128, 0, 255), it would be:<br/>`"r": 0.5019607,`<br/>`"g": 0,`<br/>`"b": 1,` |

## Acknowledgements

To help and contribute to the mod, open an [issue](https://github.com/Meivyn/ExtendedColorSchemes/issues) when a bug is discovered, so it could be fixed as soon as possible. Your help is really appreciated.

Special thanks to Kyle 1413 and DaNike for all their precious help.
