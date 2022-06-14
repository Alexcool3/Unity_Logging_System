using UnityEngine;
using System.Collections;

namespace Logging_System
{
    public class FPS : MonoBehaviour
    {
        [HideInInspector] public float sampleDuration = 1f;

        private int frames; // total number of frames in a cycle.
        private float duration, bestDuration = float.MaxValue, worstDuration;
        // 
        public delegate void GetFPS(float averageDuration, float worstDuration, float bestDuration);
        public delegate void GetFrame(float averageFrame, float worstFrame, float bestFrame);
        public delegate void GetFPSAndFrames(float averageFPS, float worstFPS, float bestFPS, float averageFrame, float worstFrame, float bestFrame);

        // Delegates used to get each sample exactly after 1 second sample duration.
        public GetFPS onGetFPS;
        public GetFrame onGetFrame;
        public GetFPSAndFrames onGetFPSAndFrames;

        // Update is called once per frame
        private void Update()
        {
            frames++;
            duration += Time.unscaledDeltaTime;

            if(Time.unscaledDeltaTime < bestDuration)
            {
                bestDuration = Time.unscaledDeltaTime;
            }

            if (Time.unscaledDeltaTime > worstDuration)
            {
                worstDuration = Time.unscaledDeltaTime;
            }

            if (duration >= sampleDuration)
            {
                // Pass values to subscribed methods
                onGetFPS?.Invoke(frames/duration, 1f/worstDuration, 1f/bestDuration);
                onGetFrame?.Invoke(1000f*duration/frames, 1000f*worstDuration, 1000f*bestDuration);
                onGetFPSAndFrames?.Invoke(frames / duration, 1f / worstDuration, 1f / bestDuration, 1000f * duration / frames, 1000f * worstDuration, 1000f * bestDuration);
                // Reset values for next iteration 
                ResetValues();
            }
        }

        private void OnDisable()
        {
            UnSubscribeDelegates();
            ResetValues();
        }

        private void OnDestroy()
        {
            UnSubscribeDelegates();
            ResetValues();
        }

        private void ResetValues()
        {
            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }

        private void UnSubscribeDelegates()
        {
            onGetFPS = null;
            onGetFrame = null;
            onGetFPSAndFrames = null;
        }
    }
}
