using System.Collections.Generic;

using MongoDB.Bson;

namespace ExtBlock.Utility
{
    public static class BsonHelper
    {
        public static bool TryGetBool(BsonDocument bson, string name, out bool value)
        {
            if(bson.TryGetValue(name, out BsonValue bsonValue) && bsonValue.IsBoolean)
            {
                value = bsonValue.AsBoolean;
                return true;
            }
            value = default;
            return false;
        }

        public static bool GetBool(BsonDocument bson, string name)
        {
            if (bson.TryGetValue(name, out BsonValue bsonValue))
            {
                return bsonValue.AsBoolean;
            }
            throw new KeyNotFoundException($"there is no value named {name} in BsonDocument");
        }

        public static bool TryGetInt32(BsonDocument bson, string name, out int value)
        {
            if (bson.TryGetValue(name, out BsonValue bsonValue) && bsonValue.IsInt32)
            {
                value = bsonValue.AsInt32;
                return true;
            }
            value = default;
            return false;
        }

        public static int GetInt32(BsonDocument bson, string name)
        {
            if (bson.TryGetValue(name, out BsonValue bsonValue))
            {
                return bsonValue.AsInt32;
            }
            throw new KeyNotFoundException($"there is no value named {name} in BsonDocument");
        }

        public static bool TryGetInt64(BsonDocument bson, string name, out long value)
        {
            if (bson.TryGetValue(name, out BsonValue bsonValue) && bsonValue.IsInt64)
            {
                value = bsonValue.AsInt64;
                return true;
            }
            value = default;
            return false;
        }

        public static long GetInt64(BsonDocument bson, string name)
        {
            if (bson.TryGetValue(name, out BsonValue bsonValue))
            {
                return bsonValue.AsInt64;
            }
            throw new KeyNotFoundException($"there is no value named {name} in BsonDocument");
        }

        public static bool TryGetDouble(BsonDocument bson, string name, out double value)
        {
            if (bson.TryGetValue(name, out BsonValue bsonValue) && bsonValue.IsDouble)
            {
                value = bsonValue.AsDouble;
                return true;
            }
            value = default;
            return false;
        }

        public static double GetDouble(BsonDocument bson, string name)
        {
            if (bson.TryGetValue(name, out BsonValue bsonValue))
            {
                return bsonValue.AsDouble;
            }
            throw new KeyNotFoundException($"there is no value named {name} in BsonDocument");
        }
    }
}
