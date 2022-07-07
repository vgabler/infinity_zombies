using Fusion;
using UnityEngine.Events;

namespace Game.Domain
{
    public interface IHealth
    {
        public int CurrentHealth { get; }
    }

    public class HealthNetworked : NetworkBehaviour, IHealth
    {
        public int maxHealth = 3;
        [Networked(OnChanged = nameof(OnHealthChanged))] public int CurrentHealth { get; set; }
        bool IsDead => CurrentHealth <= 0;

        public UnityEvent<int> onChanged;
        public UnityEvent onDeath;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            CurrentHealth -= damage;
        }

        static void OnHealthChanged(Changed<HealthNetworked> info)
        {
            info.Behaviour.OnHealthChanged(info.Behaviour.CurrentHealth);
        }

        void OnHealthChanged(int lives)
        {
            onChanged?.Invoke(lives);
            if (IsDead)
            {
                onDeath?.Invoke();
            }
        }
    }
}