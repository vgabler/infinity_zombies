using UnityEngine;
using UniRx;
using Fusion;
using System;
using Zenject;

namespace Game.Domain
{
    /// <summary>
    /// Controla o estado atual do jogo, com os temporizadores.
    /// </summary>
    public class GameStateControllerImpl : NetworkBehaviour, IGameStateController, IDisposable
    {
        enum State { Starting, Running, Ending }

        [SerializeField] private float _startDelay = 4.0f;
        [SerializeField] private float _endDelay = 4.0f;
        [SerializeField] private float _gameSessionLength = 180.0f;

        public IReadOnlyReactiveProperty<IGameState> GameState => gameState;
        ReactiveProperty<IGameState> gameState = new ReactiveProperty<IGameState>(new GameStateStarting(4));

        //Precisa sincronizar essas duas variáveis
        [Networked] private TickTimer _timer { get; set; }
        [Networked] private State _state { get; set; } = State.Starting;

        IEntityManager<PlayerEntity> entityManager;

        [Inject]
        public void Setup(IEntityManager<PlayerEntity> entityManager)
        {
            this.entityManager = entityManager;
        }

        public override void Spawned()
        {
            // Inicializar o estado do jogo somente no host
            if (Object.HasStateAuthority == false) return;

            gameState.Value = new GameStateStarting((int)_startDelay);
            _state = State.Starting;
            _timer = TickTimer.CreateFromSeconds(Runner, _startDelay);
        }

        public override void FixedUpdateNetwork()
        {
            switch (_state)
            {
                case State.Starting:
                    UpdateStarting();
                    break;
                case State.Running:
                    UpdateRunning();
                    break;
                case State.Ending:
                    UpdateEnding();
                    break;
            }
        }

        void UpdateStarting()
        {
            //Atualiza a propriedade para notificar
            gameState.Value = new GameStateStarting(_timer.RemainingTime(Runner) ?? 0);

            //TODO verificar se tem jogadores vivos

            if (Object.HasStateAuthority == false) return;
            if (!_timer.ExpiredOrNotRunning(Runner)) return;

            _state = State.Running;
            _timer = TickTimer.CreateFromSeconds(Runner, _gameSessionLength);
        }

        void UpdateRunning()
        {
            //Atualiza a propriedade para notificar
            gameState.Value = new GameStateRunning(_timer.RemainingTime(Runner) ?? 0);

            if (Object.HasStateAuthority == false) return;
            if (_timer.ExpiredOrNotRunning(Runner) == false && IsAnyPlayerAlive()) return;

            EndGame();
        }

        //TODO isso pode ser reativo; fazer um react na lista, e depois react em cada propriedade IHealth
        bool IsAnyPlayerAlive()
        {
            //Verificar se todos os jogadores estão vivos

            foreach (var c in entityManager.Entities)
            {
                var health = c.Context.Container.Resolve<IHealth>();

                if (health == null || health.IsDead.Value == true)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        void UpdateEnding()
        {
            if (!(gameState.Value is GameStateEnded))
            {
                gameState.Value = new GameStateEnded();
            }

            if (Object.HasStateAuthority == false) return;
            if (!_timer.ExpiredOrNotRunning(Runner)) return;

            Runner.Shutdown();
        }

        public void EndGame()
        {
            _timer = TickTimer.CreateFromSeconds(Runner, _endDelay);
            _state = State.Ending;
        }

        public void Dispose()
        {
            gameState.Dispose();
        }
    }
}