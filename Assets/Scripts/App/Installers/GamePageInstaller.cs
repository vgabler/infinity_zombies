using InfinityZombies.Domain;
using Zenject;

namespace InfinityZombies
{
    public class GamePageInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IExitCurrentMatch>().To<ExitCurrentMatchImpl>().AsCached();
            Container.Bind<IRetryCurrentMatch>().To<RetryCurrentMatchImpl>().AsCached();
        }
    }
}