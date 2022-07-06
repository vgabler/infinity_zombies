using Auth.Domain;
using InfinityZombies;
using InfinityZombies.Presentation;
using Moq;
using NUnit.Framework;
using System.Collections;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class SplashScreenPageTests
{
    SplashScreenPageController view;
    Mock<IAuthController> authControllerMock;
    Mock<ISceneController> sceneControllerMock;

    ReactiveProperty<UserInfo> currentUser;
    ReactiveProperty<bool> initialized;

    string changedPage;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        sceneControllerMock = new Mock<ISceneController>();
        sceneControllerMock.Setup((c) => c.ChangePage(It.IsAny<string>()))
        .Callback<string>((name) => changedPage = name);

        authControllerMock = new Mock<IAuthController>();
        currentUser = new ReactiveProperty<UserInfo>();
        initialized = new ReactiveProperty<bool>();
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
        initialized.Value = false;
        changedPage = null;

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/App/Presentation/Pages/SplashScreenPage.prefab");
        view = GameObject.Instantiate(prefab).GetComponent<SplashScreenPageController>();
        view.Setup(authControllerMock.Object, sceneControllerMock.Object);
    }
    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(view.gameObject);
    }


    [UnityTest, Timeout(3000)]
    public IEnumerator Should_initialize_and_go_to_login()
    {
        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loading"), Is.EqualTo(true));

        currentUser.Value = null;
        initialized.Value = true;

        yield return new WaitForSeconds(1);

        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loaded"), Is.EqualTo(true));

        yield return new WaitUntil(() => changedPage == Constants.Pages.Login);
    }

    [UnityTest, Timeout(3000)]
    public IEnumerator Should_initialize_and_go_to_home()
    {
        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loading"), Is.EqualTo(true));

        currentUser.Value = new UserInfo();
        initialized.Value = true;

        yield return new WaitForSeconds(1);

        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loaded"), Is.EqualTo(true));

        yield return new WaitUntil(() => changedPage == Constants.Pages.Home);
    }
}
