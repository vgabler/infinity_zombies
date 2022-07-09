using Game.Domain;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Game.Presentation
{
    public class GameUIController : ReactiveMonoBehaviour
    {
        public GameObject startingUI;
        public Text startingUITimer;
        public GameObject endingUI;
        public GameObject runningUI;
        public Text runningUITimer;
        public Text runningUIzombiesCount;
        public Text runningUITotalScore;

        [Inject]
        public void Setup(IGameStateController gameStateController, IEntityManager<ZombieEntity> entityManager, IScoreController scoreController)
        {
            SubscribePropertyUpdateNow(gameStateController.GameState, OnGameStateChanged);
            Subscribe(entityManager.Entities.ObserveCountChanged(true), (count) => runningUIzombiesCount.text = $"Zombies left: {count}");
            Subscribe(scoreController.Score, (score) => runningUITotalScore.text = $"Total score: {score}");
        }

        private void OnGameStateChanged(IGameState obj)
        {
            if (obj is GameStateStarting starting)
            {
                startingUI.SetActive(true);
                runningUI.SetActive(false);
                endingUI.SetActive(false);

                var secondsRemaining = starting.SecondsRemaining;

                startingUITimer.text = $"{Mathf.RoundToInt(secondsRemaining)}";
            }
            else if (obj is GameStateRunning running)
            {
                startingUI.SetActive(false);
                runningUI.SetActive(true);
                endingUI.SetActive(false);

                var secondsRemaining = running.SecondsRemaining;

                runningUITimer.text = $"{Mathf.RoundToInt(secondsRemaining)}";
            }
            else if (obj is GameStateEnded)
            {
                startingUI.SetActive(false);
                runningUI.SetActive(false);
                endingUI.SetActive(true);

            }
        }
    }
}