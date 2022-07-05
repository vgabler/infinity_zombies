using System;
using UnityEngine;
using Utils;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;
using Auth.Domain.Controllers;

namespace InfinityZombies.SplashScreen
{
    public class SplashScreenView : MonoBehaviour
    {
        public IAuthController authController;
        public Animator splashAnimation;

        IDisposable subscription;

        [Inject]
        public void Setup(IAuthController authController)
        {
            this.authController = authController;

            subscription = authController.Initialized
            .Subscribe(OnAuthInitialized);

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
                SceneManager.LoadScene("Login");
            }
            else
            {
                SceneManager.LoadScene("Home");
            }
        }
    }
}