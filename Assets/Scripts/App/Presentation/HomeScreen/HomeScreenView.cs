using Auth.Domain.Controllers;
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace InfinityZombies.Presentation.HomeScreen
{
    public class HomeScreenView : MonoBehaviour
    {
        public IAuthController authController;

        public Text welcomeText;
        public Button logoutButton;

        //TODO ver se precisa disso mesmo
        List<IDisposable> subscriptions = new List<IDisposable>();

        [Inject]
        public void Setup(IAuthController authController)
        {
            this.authController = authController;

            subscriptions.Add(authController.CurrentUser
            .Subscribe(OnUserChanged));
            OnUserChanged(authController.CurrentUser.Value);

            subscriptions.Add(logoutButton.OnClickAsObservable().Subscribe(OnLogoutClicked));
        }

        private void OnLogoutClicked(Unit obj)
        {
            //TODO logout usecase
            authController.UserChanged(null);

            //TODO deveria fazer isso automaticamente?
            SceneManager.LoadScene("Login");
        }

        void OnUserChanged(UserInfo user)
        {
            welcomeText.text = $"Bem vindo, {user?.Nickname}!";
        }

        private void OnDestroy()
        {
            foreach (var s in subscriptions)
            {
                s.Dispose();
            }
        }
    }
}
