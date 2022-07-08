using Game.Domain;
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
        IHealth health;

        [Inject]
        public void Setup(NetworkCharacterControllerPrototype controller, Animator animator, IHealth health)
        {
            this.animator = animator;
            this.controller = controller;
            this.health = health;
        }
        private void Update()
        {
            if (health.IsDead.Value)
            {
                return;
            }

            var speed = controller.Velocity.magnitude / controller.maxSpeed;

            animator.SetFloat("Speed", speed);
        }
    }
}