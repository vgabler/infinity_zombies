using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

namespace Game.Domain
{
    public class ZombiesController : SimulationBehaviour
    {
        enum State { NotStarted, WaitingWave, WaitingAction, WaitingEndOfWave, Finished }

        public List<Transform> spawnPositions;

        IGameStateController gameStateController;
        StageDefinition stage;

        [Networked] int _zombieCount { get; set; }
        [Networked] int _currentWave { get; set; }
        [Networked] int _currentAction { get; set; }
        [Networked] State _state { get; set; } = State.NotStarted;

        [Networked] TickTimer _timer { get; set; }

        Wave CurrentWave => stage.waves[_currentWave];
        WaveAction CurrentAction => CurrentWave.actions[_currentAction];

        IDisposable sub;
        [Inject]
        public void Setup(IGameStateController gameStateController, IEntityManager<ZombieEntity> entityManager, StageDefinition stage)
        {
            this.gameStateController = gameStateController;
            this.stage = stage;
            sub = entityManager.Entities.ObserveCountChanged().Subscribe((count) => _zombieCount = count);
        }

        private void OnDestroy()
        {
            sub?.Dispose();
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (!(gameStateController.GameState.Value is GameStateRunning))
            {
                return;
            }

            switch (_state)
            {
                case State.NotStarted:
                    _currentWave = -1;
                    NextWave();
                    break;
                case State.WaitingWave:
                    if (_timer.ExpiredOrNotRunning(Runner))
                    {
                        NextAction();
                    }
                    break;
                case State.WaitingAction:
                    if (_timer.ExpiredOrNotRunning(Runner))
                    {
                        DoSpawns();
                        NextAction();
                    }
                    break;
                //A wave só termina quando todos os inimigos morrerem
                case State.WaitingEndOfWave:
                    //TODO verificar se morreu todo mundo
                    if (_zombieCount <= 0 || _timer.ExpiredOrNotRunning(Runner))
                    {
                        NextWave();
                    }
                    return;
                default:
                    return;
            }
        }

        void NextWave()
        {
            _currentWave++;

            if (_currentWave >= stage.waves.Count)
            {
                EndGame();
                return;
            }

            _currentAction = -1;
            _state = State.WaitingWave;

            var wave = stage.waves[_currentWave];

            _timer = TickTimer.CreateFromSeconds(Runner, wave.waveDelay > 0 ? wave.waveDelay : stage.defaultWaveDelay);
        }

        void NextAction()
        {
            _currentAction++;

            //Se terminou de spawnar todos, vai para a verificação se já morreram todos ou 
            if (_currentAction >= CurrentWave.actions.Count)
            {
                _state = State.WaitingEndOfWave;
                _timer = TickTimer.CreateFromSeconds(Runner, stage.waveTimeout);
                return;
            }

            _state = State.WaitingAction;

            _timer = TickTimer.CreateFromSeconds(Runner, CurrentAction.actionDelay > 0 ? CurrentAction.actionDelay : CurrentWave.defaultActionDelay);
        }
        void DoSpawns()
        {
            try
            {
                foreach (var index in CurrentAction.spawns)
                {
                    Spawn(index);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Falha não tratada!");
                Debug.LogError(e);
                return;
            }
        }

        Vector3 GetRandomSpawnPosition()
        {
            return spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)].position;
        }

        private void Spawn(int index)
        {
            var position = GetRandomSpawnPosition();
            Runner.Spawn(CurrentWave.prefabs[index], position, inputAuthority: PlayerRef.None);
        }

        void EndGame()
        {
            _state = State.Finished;
            gameStateController.EndGame();
        }
    }
}