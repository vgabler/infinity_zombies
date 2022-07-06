using Auth.Domain;
using InfinityZombies.Domain;
using UniRx;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace InfinityZombies.Presentation
{
    public class HomePageController : ReactiveMonobehaviour
    {
        public IAuthController authController;
        public ISceneController sceneController;

        public Text welcomeText;
        public Button logoutButton;
        public Button startNewGameButton;
        public Button joinExistingGameButton;

        IStartNewGame startNewGame;
        IJoinExistingGame joinExistingGame;

        [Inject]
        public void Setup(IAuthController authController, ISceneController sceneController, IStartNewGame startNewGame, IJoinExistingGame joinExistingGame)
        {
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
            welcomeText.text = $"Bem vindo, {user?.Nickname}!";
        }

        private void OnStartNewGame(Unit obj)
        {
            joinExistingGame.Invoke();
        }

        private void OnJoinExistingGame(Unit obj)
        {
            startNewGame.Invoke();
        }

        private void OnLogout(Unit obj)
        {
            //TODO logout usecase
            authController.UserChanged(null);

            //TODO deveria sair dessa página automaticamente?
            sceneController.ChangePage(Constants.Pages.Login);
        }
    }
}
