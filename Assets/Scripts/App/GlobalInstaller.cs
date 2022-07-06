using Auth.Domain;
using Auth.Infra;
using UnityEngine.EventSystems;
using Zenject;
using InfinityZombies.Presentation;

namespace InfinityZombies
{
    public class GlobalInstaller : MonoInstaller
    {
        //TODO testing only; must implement
        public string testUserNickname = "";
        public int testSplashDelay = 2000;

        public override void InstallBindings()
        {
            Container.Bind<EventSystem>().FromComponentInChildren().AsSingle();

            Container.BindInterfacesTo<SceneController>().AsSingle();

            //TODO criar um installer dentro do módulo Auth?
            Container.BindInterfacesTo<AuthControllerImpl>().AsSingle().NonLazy();
            Container.Bind<IGetCurrentUser>().To<GetCurrentUserImpl>().AsSingle();

            Container.Bind<IAuthRepository>().FromMethod(
                //() => new BrokenAuthRepository()
                () => new MockAuthRepository(
                    string.IsNullOrEmpty(testUserNickname) ? null :
                    new UserInfo() { Nickname = testUserNickname, Id = $"{testUserNickname}-{testSplashDelay}" },
                    testSplashDelay
                )
            ).AsSingle().IfNotBound();
        }
    }
}