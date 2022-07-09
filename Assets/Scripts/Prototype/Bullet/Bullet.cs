using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Game.Domain;
using UnityEngine;
using Zenject;

namespace InfinityZombies.Prototype
{
    public class Bullet : NetworkBehaviour
    {
        // The settings
        [SerializeField] private float _maxLifetime = 3.0f;
        [SerializeField] private float _speed = 200.0f;
        [SerializeField] private LayerMask _enemyLayer;

        // The direction in which the bullet travels.
        [Networked] private Vector3 _direction { get; set; }

        // The countdown for a bullet lifetime.
        [Networked] private TickTimer _currentLifetime { get; set; }

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            // The network parameters get initializes by the host. These will be propagated to the clients since the
            // variables are [Networked]
            _direction = transform.forward;
            _currentLifetime = TickTimer.CreateFromSeconds(Runner, _maxLifetime);
        }

        public override void FixedUpdateNetwork()
        {
            // If the bullet has not hit, moves forward.
            if (HitSomething() == false)
            {
                transform.Translate(_speed * Runner.DeltaTime * _direction, Space.World);
            }
            else
            {
                Runner.Despawn(Object);
                return;
            }

            CheckLifetime();
        }

        // If the bullet has exceeded its lifetime, it gets destroyed
        private void CheckLifetime()
        {
            if (_currentLifetime.Expired(Runner) == false) return;

            Runner.Despawn(Object);
        }

        // Check if the bullet will hit an asteroid in the next tick.
        private bool HitSomething()
        {
            var hitSomething = Runner.LagCompensation.Raycast(transform.position, _direction, _speed * Runner.DeltaTime,
                Object.InputAuthority, out var hit, _enemyLayer);

            if (hitSomething == false) return false;

            var ctx = hit.Hitbox.Root.GetComponent<GameObjectContext>();
            var target = ctx.Container.Resolve<ITakesDamage>();

            target.TakeDamage(1, Object.InputAuthority);

            return true;
        }
    }
}