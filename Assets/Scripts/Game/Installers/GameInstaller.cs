using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        public GameObject gameStateController;
        public GameObject zombiesController;
        public GameObject playersController;
        public StageDefinition stage;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameStateControllerImpl>().FromComponentOn(gameStateController).AsSingle();
            Container.BindInterfacesTo<ZombiesController>().FromComponentOn(zombiesController).AsSingle();
            Container.Bind<PlayersController>().FromComponentOn(playersController).AsSingle();
            Container.BindInstance(stage).AsSingle();
        }
    }
}