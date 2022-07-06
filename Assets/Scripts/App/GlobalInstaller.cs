using Auth.Domain.Controllers;
using Auth.Domain.Repositories;
using Auth.Infra.Repositories;
using Auth.Domain.UseCases;
using UnityEngine.EventSystems;
using Zenject;
using Auth.Domain.Entities;
using InfinityZombies.Presentation.Controllers;

namespace App
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