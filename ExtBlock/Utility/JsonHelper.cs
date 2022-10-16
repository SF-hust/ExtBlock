using System;
using System.Text.Json.Nodes;

namespace ExtBlock.Utility
{
    public static class JsonHelper
    {
        public static bool TryGetBool(JsonObject obj, string name, out bool value)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                value = node.GetValue<bool>();
                return true;
            }
            value = default;
            return false;
        }

        public static bool GetBool(JsonObject obj, string name)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                return node.GetValue<bool>();
            }
            throw new FormatException($"failed to get {name}(bool) from json");
        }

        public static bool TryGetString(JsonObject obj, string name, out string value)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                value = node.GetValue<string>();
                return true;
            }
            value = string.Empty;
            return false;
        }

        public static string GetString(JsonObject obj, string name)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                return node.GetValue<string>();
            }
            throw new FormatException($"failed to get {name}(string) from json");
        }

        public static bool TryGetInt(JsonObject obj, string name, out int value)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                value = node.GetValue<int>();
                return true;
            }
            value = default;
            return false;
        }

        public static int GetInt(JsonObject obj, string name)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                return node.GetValue<int>();
            }
            throw new FormatException($"failed to get {name}(int) from json");
        }

        public static bool TryGetDouble(JsonObject obj, string name, out double value)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                value = node.GetValue<double>();
                return true;
            }
            value = default;
            return false;
        }

        public static double GetDouble(JsonObject obj, string name)
        {
            if (obj.TryGetPropertyValue(name, out JsonNode? node) && node is JsonValue)
            {
                return node.GetValue<double>();
            }
            throw new FormatException($"failed to get {name}(double) from json");
        }

    }
}
