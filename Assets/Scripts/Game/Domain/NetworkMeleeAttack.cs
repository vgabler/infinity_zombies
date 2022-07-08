using Fusion;
using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    /// <summary>
    /// Ataque simples, instantâneo, só com cooldown
    /// </summary>
    public class NetworkMeleeAttack : NetworkBehaviour, IAttacker
    {
        public float cooldown = 2;
        public int damage = 1;
        public LayerMask enemyLayer;
        [Networked] TickTimer _timer { get; set; }

        public void Attack()
        {
            Attack(transform.forward);
        }

        public void Attack(Vector3 direction)
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (!_timer.ExpiredOrNotRunning(Runner))
            {
                return;
            }

            //Ativa o cooldown
            _timer = TickTimer.CreateFromSeconds(Runner, cooldown);

            var hitSomething = Runner.LagCompensation.Raycast(transform.position, direction, 2, Object.StateAuthority, out var hit, enemyLayer);

            if (hitSomething == false)
            {
                return;
            }

            print("Melee hit on: " + hit.GameObject.name);

            var target = hit.Hitbox.Root.GetComponentInChildren<ITakesDamage>();

            //var ctx = hit.Hitbox.Root.GetComponent<GameObjectContext>();
            //var target = ctx.Container.Resolve<ITakesDamage>();

            if (target == null)
            {
                Debug.Log("Acertou alvo que não tem TakesDamage");
                return;
            }

            target.TakeDamage(damage);
        }
    }
}