using Zenject;
using Utils;
using UnityEngine.UI;

namespace Game.Presentation
{
    public class EndGameScreenController : ReactiveMonoBehaviour
    {
        public Text matchScore;
        public Text highScore;

        [Inject]
        public void Setup(IScoreController scoreController)
        {
            SubscribePropertyUpdateNow(scoreController.Score, (score) => matchScore.text = $"Total score: {score}");
            SubscribePropertyUpdateNow(scoreController.HighScore, (score) => highScore.text = $"Best: {score}");
        }
    }
}