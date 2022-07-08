using Fusion;
using Zenject;

namespace Game.Domain
{
    public class PlayerEntity : NetworkBehaviour
    {
        IEntityManager<PlayerEntity> entityManager;

        public Context Context { get; private set; }

        [Inject]
        public void Setup(IEntityManager<PlayerEntity> entityManager, Context context)
        {
            this.entityManager = entityManager;
            this.Context = context;
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