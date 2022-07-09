using Auth.Domain;
using InfinityZombies;
using InfinityZombies.Domain;
using InfinityZombies.Presentation;
using Moq;
using NUnit.Framework;
using System.Collections;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class HomePageTests
{
    HomePageController controller;
    Mock<IAuthController> authControllerMock;
    Mock<ISceneController> sceneControllerMock;
    Mock<IStartNewMatch> newGameMock;
    Mock<IJoinExistingMatch> joinGameMock;
    Mock<ISignOut> signOutMock;

    ReactiveProperty<UserInfo> currentUser;
    ReactiveProperty<bool> initialized;

    string changedPage;

    readonly UserInfo testUser = new UserInfo() { Nickname = "Nickname", Id = "Id" };

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        newGameMock = new Mock<IStartNewMatch>();
        joinGameMock = new Mock<IJoinExistingMatch>();
        signOutMock = new Mock<ISignOut>();

        sceneControllerMock = new Mock<ISceneController>();
        sceneControllerMock.Setup((c) => c.ChangePage(It.IsAny<string>()))
        .Callback<string>((name) => changedPage = name);

        authControllerMock = new Mock<IAuthController>();
        currentUser = new ReactiveProperty<UserInfo>(testUser);
        initialized = new ReactiveProperty<bool>(true);

        authControllerMock.Setup((c) => c.CurrentUser).Returns(() => currentUser);
        authControllerMock.Setup((c) => c.Initialized).Returns(() => initialized);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        currentUser.Dispose();
        initialized.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<HomePageController>("Assets/Prefabs/App/Presentation/Pages/HomePage.prefab");
        controller = GameObject.Instantiate(prefab).GetComponent<HomePageController>();
        controller.Setup(authControllerMock.Object, sceneControllerMock.Object, newGameMock.Object, joinGameMock.Object, signOutMock.Object);
    }
    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(controller.gameObject);
    }

    [Test]
    public void Should_display_current_user_nickname()
    {
        Assert.That(controller.welcomeText.text, Does.Contain(testUser.Nickname));
    }

    [Test]
    public void Should_display_loading_indicator()
    {
        throw new System.NotImplementedException();
    }

    [Test]
    public void Should_logout()
    {
        controller.logoutButton.onClick.Invoke();
        signOutMock.Verify((m) => m.Invoke());
        sceneControllerMock.Verify((m) => m.ChangePage(Constants.Pages.Login));
    }

    [Test]
    public void Should_start_new_game()
    {
        controller.startNewGameButton.onClick.Invoke();
        newGameMock.Verify((m) => m.Invoke());
    }

    [Test]
    public void Should_join_existing_game()
    {
        controller.joinExistingGameButton.onClick.Invoke();
        joinGameMock.Verify((m) => m.Invoke());
    }
}
