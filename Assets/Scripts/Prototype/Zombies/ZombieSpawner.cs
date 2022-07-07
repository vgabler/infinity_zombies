using Fusion;
using UnityEngine;

namespace InfinityZombies.Prototype
{
    public class ZombieSpawner : NetworkBehaviour
    {
        public NetworkPrefabRef zombiePrefab;
        public float _minSpawnDelay = 2;
        public float _maxSpawnDelay = 4;
        // The TickTimer controls the time lapse between spawns.
        [Networked] private TickTimer _spawnDelay { get; set; }

        public GameStateController controller;

        bool initialized;

        void SetSpawnDelay()
        {
            // Chose a random amount of time until the next spawn.
            var time = Random.Range(_minSpawnDelay, _maxSpawnDelay);

            _spawnDelay = TickTimer.CreateFromSeconds(Runner, time);
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false || controller.State != GameStateController.GameState.Running)
            {
                return;
            }

            if (!initialized)
            {
                SetSpawnDelay();
                initialized = true;
            }

            Spawn();
        }

        private void Spawn()
        {
            if (_spawnDelay.Expired(Runner) == false) return;

            var position = transform.position;

            Runner.Spawn(zombiePrefab, position, inputAuthority: PlayerRef.None);

            // Sets the delay until the next spawn.
            SetSpawnDelay();
        }
    }
}