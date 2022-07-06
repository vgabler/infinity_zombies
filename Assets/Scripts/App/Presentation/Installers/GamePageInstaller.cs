using InfinityZombies.Domain;
using Zenject;

namespace InfinityZombies.Presentation
{
    public class GamePageInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IExitCurrentMatch>().To<ExitCurrentMatchImpl>().AsCached();
        }
    }
}