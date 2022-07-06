using Auth.Domain;
using Auth.Infra;
using UnityEngine.EventSystems;
using Zenject;
using InfinityZombies.Presentation;
using Fusion;
using InfinityZombies.Domain;
using InfinityZombies.External;
using UnityEngine;

namespace InfinityZombies
{
    public class GlobalInstaller : MonoInstaller
    {
        //TODO testing only; must implement
        public string testUserNickname = "";
        public int testSplashDelay = 2000;

        public GameObject networkRunnerPrefab;

        public override void InstallBindings()
        {
            //UI
            Container.Bind<EventSystem>().FromComponentInChildren().AsSingle();

            //Management
            Container.BindInterfacesTo<SceneController>().AsSingle();

            //Game
            Container.Bind<IMatchService>().To<PhotonMatchService>().AsSingle().Lazy();
            Container.Bind<NetworkRunner>().FromComponentInNewPrefab(networkRunnerPrefab).AsSingle().Lazy();


            //Auth
            //TODO criar um installer dentro do módulo Auth?
            Container.BindInterfacesTo<AuthControllerImpl>().AsSingle();
            Container.Bind<IGetCurrentUser>().To<GetCurrentUserImpl>().AsSingle();

            Container.Bind<IAuthRepository>().FromMethod(
                () => new MockAuthRepository(
                    string.IsNullOrEmpty(testUserNickname) ? null :
                    new UserInfo() { Nickname = testUserNickname, Id = $"{testUserNickname}-{testSplashDelay}" },
                    testSplashDelay
                )
            ).AsSingle();

        }
    }
}