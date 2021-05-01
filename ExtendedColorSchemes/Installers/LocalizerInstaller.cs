using Zenject;

namespace ExtendedColorSchemes.Installers
{
    internal class LocalizerInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Localizer>().AsSingle();
        }
    }
}
