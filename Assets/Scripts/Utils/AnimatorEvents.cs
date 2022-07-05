using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class AnimatorEvents : MonoBehaviour
    {
        public UnityEvent OnAnimationFinishedEvent = new UnityEvent();
        public event Action OnAnimationFinished;

        public void AnimationFinished()
        {
            OnAnimationFinishedEvent?.Invoke();
            OnAnimationFinished?.Invoke();
        }
    }
}