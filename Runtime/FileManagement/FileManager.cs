using UnityEngine;
using System.IO;
using System.Text;

namespace Logging_System.FileManagement
{
    public static class FileManager
    {
        public static bool CreateFile(string filename)
        {
            File.Create(filename).Close();
            return File.Exists(filename);
        }

        public static bool CreateFolder(string foldername)
        {
            Directory.CreateDirectory(foldername);
            return Directory.Exists(foldername);
        }

        public static void WriteLine(string path, string message)
        {
            StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.WriteLine(message);
            streamWriter.Close();
        }

        // Assumes file DOES NOT exist.
        public static void WriteAllLines(string path, string[] lines)
        {
            File.WriteAllLines(path, lines);
        }

        public static void WriteLine(string path, StringBuilder message)
        {
            AppendText(path, message);
        }

        public static void AppendText(string path, string message)
        {
            try
            {
                StreamWriter streamWriter = File.AppendText(path);
                streamWriter.Write(message);
                streamWriter.Close();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"The following error occured:{e.Message}");
            }
        }

        public static void AppendText(string path, StringBuilder message)
        {
            try
            {
                StreamWriter streamWriter = File.AppendText(path);
                streamWriter.Write(message);
                streamWriter.Close();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"The following error occured:{e.Message}");
            }
        }

        public static void SaveBinaryFile(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public static string ReadToEnd(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    StreamReader streamReader = new StreamReader(path);
                    string text = streamReader.ReadToEnd();
                    streamReader.Close();
                    return text;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"The following error occured: {e.Message}");
                    return null;
                }
            }

            return null;
        }

    }
}