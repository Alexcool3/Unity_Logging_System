using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Threading.Tasks;

namespace Logging_System
{
    public class LoggingController : MonoBehaviour
    {
        [Header("Logging Properties from Data Container")]
        [SerializeField, Tooltip("Data Fields Object")] private LoggingProperties loggingProperties;
        // This class is used to log data individually.
        private IEnumerator repeat;
        private bool async;

        #region Unity Events
        [Header("Logging Events")]
        [Tooltip("Event invoked before logging starts")] public UnityEvent OnLoggingStart;
        [Tooltip("Event invoked one time per repiteve call")] public UnityEvent OnLogging;
        [Tooltip("Event invoked when logging stops")] public UnityEvent OnLoggingStop;
        #endregion

        [ContextMenu("Start Logging")]
        public void StartLogging()
        {
            if (OnLogging == null)
            {
                return;
            }

            StartCoroutine(repeat = Repeat(loggingProperties.waitTime, loggingProperties.timeInterval));
        }

        private IEnumerator Repeat(float waitTime, float timeInterval)
        {
#if UNITY_EDITOR

            Debug.Log($"Logging Starts in:{waitTime}");
#endif
            yield return new WaitForSecondsRealtime(waitTime);

#if UNITY_EDITOR
            Debug.Log($"Logging Started");
#endif
            OnLoggingStart?.Invoke();

            loggingProperties.iterations = (int)Mathf.Round(loggingProperties.stopTime / loggingProperties.timeInterval);

            loggingProperties.currentIteration = 0;
            loggingProperties.currentTime = 0.0f;

            while (true)
            {
                OnLogging?.Invoke();

                if (loggingProperties.currentIteration >= loggingProperties.iterations)
                {
                    StopCoroutine(repeat);
                    OnLoggingStop?.Invoke();
#if UNITY_EDITOR
                    Debug.Log($"Logging Stopped");
#endif
                }

                loggingProperties.currentIteration++;
                loggingProperties.currentTime += loggingProperties.timeInterval;
                yield return new WaitForSecondsRealtime(timeInterval);
            }
        }

        public async Task StartLoggingAsync(bool async = false)
        {
            if (async && OnLogging != null) StartCoroutine(repeat = Repeat(loggingProperties.waitTime, loggingProperties.timeInterval));
            else await Repeat(10);
        }

        private Task Repeat(int iterations)
        {
#if UNITY_EDITOR
            Debug.Log($"Logging Starts in:{loggingProperties.waitTime}");
#endif
            Task.Delay((int)loggingProperties.waitTime * 1000);
#if UNITY_EDITOR
            Debug.Log($"Logging Started");
#endif
            OnLoggingStart?.Invoke();

            loggingProperties.currentIteration = 0;
            loggingProperties.currentTime = 0.0f;

            while (loggingProperties.currentIteration < iterations)
            {
                OnLogging?.Invoke();

                loggingProperties.currentTime += loggingProperties.timeInterval;
                loggingProperties.currentIteration++;
                Task.Delay((int)loggingProperties.timeInterval * 1000);
            }

            OnLoggingStop?.Invoke();
#if UNITY_EDITOR
            Debug.Log($"Logging Stopped");
#endif

#if UNITY_EDITOR
            return new Task(null);
#endif
        }

        private void OnDestroy()
        {
            OnLoggingStart?.RemoveAllListeners();
            OnLogging?.RemoveAllListeners();
            OnLoggingStop?.RemoveAllListeners();
        }
    }
}