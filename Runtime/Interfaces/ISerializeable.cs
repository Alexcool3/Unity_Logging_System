using Newtonsoft.Json.Linq;

namespace Logging_System
{
    public interface ISerializeable
    {
        JObject Serialize();

        void Deserialize(string jsonString);

        string GetJsonKey();
    }
}