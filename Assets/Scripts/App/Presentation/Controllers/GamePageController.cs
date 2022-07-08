using InfinityZombies.Domain;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace InfinityZombies.Presentation
{
    public class GamePageController : ReactiveMonoBehaviour
    {
        public List<Button> leaveButtons;
        public Button retryButton;
        public GameObject loadingIndicator; //TODO usar o animation, criar uma classe, trazer por DI

        ISceneController sceneController;
        IExitCurrentMatch exitCurrentMatch;
        IRetryCurrentMatch retryCurrentMatch;

        [Inject]
        public void Setup(ISceneController sceneController, IExitCurrentMatch exitCurrentMatch, IRetryCurrentMatch retryCurrentMatch)
        {
            this.sceneController = sceneController;
            this.retryCurrentMatch = retryCurrentMatch;
            this.exitCurrentMatch = exitCurrentMatch;

            //Botões "sair"
            foreach (var btn in leaveButtons)
            {
                Subscribe(btn.OnClickAsObservable(), OnExitCurrentMatch);
            }

            Subscribe(retryButton.OnClickAsObservable(), OnRetryCurrentMatch);
        }

        private async void OnRetryCurrentMatch(Unit obj)
        {
            loadingIndicator.SetActive(true);
            await sceneController.ChangePage(Constants.Pages.Empty);
            await retryCurrentMatch.Invoke();
            //loadingIndicator.SetActive(false);
        }

        private async void OnExitCurrentMatch(Unit obj)
        {
            loadingIndicator.SetActive(true);
            await exitCurrentMatch.Invoke();
            await sceneController.ChangePage(Constants.Pages.Home);
            //loadingIndicator.SetActive(false);
        }
    }
}