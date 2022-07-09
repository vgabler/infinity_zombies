using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class CharacterVisualsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(GetComponentInChildren<Animator>()).AsSingle();
        }
    }
}