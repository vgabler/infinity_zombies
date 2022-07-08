using Fusion;
using UnityEngine;

namespace Game
{
    public class NetworkRangedAttack : NetworkBehaviour, IAttacker
    {
        public float attackCooldown = 0.2f;
        public NetworkPrefabRef bullet = NetworkPrefabRef.Empty;
        public Transform gunTip;

        [Networked] private TickTimer _shootCooldown { get; set; }

        public void Attack()
        {
            Attack(gunTip.forward);
        }

        public void Attack(Vector3 direction)
        {
            if (Object.HasInputAuthority == false || _shootCooldown.ExpiredOrNotRunning(Runner) == false)
            {
                return;
            }

            Runner.Spawn(bullet, gunTip.position, Quaternion.LookRotation(direction, gunTip.up), Object.InputAuthority);

            _shootCooldown = TickTimer.CreateFromSeconds(Runner, attackCooldown);
        }
    }
}