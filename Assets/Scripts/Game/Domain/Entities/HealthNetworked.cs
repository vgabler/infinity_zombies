using Fusion;
using UnityEngine.Events;
using UniRx;

namespace Game.Domain
{
    public interface IHealth
    {
        public IReadOnlyReactiveProperty<int> Health { get; }

        public IReadOnlyReactiveProperty<bool> IsDead { get; }
    }

    public class HealthNetworked : NetworkBehaviour, IHealth, ITakesDamage
    {
        public int maxHealth = 3;
        [Networked(OnChanged = nameof(OnHealthChanged))] int _health { get; set; }

        IReadOnlyReactiveProperty<int> IHealth.Health => healthProp;

        IReadOnlyReactiveProperty<bool> IHealth.IsDead => isDeadProp;

        readonly ReactiveProperty<bool> isDeadProp = new ReactiveProperty<bool>();
        readonly ReactiveProperty<int> healthProp = new ReactiveProperty<int>();

        public UnityEvent<int> onChanged;
        public UnityEvent onDeath;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            };

            _health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            _health -= damage;
        }

        static void OnHealthChanged(Changed<HealthNetworked> info)
        {
            info.Behaviour.OnHealthChanged(info.Behaviour._health);
        }

        void OnHealthChanged(int health)
        {
            healthProp.Value = health;
            onChanged?.Invoke(health);

            isDeadProp.Value = health <= 0;

            if (isDeadProp.Value)
            {
                onDeath?.Invoke();
            }
        }

        void OnDestroy()
        {
            isDeadProp.Dispose();
            healthProp.Dispose();
        }
    }
}