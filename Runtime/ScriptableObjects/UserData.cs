using Logging_System.FileManagement;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logging_System
{
    [CreateAssetMenu(fileName = "UserData", menuName = "Data Object/UserData")]
    public class UserData : ScriptableObject, ISerializeable
    {
        [Header("Participant Data Properties")]
        [Tooltip("Participant ID. This will be used as the name of the saved files.")]
        public int ID = -1;
        [Tooltip("Header row for csv file")] public List<string> header;
        [Tooltip("Save csv file locally")] public bool saveCSV;
        [Tooltip("Save json file locally")] public bool saveJson;
        [Tooltip("Save Files Locally")] public bool saveLocally;
        [Tooltip("Send data to Server")] public bool sendToServer;
        // [Tooltip("Save on Web Server")] public bool saveServer;

        #region Hidden attributes
        [HideInInspector] public CSV csv;
        [HideInInspector] public SaveData saveData;
        [HideInInspector] public string directory;
        [HideInInspector] public JObject jSavegame;
        [HideInInspector] public int numberOfSamples; // Number of points the shader will iterate through.
        [HideInInspector] public float[] dataPositions;
        [HideInInspector] public float[] dataForwards;
        [HideInInspector] public int shaderIterations;
        List<ISerializeable> serializeables;
        #endregion
        private void OnEnable()
        {
            directory ??= System.IO.Directory.GetCurrentDirectory();
            Init();
            shaderIterations = 101;
        }

        public void Init()
        {
            csv = new CSV();
            saveData = new SaveData();
            jSavegame = new JObject();
            serializeables = new List<ISerializeable>();
        }

        public void SetDataSizes(int i)
        {
            saveData.time = new string[i];
            saveData.positions = new string[i];
            saveData.views = new string[i];
            saveData.fps = new string[i];
        }

        public void AddRow(params string[] row)
        {
            csv.AddRow(row);
        }

        public void LoadData()
        {
            try
            {
                CSVFileManager.LoadFile($"{directory}/CSV_Files/Participant_{ID}.csv", out csv);
                csv.LoadedData.RemoveAt(0);
                //SetData(0, 10, 1);
                Debug.Log($"Data has been loaded");
                //Deserialize(FileManager.ReadToEnd($"{directory}/JSON_Files/Participant_{ID}.json"));
            }
            catch (System.Exception)
            {

                Debug.Log($"Data not loaded");
            }

        }

        ///<summary>
        /// method <c>SetData</c> sets data positions -and forwards array with the number of samples according to the input parameters.'
        /// <paramref name="start"/> start index of samples in seconds. Must be lower than stop time.
        /// <paramref name="stop"/> end index of samples in seconds. Must be greate than start time.
        /// <paramref name="interval"/> interval between samples in seconds. Must be a number between start and stop time.
        ///</summary>
        public void SetData(int start, int stop, int interval)
        {
            if (start < 0 || stop >= csv.LoadedData.Count)
            {
                return;
            }

            int offset = 1 / interval * 10;

            dataPositions = new float[stop + 1];
            dataForwards = new float[stop + 1];

            for (int i = start; i <= stop; i++)
            {
                // Position Vector Coordinates Indeces; X:1, Y:2, Z:3
                //dataPositions[i] = csv.LoadedData[i * offset][1];
                //dataPositions[i] = csv.LoadedData[i * offset][2];
                //dataPositions[i] = csv.LoadedData[i * offset][3];
                // Forward Vector Coordinates Indeces: X:4, Y:5, Z:6
                //dataForwards[i] = csv.LoadedData[i * offset][4];
                //dataForwards[i] = csv.LoadedData[i * offset][5];
                //dataForwards[i] = csv.LoadedData[i * offset][6];
            }
        }

        public void AddSerializeable(ISerializeable serializeable)
        {
            serializeables.Add(serializeable);
        }

        public void SerializeAll()
        {
            foreach (ISerializeable serializeable in serializeables)
            {
                jSavegame?.Add(serializeable.GetJsonKey(), serializeable.Serialize());
            }
        }

        #region Save Data 
        public class SaveData
        {
            public string[] time;
            public string[] positions;
            public string[] views;
            public string[] fps;

            public SaveData()
            {

            }
        }

        public JObject Serialize()
        {
            return JObject.Parse(JsonUtility.ToJson(saveData));
        }

        public void Deserialize(string jsonString)
        {
            saveData = JsonUtility.FromJson<SaveData>(jsonString);
        }

        public string GetJsonKey()
        {
            return $"Participant_{ID}";
        }

        #endregion

        private void OnDisable()
        {

        }

    }
}