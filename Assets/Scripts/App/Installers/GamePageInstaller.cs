using App.Domain;
using Zenject;

namespace App
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