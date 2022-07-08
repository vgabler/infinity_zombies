using Fusion;
using System;
using Zenject;

namespace Game.Domain
{
    public class ZombieEntity : NetworkBehaviour
    {
        IEntityManager<ZombieEntity> entityManager;

        IHealth health;

        [Inject]
        public void Setup(IEntityManager<ZombieEntity> entityManager, IHealth health)
        {
            this.entityManager = entityManager;
            this.health = health;
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (Object.HasStateAuthority && health.IsDead.Value)
            {
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
