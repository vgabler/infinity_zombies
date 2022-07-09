using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class CharacterInstaller : MonoInstaller
    {
        public NetworkCharacterControllerPrototype controller;
        public override void InstallBindings()
        {
            Container.Bind<ITakesDamage>().FromInstance(GetComponentInChildren<ITakesDamage>()).AsSingle();
            Container.Bind<IHealth>().FromInstance(GetComponentInChildren<IHealth>()).AsSingle();
            Container.Bind<IAttacker>().FromInstance(GetComponentInChildren<IAttacker>()).AsSingle();
            Container.BindInstance(controller).AsSingle();
        }
    }
}