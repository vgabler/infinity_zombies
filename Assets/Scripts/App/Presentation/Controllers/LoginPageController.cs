using Auth.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace InfinityZombies.Presentation
{
    public class LoginPageController : MonoBehaviour
    {
        IAuthController authController;
        ISignIn signIn;

        public InputField nicknameText;
        public Button loginButton;
        public GameObject loadingIndicator;

        //TODO ver se precisa disso mesmo
        List<IDisposable> subscriptions = new List<IDisposable>();

        [Inject]
        public void Setup(IAuthController authController, ISignIn signIn)
        {
            this.authController = authController;
            this.signIn = signIn;

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
            SceneManager.LoadScene(Constants.Pages.Home);
            loginButton.interactable = true;
        }

        public async void TestLogin()
        {
            loadingIndicator.SetActive(true);
            var result = await Task.Run(() => signIn.Invoke("testMail@gmail.com", "123qwe"));
            loadingIndicator.SetActive(false);

            authController.UserChanged(result);

            if (result == null)
            {
                return;
            }

            SceneManager.LoadScene(Constants.Pages.Home);
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
