using Auth.Domain.Controllers;
using Auth.Domain.Repositories;
using Auth.Infra.Repositories;
using Auth.Domain.UseCases;
using UnityEngine.EventSystems;
using Zenject;
using Auth.Domain.Entities;

namespace App
{
    public class AppInstaller : MonoInstaller
    {
        //TODO testing only; must implement
        public string testUserNickname = "";
        public int testSplashDelay = 2000;

        public override void InstallBindings()
        {
            Container.Bind<EventSystem>().FromComponentInChildren().AsSingle();

            //TODO criar um installer dentro do módulo Auth?
            Container.Bind<IAuthController>().To<AuthControllerImpl>().AsSingle().NonLazy();
            Container.Bind<IGetCurrentUser>().To<GetCurrentUserImpl>().AsSingle();

            Container.Bind<IAuthRepository>().FromInstance(
                new MockAuthRepository(
                    string.IsNullOrEmpty(testUserNickname) ? null :
                    new UserInfo() { Nickname = testUserNickname, Id = $"{testUserNickname}-{testSplashDelay}" },
                    testSplashDelay
                )
            ).AsSingle();
        }
    }
}