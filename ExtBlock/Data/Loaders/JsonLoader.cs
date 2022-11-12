using System.IO;

using Newtonsoft.Json.Linq;

namespace ExtBlock.Data.Loaders
{
    public class JsonLoader : ILoader<JObject>
    {
        public JObject Load(string path)
        {
            string jsonString = File.ReadAllText(path, System.Text.Encoding.UTF8);
            JObject json = JObject.Parse(jsonString);
            return json;
        }
    }
}
