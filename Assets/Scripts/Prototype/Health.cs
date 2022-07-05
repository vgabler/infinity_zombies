using Fusion;
using UnityEngine.Events;

namespace InfinityZombies.Prototype
{
    public class Health : NetworkBehaviour
    {
        public int maxHealth = 3;
        [Networked(OnChanged = nameof(OnLivesChanged))] int Lives { get; set; }
        bool IsDead => Lives <= 0;

        public UnityEvent<int> onChanged;
        public UnityEvent onDeath;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            Lives = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            Lives -= damage;
        }

        static void OnLivesChanged(Changed<Health> info)
        {
            info.Behaviour.OnLivesChanged(info.Behaviour.Lives);
            //playerInfo.Behaviour._overviewPanel.UpdateLives(playerInfo.Behaviour.Object.InputAuthority, playerInfo.Behaviour.Lives);
        }

        void OnLivesChanged(int lives)
        {
            onChanged?.Invoke(lives);
            if (IsDead)
            {
                onDeath?.Invoke();
            }
        }
    }
}