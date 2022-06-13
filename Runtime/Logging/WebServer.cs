using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Logging_System
{
    public class WebServer : MonoBehaviour
    {
        #region Singleton Property
        private static WebServer instance;

        public static WebServer Singleton
        {
            get => instance;
            set
            {
                if (instance == null)
                {
                    instance = value;
                }
                else
                {
                    Destroy(value.gameObject);
                }
            }
        }
        #endregion

        private void Awake()
        {
            Singleton = this;
        }

        // enum value must allign with urls array.
        public enum URL { SEND = 0, RECEIVE = 1, SENDDATA = 2 };

        private static string[] urls =
        {
        // Url [0],
        // Url [1],
        // ...
        };

        private string lastResponse;

        public void SendToServer(string url, Dictionary<string, string> data, Dictionary<string, string> tables = null)
        {
            if (data != null)
            {
                StartCoroutine(WriteToServer(url, data));
            }
        }

        public string GetLastResponse()
        {
            return lastResponse;
        }

        private IEnumerator WriteToServer(string url, Dictionary<string, string> fields, Dictionary<string, byte[]> binaryFields = null)
        {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> item in fields)
            {
                form.AddField(item.Key, item.Value);
            }

            if (binaryFields != null)
            {
                foreach (KeyValuePair<string, byte[]> field in binaryFields)
                {
                    form.AddBinaryData(field.Key, field.Value);
                }
            }

            using (var request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
#if UNITY_EDITOR
                    Debug.LogError(request.error);
#endif
                    // Prompt message to the user
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("Success!");
#endif              
                    // Propmpt message to the user.
                }
            }
        }

        private IEnumerator ReadFromServer(string url, Dictionary<string, string> tables)
        {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> item in tables)
            {
                form.AddField(item.Key, item.Value);
            }

            using (var request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {

#if UNITY_EDITOR
                    Debug.LogError($"Error:{request.error}");
#endif  
                    // Prompt message to the user
                }
                else
                {
                    lastResponse = request.downloadHandler.text;
#if UNITY_EDITOR
                    Debug.Log($"Response:{lastResponse}");
#endif              
                    // Propmpt message to the user.
                }
            }
        }
    }
}