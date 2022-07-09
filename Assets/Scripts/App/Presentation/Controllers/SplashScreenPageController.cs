using System;
using UnityEngine;
using Utils;
using UniRx;
using Zenject;
using Auth.Domain;

namespace App.Presentation
{
    public class SplashScreenPageController : MonoBehaviour
    {
        public Animator splashAnimation;

        IAuthController authController;
        ISceneController sceneController;

        IDisposable subscription;

        [Inject]
        public void Setup(IAuthController authController, ISceneController sceneLoader)
        {
            this.authController = authController;
            this.sceneController = sceneLoader;

            subscription = authController.Initialized.Subscribe(OnAuthInitialized);

            splashAnimation.GetComponent<AnimatorEvents>().OnAnimationFinished += OnSplashAnimationFinished;
        }

        private void OnDestroy()
        {
            subscription?.Dispose();
        }

        void OnAuthInitialized(bool val)
        {
            if (val)
            {
                splashAnimation.SetTrigger("Initialized");
            }
        }

        private void OnSplashAnimationFinished()
        {
            if (authController.CurrentUser.Value == null)
            {
                sceneController.ChangePage(Constants.Pages.Login);
            }
            else
            {
                sceneController.ChangePage(Constants.Pages.Home);
            }
        }
    }
}