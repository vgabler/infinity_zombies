using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Utils
{
    public class ReactiveMonoBehaviour : MonoBehaviour
    {
        List<IDisposable> subscriptions = new List<IDisposable>();

        protected virtual void SubscribePropertyUpdateNow<T>(IReadOnlyReactiveProperty<T> observable, Action<T> onValueChanged)
        {
            Subscribe(observable, onValueChanged);
            onValueChanged(observable.Value);
        }

        protected virtual void Subscribe<T>(IObservable<T> observable, Action<T> onValueChanged)
        {
            subscriptions.Add(observable.Subscribe(onValueChanged));
        }

        protected virtual void Dispose()
        {
            foreach (var sub in subscriptions)
            {
                sub.Dispose();
            }

            subscriptions.Clear();
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }
    }
}