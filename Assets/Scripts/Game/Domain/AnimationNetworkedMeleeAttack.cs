using Fusion;
using Game.Domain;
using UnityEngine;
using Utils;
using Zenject;

namespace Game
{
    /// <summary>
    /// Ataque melee baseado na animação
    /// </summary>
    public class AnimationNetworkedMeleeAttack : NetworkBehaviour, IAttacker
    {
        public float attackRange = 2;
        public float cooldown = 2;
        public int damage = 1;
        public LayerMask enemyLayer;
        public string attackTrigger;
        [Networked] TickTimer _cooldown { get; set; }

        Animator animator;

        Vector3 attackDirection;

        [Inject]
        public void Setup(Animator animator)
        {
            this.animator = animator;
            animator.GetComponent<AnimatorEvents>().OnAnimationFinished += OnAnimationFinished;
        }

        void OnDestroy()
        {
            animator.GetComponent<AnimatorEvents>().OnAnimationFinished -= OnAnimationFinished;
        }

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

            if (_cooldown.ExpiredOrNotRunning(Runner) == false)
            {
                return;
            }
            attackDirection = direction;
            //Ativa o cooldown
            _cooldown = TickTimer.CreateFromSeconds(Runner, cooldown);

            animator.SetTrigger(attackTrigger);
        }

        public void OnAnimationFinished()
        {
            if (Object.HasStateAuthority == false) { return; }

            var hitSomething = Runner.LagCompensation.Raycast(transform.position, attackDirection, attackRange, Object.StateAuthority, out var hit, enemyLayer);

            if (hitSomething == false)
            {
                return;
            }

            var target = hit.Hitbox.Root.GetComponentInChildren<ITakesDamage>();

            if (target == null)
            {
                Debug.Log("Acertou alvo que não tem TakesDamage");
                return;
            }

            target.TakeDamage(damage, Object.StateAuthority);
        }
    }
}