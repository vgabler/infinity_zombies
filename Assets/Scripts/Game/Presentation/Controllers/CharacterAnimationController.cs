using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class CharacterAnimationController : MonoBehaviour
    {
        Animator animator;
        CharacterController controller;

        [Inject]
        public void Setup(CharacterController controller, Animator animator)
        {
            this.animator = animator;
            this.controller = controller;
        }
        private void Update()
        {
            var speed = controller.velocity.magnitude;

        }
    }
}