using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace ExtendedColorSchemes
{
    internal class PluginConfig
    {
        public virtual string? selectedColorSchemeId { get; set; }

        [NonNullable, UseConverter(typeof(ListConverter<ColorSchemeWithEditableName, Converters.ColorSchemeWithEditableNameConverter>))]
        public virtual List<ColorSchemeWithEditableName> colorSchemes { get; set; } = new();

        public virtual void Changed() { }
    }
}
