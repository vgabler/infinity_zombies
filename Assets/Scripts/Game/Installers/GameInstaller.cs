using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        public GameObject gameStateController;
        public override void InstallBindings()
        {
            var controller = gameStateController.GetComponent<IGameStateController>();

            Container.BindInterfacesTo<GameStateControllerImpl>().FromInstance(controller).AsSingle();
        }
    }
}