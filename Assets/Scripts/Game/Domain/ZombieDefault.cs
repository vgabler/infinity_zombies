using Fusion;
using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    /// <summary>
    /// O zumbi padrão vai simplesmente correr atrás do player. Ao chegar no alcance, deve atacar
    /// </summary>
    public class ZombieDefault : NetworkBehaviour
    {
        public float attackRange = 1f;
        public float speed = 1;

        NetworkCharacterControllerPrototype characterController;

        IEntityManager<PlayerEntity> playerEntityManager;

        IHealth health;
        IAttacker attacker;

        Transform currentTarget;

        [Inject]
        public void Setup(NetworkCharacterControllerPrototype characterController, IHealth health, IEntityManager<PlayerEntity> playerEntityManager, IAttacker attacker)
        {
            this.characterController = characterController;
            this.playerEntityManager = playerEntityManager;
            this.health = health;
            this.attacker = attacker;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false || health.IsDead.Value)
            {
                return;
            }

            //TODO isso pode ser uma classe própria "ITargeter"
            //Busca um alvo
            foreach (var player in playerEntityManager.Entities)
            {
                var playerTransform = player.transform;
                var playerHealth = player.Context.Container.Resolve<IHealth>();

                if (playerHealth.IsDead.Value == true)
                {
                    if (currentTarget == playerTransform)
                    {
                        currentTarget = null;
                    }
                    continue;
                }


                if (currentTarget == null ||
                Vector3.Distance(playerTransform.position, transform.position) <
                Vector3.Distance(transform.position, currentTarget.position))
                {
                    currentTarget = playerTransform;
                }
            }

            if (currentTarget == null)
            {
                return;
            }

            var diff = currentTarget.transform.position - transform.position;

            if (diff.magnitude <= attackRange)
            {
                attacker.Attack();
            }
            else
            {
                characterController.Move(Runner.DeltaTime * speed * diff.normalized);
            }
        }
    }
}