using Auth.Domain;
using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace App.Presentation
{
    public class RegisterScreenController : ReactiveMonoBehaviour
    {
        ISceneController sceneController;
        IAuthController authController;
        ISignUp signUp;

        public InputField nicknameField;
        public InputField emailField;
        public InputField passwordField;
        public Button submitBtn;
        public GameObject loadingIndicator;
        public GameObject failureScreen;

        bool FormValid => !string.IsNullOrEmpty(emailField.text) && !string.IsNullOrEmpty(passwordField.text) && !string.IsNullOrEmpty(nicknameField.text);

        [Inject]
        public void Setup(IAuthController authController, ISceneController sceneController, ISignUp signUp)
        {
            this.sceneController = sceneController;
            this.authController = authController;
            this.signUp = signUp;

            SubscribePropertyUpdateNow(nicknameField.OnValueChangedAsObservable().Throttle(TimeSpan.FromMilliseconds(300)).ToReactiveProperty(), OnFieldsChanged);
            SubscribePropertyUpdateNow(emailField.OnValueChangedAsObservable().Throttle(TimeSpan.FromMilliseconds(300)).ToReactiveProperty(), OnFieldsChanged);
            SubscribePropertyUpdateNow(passwordField.OnValueChangedAsObservable().Throttle(TimeSpan.FromMilliseconds(300)).ToReactiveProperty(), OnFieldsChanged);

            Subscribe(submitBtn.OnClickAsObservable(), Submit);
        }

        private void OnFieldsChanged(string obj)
        {
            submitBtn.interactable = FormValid;
        }

        private async void Submit(Unit obj)
        {
            if (!FormValid)
            {
                return;
            }
            //TODO validation

            submitBtn.interactable = false;
            loadingIndicator.SetActive(true);

            var result = await Task.Run(() => signUp.Invoke(emailField.text, passwordField.text, nicknameField.text));

            loadingIndicator.SetActive(false);
            submitBtn.interactable = true;

            authController.UserChanged(result);

            if (result == null)
            {
                failureScreen.SetActive(true);
                return;
            }

            await sceneController.ChangePage(Constants.Pages.Home);
        }
    }
}