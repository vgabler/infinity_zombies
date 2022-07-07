using Game.Domain;
using InfinityZombies.Domain;
using UnityEngine;
using Zenject;

namespace InfinityZombies
{
    public class GamePageInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IExitCurrentMatch>().To<ExitCurrentMatchImpl>().AsCached();
        }
    }
}