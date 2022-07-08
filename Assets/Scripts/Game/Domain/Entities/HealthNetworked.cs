using Fusion;
using UnityEngine.Events;
using UniRx;
using UnityEngine;

namespace Game.Domain
{
    public interface IHealth
    {
        public IReadOnlyReactiveProperty<int> MaxHealth { get; }
        public IReadOnlyReactiveProperty<int> Health { get; }

        public IReadOnlyReactiveProperty<bool> IsDead { get; }
    }

    public class HealthNetworked : NetworkBehaviour, IHealth, ITakesDamage
    {
        public int startingMaxHealth = 3;

        [Networked(OnChanged = nameof(OnPropertyChanged))] int _maxHealth { get; set; }
        [Networked(OnChanged = nameof(OnPropertyChanged))] int _health { get; set; }
        [Networked(OnChanged = nameof(OnPropertyChanged))] int _lastAttackerId { get; set; }

        public IReadOnlyReactiveProperty<int> Health => healthProp;
        public IReadOnlyReactiveProperty<int> MaxHealth => maxHealthProp;
        public IReadOnlyReactiveProperty<bool> IsDead => isDeadProp;
        public IReadOnlyReactiveProperty<int> LastAttackerId => lastAttackerIdProp;

        readonly ReactiveProperty<bool> isDeadProp = new ReactiveProperty<bool>();
        readonly ReactiveProperty<int> healthProp = new ReactiveProperty<int>();
        readonly ReactiveProperty<int> maxHealthProp = new ReactiveProperty<int>();
        readonly ReactiveProperty<int> lastAttackerIdProp = new ReactiveProperty<int>();

        public UnityEvent<int> onChanged;
        public UnityEvent onDeath;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            };

            _maxHealth = startingMaxHealth;
            _health = _maxHealth;
        }

        public void TakeDamage(int damage, int attackerId)
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            _lastAttackerId = attackerId;
            _health -= damage;
        }

        static void OnPropertyChanged(Changed<HealthNetworked> info)
        {
            info.Behaviour.OnHealthChanged();
        }

        void OnHealthChanged()
        {
            lastAttackerIdProp.Value = _lastAttackerId;
            maxHealthProp.Value = _maxHealth;
            healthProp.Value = _health;
            onChanged?.Invoke(_health);

            isDeadProp.Value = _health <= 0;

            if (isDeadProp.Value)
            {
                onDeath?.Invoke();
            }
        }

        void OnDestroy()
        {
            lastAttackerIdProp.Dispose();
            isDeadProp.Dispose();
            healthProp.Dispose();
            maxHealthProp.Dispose();
        }
    }
}