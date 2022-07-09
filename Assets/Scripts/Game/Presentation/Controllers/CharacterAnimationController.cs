using Fusion;
using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class CharacterAnimationController : NetworkBehaviour
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

        public override void FixedUpdateNetwork()
        {
            if (health.IsDead.Value)
            {
                return;
            }

            var speed = controller.Velocity.magnitude / controller.maxSpeed;

            animator.SetFloat("Speed", speed);

            if (Runner.IsForward == false)
            {
                return;
            }

            if (GetInput(out PlayerNetworkInput input))
            {
                animator.SetBool("Shooting", input.Buttons.IsSet(PlayerButtons.Attack));
            }
        }
    }
}