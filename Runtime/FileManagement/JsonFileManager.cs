namespace Logging_System
{
    public static class JsonFileManager
    {
        public static void SaveJson(string path, string jsonString)
        {
            FileManager.WriteLine(path, jsonString);
        }

        public static string LoadJson(string path)
        {
            return FileManager.ReadToEnd(path); ;
        }
    }
}