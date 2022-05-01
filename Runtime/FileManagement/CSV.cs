using System.Collections.Generic;
using System;
using System.Globalization;

namespace Logging_System
{
    public class CSV
    {
        private List<string[]> writeData = new List<string[]>();
        private List<float[]> loadedData = new List<float[]>();

        public List<string[]> WriteData { get => writeData; }
        public List<float[]> LoadedData { get => loadedData; }

        public int Rows { get => WriteData.Count; }

        public CSV()
        {

        }

        public CSV(string[] titleRow)
        {
            writeData.Add(titleRow);
        }

        public void AddRow(string[] row)
        {
            writeData?.Add(row);
        }

        public void AddRow(float[] row)
        {
            loadedData?.Add(row);
        }

        public float[] String2Float(params string[] strings)
        {
            float[] tmp = new float[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                try
                {
                    tmp[i] = float.Parse(strings[i], CultureInfo.InvariantCulture.NumberFormat);
                }
                catch (Exception)
                {

                    continue;
                }

            }

            return tmp;
        }
    }
}