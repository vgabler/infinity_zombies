using Fusion;
using System;
using Zenject;

namespace Game.Domain
{
    public class ZombieEntity : NetworkBehaviour
    {
        public int scoreValue = 1;

        IEntityManager<ZombieEntity> entityManager;

        IHealth health;
        ITakesDamage takesDamage;
        IScoreController scoreController;

        [Inject]
        public void Setup(IEntityManager<ZombieEntity> entityManager, IHealth health, ITakesDamage takesDamage, IScoreController scoreController)
        {
            this.entityManager = entityManager;
            this.health = health;
            this.takesDamage = takesDamage;
            this.scoreController = scoreController;
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (Object.HasStateAuthority && health.IsDead.Value)
            {
                scoreController.AddScore(takesDamage.LastAttackerId.Value, scoreValue);

                Runner.Despawn(Object);
            }
        }

        public override void Spawned()
        {
            base.Spawned();
            entityManager.EntitySpawned(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            entityManager.EntityDespawned(this);
        }
    }
}
