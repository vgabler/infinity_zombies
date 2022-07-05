using NUnit.Framework;
using UniRx;
using UnityEngine;
using Utils;

public class AnimatorEventsTest
{
    AnimatorEvents component;
    ReactiveProperty<bool> prop;

    [SetUp]
    public void Setup()
    {
        component = new GameObject().AddComponent<AnimatorEvents>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(component.gameObject);
    }

    [Test]
    public void Should_raise_events_correctly()
    {
        var actionCalled = false;
        var unityEventCalled = false;
        component.OnAnimationFinished += () => actionCalled = true;
        component.OnAnimationFinishedEvent.AddListener(() => unityEventCalled = true);

        component.AnimationFinished();

        Assert.That(actionCalled, Is.EqualTo(true));
        Assert.That(unityEventCalled, Is.EqualTo(true));
    }
}
