using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        public GameObject gameStateController;
        public GameObject zombiesController;
        public StageDefinition stage;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BasicEntityManager<PlayerEntity>>().AsSingle();
            Container.BindInterfacesTo<BasicEntityManager<ZombieEntity>>().AsSingle();

            Container.BindInstance(GetComponentInChildren<IScoreController>()).AsSingle();

            Container.BindInterfacesTo<GameStateControllerImpl>().FromComponentOn(gameStateController).AsSingle();
            Container.BindInterfacesTo<ZombiesController>().FromComponentOn(zombiesController).AsSingle();
            Container.BindInstance(stage).AsSingle();
        }
    }
}