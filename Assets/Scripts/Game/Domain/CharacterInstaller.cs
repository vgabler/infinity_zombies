using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class CharacterInstaller : MonoInstaller
    {
        //public GameObject health;
        //public GameObject takesDamage;
        public NetworkCharacterControllerPrototype controller;
        public override void InstallBindings()
        {
            Container.Bind<ITakesDamage>().FromInstance(GetComponentInChildren<ITakesDamage>()).AsSingle();
            Container.Bind<IHealth>().FromInstance(GetComponentInChildren<IHealth>()).AsSingle();
            Container.Bind<IAttacker>().FromInstance(GetComponentInChildren<IAttacker>()).AsSingle();
            //Container.BindInstance (health.GetComponent<IHealth>()).AsSingle();
            //Container.BindInstance(takesDamage.GetComponent<ITakesDamage>()).AsSingle();
            Container.BindInstance(controller).AsSingle();
        }
    }
}