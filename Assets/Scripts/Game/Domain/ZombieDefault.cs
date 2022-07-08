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

        PlayersController playersController;

        IHealth health;

        Transform currentTarget;

        [Inject]
        public void Setup(NetworkCharacterControllerPrototype characterController, IHealth health, PlayersController playersController)
        {
            this.characterController = characterController;
            this.playersController = playersController;
            this.health = health;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false || health.IsDead.Value)
            {
                return;
            }

            //TODO isso pode ser uma classe própria "ITargeter"
            //Busca um alvo
            foreach (var character in playersController.Characters.Values)
            {

                if (currentTarget == null ||
                Vector3.Distance(character.transform.position, transform.position) <
                Vector3.Distance(transform.position, currentTarget.position))
                {
                    currentTarget = character.transform;
                }
            }

            if (currentTarget == null)
            {
                Debug.LogWarning("Não tem player!");
                return;
            }

            var diff = currentTarget.transform.position - transform.position;

            if (diff.magnitude <= attackRange)
            {
                Debug.Log("On attack range!");
            }
            else
            {
                characterController.Move(Runner.DeltaTime * speed * diff.normalized);
            }
        }
    }
}