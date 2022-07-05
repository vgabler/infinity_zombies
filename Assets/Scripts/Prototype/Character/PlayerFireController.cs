
using Fusion;
using UnityEngine;

namespace InfinityZombies.Prototype
{
    public class PlayerFireController : NetworkBehaviour
    {
        // Game Session AGNOSTIC Settings
        [SerializeField] private float _delayBetweenShots = 0.2f;
        [SerializeField] private NetworkPrefabRef _bullet = NetworkPrefabRef.Empty;

        // Local Runtime references
        private PlayerMovement _playerController = null;

        // Game Session SPECIFIC Settings
        [Networked] private NetworkButtons _buttonsPrevious { get; set; }
        [Networked] private TickTimer _shootCooldown { get; set; }

        public override void Spawned()
        {
            // --- Host & Client
            // Set the local runtime references.
            _playerController = GetComponent<PlayerMovement>();
        }

        public override void FixedUpdateNetwork()
        {
            //// Bail out of FUN() if this spaceship does not currently accept input
            //if (_playerController.AcceptInput == false) return;

            // Bail out of FUN() if this Client does not have InputAuthority over this object or
            // if no PlayerNetworkInput struct is available for this tick
            if (GetInput<PlayerNetworkInput>(out var input) == false) return;

            Fire(input);
        }

        // Checks the Buttons in the input struct against their previous state to check
        // if the fire button was just pressed.
        private void Fire(PlayerNetworkInput input)
        {
            var pressed = input.Buttons.GetPressed(_buttonsPrevious);

            if (pressed.WasPressed(_buttonsPrevious, PlayerButtons.Attack))
            {
                SpawnBullet();
            }

            _buttonsPrevious = input.Buttons;
        }

        // Spawns a bullet which will be travelling in the direction the player is facing
        private void SpawnBullet()
        {
            if (_shootCooldown.ExpiredOrNotRunning(Runner) == false) return;

            Runner.Spawn(_bullet, transform.position, transform.rotation, Object.InputAuthority);

            _shootCooldown = TickTimer.CreateFromSeconds(Runner, _delayBetweenShots);
        }
    }
}