using Auth.Domain;
using Auth.Infra;
using UnityEngine.EventSystems;
using Zenject;
using App.Presentation;
using App.Domain;
using App.External;
using Auth.External;

namespace App
{
    public class GlobalInstaller : MonoInstaller
    {
        //TODO testing only; must implement
        public string testUserNickname = "";
        public int testSplashDelay = 2000;

        public override void InstallBindings()
        {
            //UI
            Container.Bind<EventSystem>().FromComponentInChildren().AsSingle();

            //Management
            Container.BindInterfacesTo<SceneController>().AsSingle();

            //Game
            Container.Bind<IMatchService>().To<PhotonMatchService>().AsSingle().Lazy();

            //TODO criar um installer dentro do módulo Auth?
            //Auth
            //--- Domain --- Controllers
            Container.BindInterfacesTo<AuthControllerImpl>().AsSingle();

            //--- Domain --- Usecases
            Container.Bind<IGetCurrentUser>().To<GetCurrentUserImpl>().AsTransient();
            Container.Bind<ISignInWithFacebook>().To<SignInWithFacebookImpl>().AsTransient();
            Container.Bind<ISignIn>().To<SignInImpl>().AsTransient();
            Container.Bind<ISignUp>().To<SignUpImpl>().AsTransient();
            Container.Bind<ISignOut>().To<SignOutImpl>().AsTransient();

            //--- Infra --- Repositories
            Container.Bind<IAuthRepository>().To<PlayfabAuthRepository>().AsSingle();

            //--- External --- Datasources
            Container.Bind<ILocalStorage>().To<PlayerPrefsLocalStorage>().AsSingle().Lazy();

            //--- External --- Datasources
            Container.Bind<IFacebookService>().To<FacebookServiceImpl>().AsSingle();
        }
    }
}