using Fusion;
using Game.Domain;
using Zenject;
using UniRx;
using Auth.Domain;
using System;
using System.Threading.Tasks;

namespace Game
{
    public interface IScoreController
    {
        IReadOnlyReactiveProperty<int> HighScore { get; }
        IReadOnlyReactiveProperty<int> Score { get; }
        void AddScore(int playerId, int scoreValue);
    }
    public class ScoreControllerImpl : NetworkBehaviour, IScoreController
    {
        static string HIGH_SCORE_KEY = "HIGH_SCORE";
        [Networked(OnChanged = nameof(OnPropertyChanged))] int _score { get; set; }

        public IReadOnlyReactiveProperty<int> HighScore => highScore;

        readonly ReactiveProperty<int> highScore = new ReactiveProperty<int>();

        public IReadOnlyReactiveProperty<int> Score => score;

        readonly ReactiveProperty<int> score = new ReactiveProperty<int>();

        IGameStateController controller;
        ILocalStorage localStorage;

        IDisposable gameStateSub;

        [Inject]
        public void Setup(IGameStateController controller, ILocalStorage localStorage)
        {
            this.controller = controller;
            this.localStorage = localStorage;

            gameStateSub = controller.GameState.Subscribe((state) =>
            {
                if (state is GameStateEnded)
                    SaveHighScore();
            });

            //Ao iniciar já busca o highscore salvo
            Task.Run(async () =>
            {
                var hs = await localStorage.Get<int>(HIGH_SCORE_KEY);

                UnityMainThreadDispatcher.Instance().Enqueue(() => highScore.Value = hs);
            });
        }

        void OnDestroy()
        {
            score.Dispose();
            gameStateSub.Dispose();
        }

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            _score = 0;
        }

        static void OnPropertyChanged(Changed<ScoreControllerImpl> info)
        {
            info.Behaviour.score.Value = info.Behaviour._score;
        }

        /// <summary>
        /// Ao terminar o jogo, deve salvar localmente qual foi o melhor score
        /// </summary>
        void SaveHighScore()
        {
            //Se pontuou mais que a última vez, salva;
            if (score.Value > highScore.Value)
            {
                highScore.Value = score.Value;
                Task.Run(() => localStorage.Set(HIGH_SCORE_KEY, score.Value));
            }
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