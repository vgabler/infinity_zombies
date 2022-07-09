using App.Domain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace App.Presentation
{
    public class MatchLeaver : ReactiveMonoBehaviour
    {
        public ISceneController sceneController;

        public Button leaveButton;
        public GameObject loadingIndicator; //TODO usar o animation, criar uma classe, trazer por DI

        IExitCurrentMatch exitCurrentMatch;

        [Inject]
        public void Setup(ISceneController sceneController, IExitCurrentMatch exitCurrentMatch)
        {
            this.sceneController = sceneController;
            this.exitCurrentMatch = exitCurrentMatch;

            //Botão Sair
            Subscribe(leaveButton.OnClickAsObservable(), OnExitCurrentMatch);
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