using Game.Domain;
using Zenject;
using Utils;
using Fusion;

namespace Game
{
    public class ZombieOnDeath : NetworkBehaviour
    {
        IZombiesController zombiesController;
        IHealth health;

        [Inject]
        public void Setup(IHealth health, IZombiesController zombiesController)
        {
            this.zombiesController = zombiesController;
            this.health = health;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (health.IsDead.Value == true)
            {
                zombiesController.ZombieDied(Object.Id);
            }
        }
    }
}