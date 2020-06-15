using Zenject;

namespace Services.Selector
{
    public class SelectorServiceIntaller : Installer<SelectorServiceIntaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISelectorInput>().To<SelectorInput>()
                                    .FromNewComponentOnNewGameObject()
                                    .WithGameObjectName("StandAloneSelectorInput")
                                    .AsSingle();


            Container.Bind<ISelector>().To<Selector>().AsSingle().NonLazy();
        }
    }
}