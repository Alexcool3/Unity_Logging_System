using UnityEngine;

namespace Logging_System
{
    [CreateAssetMenu(fileName = "Logging Properties", menuName = "Logging/Logging Properties")]
    public class LoggingProperties : ScriptableObject
    {
        [Header("Universal Logging Properties")]
        [Header("Logging Parameters")]
        [Range(1f, 10f), Tooltip("The time before the logging starts")]
        public float waitTime = 1f;
        [Range(5f, 180f), Tooltip("The time at which the logging stops")]
        public float stopTime = 60f;
        [Range(0.1f, 10f), Tooltip("The time between each logging call")]
        public float timeInterval = .1f;
        [HideInInspector] public float startTime; // Time in-game when logging started.
    }
}