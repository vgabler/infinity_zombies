using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class CharacterAnimationController : MonoBehaviour
    {
        Animator animator;
        NetworkCharacterControllerPrototype controller;

        [Inject]
        public void Setup(NetworkCharacterControllerPrototype controller, Animator animator)
        {
            this.animator = animator;
            this.controller = controller;
        }
        private void Update()
        {
            var speed = controller.Velocity.magnitude / controller.maxSpeed;

            animator.SetFloat("Speed", speed);
        }
    }
}