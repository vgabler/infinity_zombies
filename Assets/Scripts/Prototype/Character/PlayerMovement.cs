using Fusion;
using UnityEngine;

namespace InfinityZombies.Prototype
{
    public class PlayerMovement : NetworkBehaviour
    {
        public float speed = 5;
        private NetworkCharacterControllerPrototype _cc;

        private void Awake()
        {
            _cc = GetComponent<NetworkCharacterControllerPrototype>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerNetworkInput data))
            {
                var dir = new Vector3(data.Horizontal, 0, data.Vertical).normalized;

                _cc.Move(speed * dir * Runner.DeltaTime);
            }
        }
    }
}