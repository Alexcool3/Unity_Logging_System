using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Logging_System
{
    public static class CSVFileManager
    {
        public static bool CreateFolder(string foldername)
        {
            Directory.CreateDirectory(foldername);
            return Directory.Exists(foldername);
        }

        public static void SaveFile(string path, CSV csv, string delimiter = ";", bool overwrite = false)
        {
            if (overwrite)
            {
                FileManager.WriteAllLines(path, CSVLines(csv, delimiter));
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (string[] row in csv.WriteData)
                {
                    stringBuilder.AppendLine(string.Join(delimiter, row));
                }
                FileManager.AppendText(path, stringBuilder);
            }
        }

        public static List<string[]> LoadFile(string path, char delimiter = ';')
        {
            using (var reader = new StreamReader(path))
            {
                List<string[]> data = new List<string[]>();
                while (!reader.EndOfStream)
                {
                    data.Add(reader.ReadLine().Split(delimiter));
                }

                return data;
            }
        }

        public static void LoadFile(string path, ref List<string[]> data, char delimiter = ';')
        {
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    data.Add(reader.ReadLine().Split(delimiter));
                }
            }
        }

        public static void LoadFile(string path, out CSV csv, char delimiter = ';')
        {
            csv = new CSV();
            ReadDataFromCSVFile(path, ref csv, delimiter);
        }

        private static void ReadDataFromCSVFile(string path, ref CSV csv, char delimiter = ';')
        {
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string[] tmp = reader?.ReadLine()?.Split(delimiter);
                    csv?.AddRow(tmp);
                    csv?.AddRow(csv.String2Float(tmp));
                }
            }
        }

        public static string CSVFormat(CSV csv, string delimiter = ";")
        {
            if (csv != null)
            {
                string[] tmp = new string[csv.Rows];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = string.Join(delimiter, csv.WriteData[i]);
                }

                return string.Join(delimiter == "," ? ";" : ",", tmp);
            }

            return null;
        }

        public static string[] CSVLines(CSV csv, string delimiter = ";")
        {
            if (csv != null)
            {
                string[] tmp = new string[csv.Rows];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = string.Join(delimiter, csv.WriteData[i]);
                }

                return tmp;
            }

            return null;
        }
    }
}