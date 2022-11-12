using System;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

namespace ExtBlock.Utility
{
    public static class JsonHelper
    {
        public static bool TryGetBool(JObject obj, string name, out bool value)
        {

            if (obj.TryGetValue(name, out JToken? node) && node.Type == JTokenType.Boolean)
            {
                value = (bool)node;
                return true;
            }
            value = default;
            return false;
        }

        public static bool GetBool(JObject obj, string name)
        {
            if (obj.TryGetValue(name, out JToken? node) && node.Type == JTokenType.Boolean)
            {
                return (bool)node;
            }
            throw new InvalidOperationException($"failed to get {name}(bool) from json");
        }

        public static bool TryGetString(JObject obj, string name, out string value)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JValue jv && jv.Type == JTokenType.String)
            {
                value = (string)jv!;
                return true;
            }
            value = string.Empty;
            return false;
        }

        public static string GetString(JObject obj, string name)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JValue jv && jv.Type == JTokenType.String)
            {
                return (string)jv!;
            }
            throw new InvalidOperationException($"failed to get {name}(string) from json");
        }

        public static bool TryGetInt(JObject obj, string name, out int value)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JValue jv && jv.Type == JTokenType.Integer)
            {
                value = (int)jv;
                return true;
            }
            value = default;
            return false;
        }

        public static int GetInt(JObject obj, string name)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JValue jv && jv.Type == JTokenType.Integer)
            {
                return (int)jv;
            }
            throw new InvalidOperationException($"failed to get {name}(int) from json");
        }

        public static bool TryGetDouble(JObject obj, string name, out double value)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JValue jv && jv.Type == JTokenType.Float)
            {
                value = (double)jv;
                return true;
            }
            value = default;
            return false;
        }

        public static double GetDouble(JObject obj, string name)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JValue jv && jv.Type == JTokenType.Float)
            {
                return (double)node;
            }
            throw new InvalidOperationException($"failed to get {name}(double) from json");
        }

        public static bool TryGetArray(JObject obj, string name, [NotNullWhen(true)] out JArray? value)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JArray array)
            {
                value = array;
                return true;
            }
            value = null;
            return false;
        }

        public static JArray GetArray(JObject obj, string name)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JArray array)
            {
                return array;
            }
            throw new InvalidOperationException($"failed to get {name}(array) from json");
        }

        public static bool TryGetObject(JObject obj, string name, [NotNullWhen(true)] out JObject? value)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JObject jsonObject)
            {
                value = jsonObject;
                return true;
            }
            value = null;
            return false;
        }

        public static JObject GetObject(JObject obj, string name)
        {
            if (obj.TryGetValue(name, out JToken? node) && node is JObject jsonObject)
            {
                return jsonObject;
            }
            throw new InvalidOperationException($"failed to get {name}(array) from json");
        }

    }
}
