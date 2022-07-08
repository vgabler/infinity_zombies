using Fusion;
using Game.Domain;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerAttackController : NetworkBehaviour
    {
        IHealth health;
        IAttacker attacker;

        [Inject]
        public void Setup(IHealth health, IAttacker attacker)
        {
            this.health = health;
            this.attacker = attacker;
        }

        public override void FixedUpdateNetwork()
        {
            if (health.IsDead.Value == true)
            {
                return;
            }

            if (GetInput<PlayerNetworkInput>(out var input) == false) return;

            if (input.Buttons.IsSet(PlayerButtons.Attack))
            {
                attacker.Attack(new Vector3(input.HorizontalFire, 0, input.VerticalFire));
            }
        }

    }
}