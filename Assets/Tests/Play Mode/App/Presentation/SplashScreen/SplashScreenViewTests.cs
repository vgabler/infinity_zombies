using Auth.Domain.Controllers;
using Auth.Domain.Entities;
using InfinityZombies.Presentation.SplashScreen;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SplashScreenViewTests
{
    SplashScreenView view;
    Mock<IAuthController> authControllerMock;

    ReactiveProperty<UserInfo> currentUser;
    ReactiveProperty<bool> initialized;

    string loadedScene;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedScene = scene.name;
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        currentUser = new ReactiveProperty<UserInfo>();
        initialized = new ReactiveProperty<bool>();

        authControllerMock = new Mock<IAuthController>();
        authControllerMock.Setup((c) => c.CurrentUser).Returns(() => currentUser);
        authControllerMock.Setup((c) => c.Initialized).Returns(() => initialized);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        currentUser.Dispose();
        initialized.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        loadedScene = null;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/App/Presentation/SplashScreen/SplashScreenView.prefab");
        view = GameObject.Instantiate(prefab).GetComponent<SplashScreenView>();
        view.Setup(authControllerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        initialized.Value = false;
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator Should_go_to_login()
    {
        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loading"), Is.EqualTo(true));

        currentUser.Value = null;
        initialized.Value = true;

        yield return new WaitForSeconds(1);

        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loaded"), Is.EqualTo(true));

        yield return new WaitUntil(() => loadedScene == "Login");
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator Should_go_to_home()
    {
        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loading"), Is.EqualTo(true));

        currentUser.Value = new UserInfo();
        initialized.Value = true;

        yield return new WaitForSeconds(1);

        Assert.That(view.splashAnimation.GetCurrentAnimatorStateInfo(0).IsName("Loaded"), Is.EqualTo(true));

        yield return new WaitUntil(() => loadedScene == "Home");
    }
}
