using Zenject;
using Services.Selector;
public class AppInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SelectorServiceIntaller.Install(Container);
    }
}
