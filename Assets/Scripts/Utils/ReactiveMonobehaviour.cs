using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Utils
{
    public class ReactiveMonobehaviour : MonoBehaviour
    {
        List<IDisposable> subscriptions = new List<IDisposable>();

        protected virtual void SubscribeProperty<T>(IReadOnlyReactiveProperty<T> observable, Action<T> onValueChanged, bool triggerCurrent = true)
        {
            subscriptions.Add(observable.Subscribe(onValueChanged));

            if (triggerCurrent)
            {
                onValueChanged(observable.Value);
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var sub in subscriptions)
            {
                sub.Dispose();
            }
        }
    }
}