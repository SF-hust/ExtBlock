using System;

using Newtonsoft.Json.Linq;

namespace ExtBlock.Utility
{
    public static class JsonHelper
    {
        public static bool TryGetBool(JObject obj, string name, out bool value, bool acceptCast = false)
        {
            if (obj.TryGetValue(name, out JToken? token))
            {
                if (acceptCast)
                {
                    bool? v = (bool?)token;
                    if(v != null)
                    {
                        value = v.Value;
                        return true;
                    }
                }
                else if (token.Type == JTokenType.Boolean)
                {
                    value = (bool)token;
                    return true;
                }
            }
            value = false;
            return false;
        }

        public static bool GetBool(JObject obj, string name, bool acceptCast = false)
        {
            if (obj.TryGetValue(name, out JToken? token) && (acceptCast || token.Type == JTokenType.Boolean))
            {
                return (bool)token;
            }
            throw new ArgumentException($"failed to get {name}(bool) from json");
        }

        public static bool TryGetString(JObject obj, string name, out string value)
        {
            if (obj.TryGetValue(name, out JToken? token) && token.Type == JTokenType.String)
            {
                value = ((string?)token)!;
                return true;
            }
            value = string.Empty;
            return false;
        }

        public static string GetString(JObject obj, string name)
        {
            if (obj.TryGetValue(name, out JToken? token) && token.Type == JTokenType.String)
            {
                return ((string?)token)!;
            }
            throw new ArgumentException($"failed to get {name}(bool) from json");
        }
    }
}
