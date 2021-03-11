using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace ExtendedColorSchemes
{
    internal class Config
    {
        public virtual string SelectedColorSchemeId { get; set; }

        [UseConverter(typeof(ListConverter<Utilities.ExtendedColorScheme>))]
        public virtual List<Utilities.ExtendedColorScheme> ColorSchemesList { get; set; } = new List<Utilities.ExtendedColorScheme>();

        public virtual void Changed() { }
    }
}