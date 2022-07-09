using Auth.Domain;
using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace InfinityZombies.Presentation
{
    public class LoginScreenController : ReactiveMonoBehaviour
    {
        ISceneController sceneController;
        IAuthController authController;
        ISignIn signIn;

        public InputField emailField;
        public InputField passwordField;
        public Button submitBtn;
        public GameObject loadingIndicator;

        bool FormValid => !string.IsNullOrEmpty(emailField.text) && !string.IsNullOrEmpty(passwordField.text);

        [Inject]
        public void Setup(IAuthController authController, ISceneController sceneController, ISignIn signIn)
        {
            this.sceneController = sceneController;
            this.authController = authController;
            this.signIn = signIn;

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

            var result = await Task.Run(() => signIn.Invoke(emailField.text, passwordField.text));

            loadingIndicator.SetActive(false);
            submitBtn.interactable = true;

            authController.UserChanged(result);

            if (result == null)
            {
                return;
            }

            await sceneController.ChangePage(Constants.Pages.Home);
        }
    }
}
