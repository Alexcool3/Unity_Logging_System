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
                StartCoroutine(WriteToServer(url, data, tables));
            }
        }

        public string GetLastResponse()
        {
            return lastResponse;
        }

        private IEnumerator WriteToServer(string url, Dictionary<string, string> data, Dictionary<string, string> tables = null)
        {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> item in data)
            {
                form.AddField(item.Key, item.Value);
            }

            if (tables != null)
            {
                foreach (KeyValuePair<string, string> item in tables)
                {
                    form.AddField(item.Key, item.Value);
                }
            }

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"Error: Data not send to server");
            }
            else
            {
                Debug.LogError($"Server response: {request.downloadHandler.text}");
            }
        }

        private IEnumerator ReadFromServer(string url, Dictionary<string, string> tables)
        {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> item in tables)
            {
                form.AddField(item.Key, item.Value);
            }

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {

                Debug.LogError("Error: Data not send to server");
            }
            else
            {
                lastResponse = request.downloadHandler.text;
                Debug.LogError($"Server response: {lastResponse}");
            }
        }
    }
}