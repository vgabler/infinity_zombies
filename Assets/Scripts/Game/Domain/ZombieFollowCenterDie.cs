using Fusion;
using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ZombieFollowCenterDie : NetworkBehaviour
    {
        public float deathRange = .5f;
        public float speed = 1;
        Vector3 dir;

        NetworkCharacterControllerPrototype characterController;

        ITakesDamage takesDamage;

        bool isDead;

        [Inject]
        public void Setup(NetworkCharacterControllerPrototype characterController, ITakesDamage takesDamage)
        {
            this.characterController = characterController;
            this.takesDamage = takesDamage;
        }

        public override void Spawned()
        {
            dir = -transform.position.normalized;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false || isDead)
            {
                return;
            }

            characterController.Move(Runner.DeltaTime * speed * dir);

            if (transform.position.magnitude <= deathRange)
            {
                takesDamage.TakeDamage(9999, Object.StateAuthority);
                isDead = true;
            }
        }
    }
}