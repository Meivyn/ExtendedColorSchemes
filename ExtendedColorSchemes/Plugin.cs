using ExtendedColorSchemes.Installers;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using System.Reflection;
using Conf = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace ExtendedColorSchemes
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; }
        internal static Config Config { get; private set; }

        private readonly Harmony _harmony;
        private const string HarmonyID = "com.meivyn.extendedcolorschemes";

        [Init]
        public Plugin(IPALogger logger, Conf conf, Zenjector zenjector)
        {
            Log = logger;
            Config = conf.Generated<Config>();
            _harmony = new Harmony(HarmonyID);

            zenjector.OnApp<LocalizerInstaller>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnExit]
        public void OnApplicationQuit()
        {

        }
    }
}
