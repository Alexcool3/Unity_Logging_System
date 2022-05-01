using UnityEngine;
using System;

namespace Logging_System
{
    public class DataManager : MonoBehaviour
    {
        #region Singleton Properties
        private static DataManager instance;

        public static DataManager Singleton
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

        [Header("Data Properties")]
        [SerializeField, Tooltip("Object's transform to track.")] private Transform trackObject;
        [SerializeField, Tooltip("Web Server Monobehaviour")] private WebServer webServer;
        public UserData userData;
        public LoggingProperties loggingProperties;
        private int timeStamps;
        private short index = 0;
        private string directory;

        private void Awake()
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            userData.Init();
            timeStamps = (int)Math.Round(loggingProperties.stopTime / loggingProperties.timeInterval) + 2;
            Debug.Log(timeStamps);
            userData.SetDataSizes(timeStamps);
            userData.AddRow(userData?.header?.ToArray());
            directory ??= System.IO.Directory.GetCurrentDirectory();
            Debug.Log(directory);
        }

        public void Save()
        {
            if (userData.saveLocally)
            {
                if (userData.saveJson)
                {
                    try
                    {
                        userData.AddSerializeable(userData);
                        userData.SerializeAll();
                        JsonFileManager.SaveJson(@$"{directory}/JSON/Participant_{userData.ID}.json", userData.jSavegame.ToString());
                    }
                    catch (Exception e)
                    {

                        Debug.LogError($"The following error occured: {e.Message}"); ;
                    }
                }

                if (userData.saveCSV)
                {
                    try
                    {
                        CSVFileManager.SaveFile($"{directory}/CSV/Participant_{userData.ID}.csv", userData.csv, overwrite: false);
                    }
                    catch (Exception e)
                    {

                        Debug.LogError($"The following error occured: {e.Message}"); ;
                    }
                }
            }

            if (userData.sendToServer)
            {
                webServer?.SendToServer("localhost:300", new System.Collections.Generic.Dictionary<string, string>() { { "information", userData.jSavegame.ToString() } });

            }

        }

        public void LogData()
        {
            Vector3 position = trackObject.position; // Position of the tracked object in world space
            Vector3 forward = trackObject.forward; // Normalized forward facing vector of the tracked object in world space.
            float time = Time.time;
            float deltaTime = Time.unscaledDeltaTime;
            float fps = 1f / deltaTime;
            userData?.AddRow(Float2String(time - loggingProperties.startTime, position.x, position.y, position.z, forward.x, forward.y, forward.z, fps, deltaTime));

            userData.saveData.time[index] = Math.Round(time, 2).ToString().Replace(',', '.');
            userData.saveData.positions[index] = $"{Math.Round(position.x, 2).ToString().Replace(",", ".")};{Math.Round(position.z, 2).ToString().Replace(",", ".")}";
            userData.saveData.views[index] = $"{Math.Round(forward.x, 2).ToString().Replace(",", ".")};{Math.Round(forward.y, 2).ToString().Replace(",", ".")};{Math.Round(forward.z, 2).ToString().Replace(",", ".")}";
            //userData.saveData.fps[index] = Math.Round(fps, 2).ToString().Replace(',', '.');
            /*
            participantData.saveData.time += $"{Math.Round(time, 2).ToString().Replace(',', '.')}\n";
            participantData.saveData.positions += $"{Math.Round(position.x, 2).ToString().Replace(",",".")};{Math.Round(position.z, 2).ToString().Replace(",", ".")}\n";
            participantData.saveData.views += $"{Math.Round(forward.x, 2).ToString().Replace(",", ".")};{Math.Round(forward.y, 2).ToString().Replace(",", ".")};{Math.Round(forward.z, 2).ToString().Replace(",", ".")}\n";
            participantData.saveData.fps += $"{Math.Round(fps, 2).ToString().Replace(',', '.')}\n";
            */
            Debug.Log($"index:{index++}");
        }

        private string[] Float2String(params float[] values)
        {
            string[] tmp = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                tmp[i] = Math.Round(values[i], 2).ToString().Replace(',', '.');

            }
            return tmp;
        }

        [ContextMenu("Load Data")]
        public void LoadData()
        {
            userData.LoadData();
        }
    }
}