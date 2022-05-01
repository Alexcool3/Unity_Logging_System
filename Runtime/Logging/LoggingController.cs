using UnityEngine;
using UnityEngine.Events;

namespace Logging_System
{
    public class LoggingController : MonoBehaviour
    {
        [Header("Logging Properties from Data Container")]
        [SerializeField, Tooltip("Data Fields Object")] private LoggingProperties loggingProperties;
        // This class is used to log data individually.

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

            OnLoggingStart?.Invoke();
            loggingProperties.startTime = Time.time + loggingProperties.waitTime;

            Debug.Log($"Logging Started");
            InvokeRepeating(nameof(LogData), loggingProperties.waitTime, loggingProperties.timeInterval);
        }

        private void LogData()
        {
            OnLogging?.Invoke();
            if (Time.time - loggingProperties.startTime >= loggingProperties.stopTime)
            {
                CancelInvoke();
                OnLoggingStop?.Invoke();
                Debug.Log($"Logging Stopped");
            }
        }

        private void OnDestroy()
        {
            OnLoggingStart?.RemoveAllListeners();
            OnLogging?.RemoveAllListeners();
            OnLoggingStop?.RemoveAllListeners();
        }
    }
}