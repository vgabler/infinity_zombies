using Fusion;
using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerMovement : NetworkBehaviour
    {
        public float speed = 5;
        private NetworkCharacterControllerPrototype controller;

        [Inject]
        public void Setup(NetworkCharacterControllerPrototype controller)
        {
            this.controller = controller;
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerNetworkInput data))
            {
                var dir = new Vector3(data.HorizontalMovement, 0, data.VerticalMovement).normalized;

                controller.Move(Runner.DeltaTime * speed * dir);
            }
        }
    }
}