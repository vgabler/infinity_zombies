using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;
using System;

namespace Game.Domain
{
    public class GameStateControllerImpl : NetworkBehaviour, IGameStateController, IDisposable
    {
        enum State { Starting, Running, Ending }

        [SerializeField] private float _startDelay = 4.0f;
        [SerializeField] private float _endDelay = 4.0f;
        [SerializeField] private float _gameSessionLength = 180.0f;

        public IReadOnlyReactiveProperty<IGameState> GameState => _gameState;
        ReactiveProperty<IGameState> _gameState = new ReactiveProperty<IGameState>(new GameStateStarting(4));

        //Precisa sincronizar essas duas variáveis
        [Networked] private TickTimer _timer { get; set; }
        [Networked] private State _state { get; set; } = State.Starting;

        private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();

        public override void Spawned()
        {
            //Se o jogo já começou, recupera os ids dos jogadores
            if (_state != State.Starting)
            {
                //foreach (var player in Runner.ActivePlayers)
                //{
                //    if (Runner.TryGetPlayerObject(player, out var playerObject) == false) continue;
                //    _playerDataNetworkedIds.Add(playerObject.GetComponent<PlayerDataNetworked>().Id);
                //}
            }

            // Inicializar o estado do jogo somente no host
            if (Object.HasStateAuthority == false) return;

            _gameState.Value = new GameStateStarting((int)_startDelay);
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
            _gameState.Value = new GameStateStarting(_timer.RemainingTime(Runner) ?? 0);

            //TODO verificar se tem jogadores vivos

            if (Object.HasStateAuthority == false) return;
            if (!_timer.ExpiredOrNotRunning(Runner)) return;

            _state = State.Running;
            _timer = TickTimer.CreateFromSeconds(Runner, _gameSessionLength);
        }

        void UpdateRunning()
        {
            //Atualiza a propriedade para notificar
            _gameState.Value = new GameStateRunning(_timer.RemainingTime(Runner) ?? 0);

            if (Object.HasStateAuthority == false) return;
            if (!_timer.ExpiredOrNotRunning(Runner)) return;

            _state = State.Ending;
            _timer = TickTimer.CreateFromSeconds(Runner, _endDelay);
        }

        void UpdateEnding()
        {
            if (!(_gameState.Value is GameStateEnded))
            {
                _gameState.Value = new GameStateEnded();
            }

            if (Object.HasStateAuthority == false) return;
            if (!_timer.ExpiredOrNotRunning(Runner)) return;

            Runner.Shutdown();
        }

        public void Dispose()
        {
            _gameState.Dispose();
        }
    }
}