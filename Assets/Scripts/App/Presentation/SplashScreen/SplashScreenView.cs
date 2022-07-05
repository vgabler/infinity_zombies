using Auth.Controllers;
using Auth.Domain.UseCases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using UniRx;
using UnityEngine.SceneManagement;

namespace InfinityZombies.SplashScreen
{
    public class SplashScreenView : MonoBehaviour
    {
        public IAuthController authController;
        public Animator splashAnimation;

        IDisposable subscription;

        //TODO inject 
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
            if (authController.CurrentUser.Value != null)
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