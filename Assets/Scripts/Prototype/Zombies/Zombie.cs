using Fusion;
using System;
using UnityEngine;

namespace InfinityZombies.Prototype
{
    public class Zombie : NetworkBehaviour
    {
        public float deathRange = .5f;
        public float speed = 1;
        Vector3 dir;

        public UnityEngine.UI.Text textHealth;

        bool isDead;
        private NetworkCharacterControllerPrototype _cc;

        [Networked(OnChanged = nameof(OnLivesChanged))] int Lives { get; set; } = 3;

        private void Awake()
        {
            _cc = GetComponent<NetworkCharacterControllerPrototype>();
        }

        public override void Spawned()
        {
            dir = -transform.position.normalized;
            OnLivesChanged(Lives);
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false || isDead)
            {
                return;
            }

            _cc.Move(Runner.DeltaTime * speed * dir);

            if (transform.position.magnitude <= deathRange)
            {
                isDead = true;
                Runner.Despawn(Object);
            }
        }
        static void OnLivesChanged(Changed<Zombie> info)
        {
            info.Behaviour.OnLivesChanged(info.Behaviour.Lives);
            //playerInfo.Behaviour._overviewPanel.UpdateLives(playerInfo.Behaviour.Object.InputAuthority, playerInfo.Behaviour.Lives);
        }

        void OnLivesChanged(int lives)
        {
            textHealth.text = lives.ToString();
        }

        internal void HitByBullet(PlayerRef player)
        {
            // The bullet hit only triggers behaviour on the host.
            if (Object == null) return;
            if (Object.HasStateAuthority == false) return;

            //// If this hit was triggered by a projectile, the player who shot it gets points
            //// The player object is retrieved via the Runner.
            //if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            //{
            //    playerNetworkObject.GetComponent<PlayerDataNetworked>().AddToScore(_points);
            //}

            Lives--;

            if (Lives > 0)
            {
                return;
            }

            Runner.Despawn(Object);
        }
    }
}