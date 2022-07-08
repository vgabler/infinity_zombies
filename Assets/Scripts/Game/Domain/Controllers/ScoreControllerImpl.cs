using Fusion;
using Game.Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

namespace Game
{
    public interface IScoreController
    {
        IReadOnlyReactiveProperty<int> Score { get; }
        void AddScore(int playerId, int scoreValue);
    }
    public class ScoreControllerImpl : NetworkBehaviour, IScoreController
    {
        [Networked(OnChanged = nameof(OnPropertyChanged))] int _score { get; set; }

        public IReadOnlyReactiveProperty<int> Score => score;

        readonly ReactiveProperty<int> score = new ReactiveProperty<int>();

        void OnDestroy() { score.Dispose(); }

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            _score = 0;
        }

        static void OnPropertyChanged(Changed<ScoreControllerImpl> info)
        {
            info.Behaviour.score.Value = info.Behaviour._score;
        }

        public void AddScore(int playerId, int scoreValue)
        {
            if (Object.HasStateAuthority == false) return;

            _score += scoreValue;

            if (Runner.TryGetPlayerObject(playerId, out var playerObj))
            {
                var playerScorer = playerObj.GetComponent<GameObjectContext>().Container.Resolve<IScorer>();

                if (playerScorer != null)
                {
                    playerScorer.AddScore(scoreValue);
                }
            }
        }
    }
}