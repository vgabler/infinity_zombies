using Fusion;
using Game.Domain;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerMovement : NetworkBehaviour
    {
        NetworkCharacterControllerPrototype _controller;
        IHealth _health;

        IDisposable subscription;

        [Inject]
        public void Setup(NetworkCharacterControllerPrototype controller, IHealth health)
        {
            _controller = controller;
            _health = health;
            subscription = health.IsDead.Subscribe(OnDeadChanged);
            OnDeadChanged(health.IsDead.Value);
        }

        void OnDestroy()
        {
            subscription?.Dispose();
        }

        void OnDeadChanged(bool isDead)
        {
            _controller.Controller.enabled = !isDead;
            _controller.enabled = !isDead;
        }

        public override void FixedUpdateNetwork()
        {
            if (_health.IsDead.Value)
            {
                return;
            }

            if (GetInput(out PlayerNetworkInput data))
            {
                _controller.Move(new Vector3(data.HorizontalMovement, 0, data.VerticalMovement).normalized);
            }
        }
    }
}