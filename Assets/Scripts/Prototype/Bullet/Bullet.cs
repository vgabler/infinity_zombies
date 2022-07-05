using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;


namespace InfinityZombies.Prototype
{
    public class Bullet : NetworkBehaviour
    {
        // The settings
        [SerializeField] private float _maxLifetime = 3.0f;
        [SerializeField] private float _speed = 200.0f;
        [SerializeField] private LayerMask _zombieLayer;

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
            //var list = new List<LagCompensatedHit>();

            //var hits = Runner.LagCompensation.OverlapSphere(transform.position, 1, Object.InputAuthority, list, _zombieLayer);


            //if (hits <= 0)
            //{
            //    return false;
            //}

            //foreach (var hit in list)
            //{
            //    print("Hit something: " + hit.GameObject.name);
            //    var zombie = hit.GameObject.GetComponent<Zombie>();
            //    zombie.HitByBullet(Object.InputAuthority);
            //}

            var hitSomething = Runner.LagCompensation.Raycast(transform.position, _direction, _speed * Runner.DeltaTime,
                Object.InputAuthority, out var hit, _zombieLayer);

            if (hitSomething == false) return false;

            print("Hit something: " + hit.GameObject.name);

            var h = hit.Hitbox.Root.GetComponent<Health>();
            h.TakeDamage(1);

            return true;
        }
    }
}