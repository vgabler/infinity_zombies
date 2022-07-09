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
    public class LoginScreenController : ReactiveMonoBehaviour
    {
        ISceneController sceneController;
        IAuthController authController;
        ISignIn signIn;
        ISignInWithFacebook signInWithFacebook;

        public InputField emailField;
        public InputField passwordField;
        public Button submitBtn;
        public Button loginWithFacebookBtn;
        public GameObject loadingIndicator;

        bool FormValid => !string.IsNullOrEmpty(emailField.text) && !string.IsNullOrEmpty(passwordField.text);

        [Inject]
        public void Setup(IAuthController authController, ISceneController sceneController, ISignIn signIn, ISignInWithFacebook signInWithFacebook)
        {
            this.sceneController = sceneController;
            this.authController = authController;
            this.signIn = signIn;
            this.signInWithFacebook = signInWithFacebook;

            SubscribePropertyUpdateNow(emailField.OnValueChangedAsObservable().Throttle(TimeSpan.FromMilliseconds(300)).ToReactiveProperty(), OnFieldsChanged);
            SubscribePropertyUpdateNow(passwordField.OnValueChangedAsObservable().Throttle(TimeSpan.FromMilliseconds(300)).ToReactiveProperty(), OnFieldsChanged);

            Subscribe(submitBtn.OnClickAsObservable(), Submit);
            Subscribe(loginWithFacebookBtn.OnClickAsObservable(), LoginWithFacebook);
        }

        private async void LoginWithFacebook(Unit obj)
        {
            loadingIndicator.SetActive(true);

            var result = await Task.Run(() => signInWithFacebook.Invoke());

            loadingIndicator.SetActive(false);
            submitBtn.interactable = true;

            authController.UserChanged(result);

            if (result == null)
            {
                return;
            }

            await sceneController.ChangePage(Constants.Pages.Home);
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
