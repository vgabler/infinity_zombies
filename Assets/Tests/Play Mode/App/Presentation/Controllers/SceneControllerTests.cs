using App.Presentation;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneControllerTests
{
    SceneController controller;

    Scene currentScene;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene;
    }

    [SetUp]
    public void SetUp()
    {
        controller = new SceneController();
    }

    [UnityTest, Timeout(3000)]
    public IEnumerator Should_change_page_successfully()
    {
        controller.ChangePage("Empty");

        yield return new WaitUntil(() => currentScene.name == "Empty");
    }
}
