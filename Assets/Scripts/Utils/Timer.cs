using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class Timer : MonoBehaviour
    {
        float timer;
        float currentDuration;

        [field: SerializeField]
        public float DefaultDuration { get; set; } = 1;
        [field: SerializeField]
        public bool AutoPlay { get; set; } = false;
        [field: SerializeField]
        public bool UseUnscaledTime { get; set; } = false;

        public UnityEvent OnBegin = new UnityEvent();
        public UnityEvent OnEnd = new UnityEvent();

        public bool IsRunning { get; set; }
        public float RemainingTime => timer;

        private void Start()
        {
            if (AutoPlay)
            {
                Activate();
            }
        }

        public void Restart()
        {
            Restart(currentDuration);
        }

        public void Restart(float duration)
        {
            Stop();
            Activate(duration);
        }

        public void Activate(float duration)
        {
            if (IsRunning)
            {
                return;
            }

            currentDuration = duration;
            timer = duration;
            IsRunning = true;
            OnBegin?.Invoke();
        }

        public void Activate()
        {
            Activate(DefaultDuration);
        }

        public void Stop()
        {
            IsRunning = false;
            timer = 0;
        }

        public void End()
        {
            Stop();
            OnEnd?.Invoke();
        }

        private void Update()
        {
            if (!IsRunning)
            {
                return;
            }

            timer -= UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (timer <= 0)
            {
                End();
            }
        }
    }
}