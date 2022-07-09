using Auth.Domain;
using App.Domain;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace App.Presentation
{
    public class HomePageController : ReactiveMonoBehaviour
    {
        public IAuthController authController;
        public ISceneController sceneController;
        public ISignOut signOut;

        public Text welcomeText;
        public Button logoutButton;
        public Button startNewGameButton;
        public Button joinExistingGameButton;
        public GameObject loadingIndicator; //TODO usar o animation, criar uma classe, trazer por DI

        IStartNewMatch startNewGame;
        IJoinExistingMatch joinExistingGame;

        [Inject]
        public void Setup(IAuthController authController, ISceneController sceneController, IStartNewMatch startNewGame, IJoinExistingMatch joinExistingGame, ISignOut signOut)
        {
            this.signOut = signOut;
            this.authController = authController;
            this.sceneController = sceneController;
            this.startNewGame = startNewGame;
            this.joinExistingGame = joinExistingGame;

            //Atualizar as informações do jogador atual
            SubscribePropertyUpdateNow(authController.CurrentUser, OnUserChanged);

            //Botão New Game
            Subscribe(startNewGameButton.OnClickAsObservable(), OnStartNewGame);
            //Botão Join Game
            Subscribe(joinExistingGameButton.OnClickAsObservable(), OnJoinExistingGame);
            //Botão logout
            Subscribe(logoutButton.OnClickAsObservable(), OnLogout);
        }

        void OnUserChanged(UserInfo user)
        {
            welcomeText.text = $"Logged as:\n{user?.Nickname}";
        }

        private async void OnStartNewGame(Unit obj)
        {
            loadingIndicator.SetActive(true);
            await startNewGame.Invoke();
            //loadingIndicator.SetActive(false);
        }

        private async void OnJoinExistingGame(Unit obj)
        {
            loadingIndicator.SetActive(true);
            await joinExistingGame.Invoke();
            //TODO em caso de falha, desativa
            //loadingIndicator.SetActive(false);
        }

        private async void OnLogout(Unit obj)
        {
            loadingIndicator.SetActive(true);
            await Task.Run(() => signOut.Invoke());
            authController.UserChanged(null);

            //TODO deveria sair dessa página automaticamente?
            await sceneController.ChangePage(Constants.Pages.Login);
        }
    }
}
