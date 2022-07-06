using InfinityZombies.Domain;
using Zenject;

public class HomePageInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IJoinExistingGame>().To<JoinExistingGame>().AsCached();
        Container.Bind<IStartNewGame>().To<StartNewGame>().AsCached();
    }
}
