using Auth.Domain.Controllers;
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace InfinityZombies.Presentation.LoginScreen
{
    public class LoginPageController : MonoBehaviour
    {
        public IAuthController authController;

        public InputField nicknameText;
        public Button loginButton;

        //TODO ver se precisa disso mesmo
        List<IDisposable> subscriptions = new List<IDisposable>();

        [Inject]
        public void Setup(IAuthController authController)
        {
            this.authController = authController;

            subscriptions.Add(nicknameText.OnValueChangedAsObservable().Subscribe(OnNicknameChanged));
            OnNicknameChanged(null);
            subscriptions.Add(loginButton.OnClickAsObservable().Subscribe(OnLoginClicked));
        }

        private void OnLoginClicked(Unit obj)
        {
            loginButton.interactable = false;
            if (string.IsNullOrEmpty(nicknameText.text))
            {
                return;
            }
            //TODO validation

            //TODO login usecase
            authController.UserChanged(new UserInfo() { Nickname = nicknameText.text, Id = $"{nicknameText.text}-{Time.deltaTime * 1000}" });
            SceneManager.LoadScene("Home");
            loginButton.interactable = true;
        }

        private void OnNicknameChanged(string obj)
        {
            loginButton.interactable = !string.IsNullOrEmpty(obj);
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
