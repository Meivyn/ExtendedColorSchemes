using System.Reflection;
using ExtendedColorSchemes.HarmonyPatches;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using IPALogger = IPA.Logging.Logger;

namespace ExtendedColorSchemes
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        private readonly PluginMetadata _metadata;
        private readonly Harmony _harmony;

        internal static IPALogger Log { get; private set; } = null!;
        internal static PluginConfig Config { get; private set; } = null!;

        [Init]
        public Plugin(IPALogger logger, PluginMetadata metadata, Config config)
        {
            Log = logger;
            Config = config.Generated<PluginConfig>();
            _metadata = metadata;
            _harmony = new Harmony("com.meivyn.ExtendedColorSchemes");
        }

        [OnStart]
        public void OnStart()
        {
            MethodInfo? original = AccessTools.PropertySetter(typeof(ColorSchemesSettings), nameof(ColorSchemesSettings.selectedColorSchemeId));
            MethodInfo? prefix = AccessTools.Method(typeof(ColorSchemesSettingsSelectedColorSchemeIdSetter), nameof(ColorSchemesSettingsSelectedColorSchemeIdSetter.Prefix));

            // We must first patch the setter, otherwise the subsequent calls in
            // LoadFromCurrentVersion are not using our patch.
            _harmony.Patch(original, new HarmonyMethod(prefix));
            _harmony.PatchAll(_metadata.Assembly);
        }

        [OnExit]
        public void OnExit()
        {
        }
    }
}
