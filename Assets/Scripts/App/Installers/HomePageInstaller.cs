using InfinityZombies.Domain;
using Zenject;

namespace InfinityZombies.Presentation
{
    public class HomePageInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IJoinExistingMatch>().To<JoinExistingMatchImpl>().AsCached();
            Container.Bind<IStartNewMatch>().To<StartNewMatchImpl>().AsCached();
        }
    }
}