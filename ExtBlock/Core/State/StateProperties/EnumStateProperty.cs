using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace ExtBlock.Core.State
{
    public class EnumStateProperty<T> : StateProperty<T>
        where T : struct, Enum
    {
        public static EnumStateProperty<T> Create(string name)
        {
            CheckEnumEntries();
            Array array = Enum.GetValues(typeof(T));
            Debug.Assert(array.Rank == 1);
            List<T> values = new List<T>(array.Length);
            for(int i = 0; i < array.Length; ++i)
            {
                object o = array.GetValue(i);
                Debug.Assert(o is T);
                T v = (T)o;
                values.Add(v);
            }
            return new EnumStateProperty<T>(name, values.ToArray());
        }

        public static EnumStateProperty<T> Create(string name, IEnumerable<T> values)
        {
            CheckEnumEntries();
            List<T> valueList = new List<T>();
            foreach (T value in values)
            {
                if (Enum.IsDefined(typeof(T), value))
                {
                    if (valueList.Contains(value))
                    {
                        throw new Exception($"Fail to create EnumProperty : value \"{value}\" presents twice in values");
                    }
                    valueList.Add(value);
                }
                else
                {
                    throw new Exception($"Fail to create EnumProperty : value \"{value}\" is not in Enum \"{typeof(T).FullName}\"");
                }
            }
            return new EnumStateProperty<T>(name, valueList.ToArray());
        }

        private static void CheckEnumEntries()
        {
            if (EnumCheckCache.Enums.Contains(typeof(T)))
            {
                return;
            }
            if(Enum.GetUnderlyingType(typeof(T)) != typeof(int))
            {
                throw new Exception($"Check enum entries for EnumStateProperty failed : enum \"{typeof(T).FullName}\" must has underlying type of int");
            }
            HashSet<string> keys = new HashSet<string>();
            foreach (string key in Enum.GetNames(typeof(T)))
            {
                string lower = key.ToLower();
                if (keys.Contains(key))
                {
                    throw new Exception($"Check enum entries for EnumStateProperty failed : enum \"{typeof(T).FullName}\" contains same ignorcase keys \"{lower}\"");
                }
                keys.Add(lower);
            }
            EnumCheckCache.Enums.Add(typeof(T));
        }

        protected EnumStateProperty(string name, T[] values) : base(name, values.Length)
        {
            _values = values;
            _keys = new string[values.Length];
            for(int i = 0; i < values.Length; ++i)
            {
                string? key = Enum.GetName(typeof(T), values[i]);
                Debug.Assert(key != null);
                _keys[i] = key;
            }
        }

        protected T[] _values;
        public override IEnumerable<T> Values => _values;
        protected string[] _keys;

        public override bool ValueIsValid(T value)
        {
            return _values.Contains(value);
        }

        public override int GetValueIndex(T value)
        {
            for(int i = 0; i < _values.Length; ++i)
            {
                if (value.Equals(_values[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public override T GetValueByIndex(int index)
        {
            if(index >= 0 && index < CountOfValues)
            {
                return _values[index];
            }
            throw new IndexOutOfRangeException();
        }

        public override bool ParseValue(string str, out T value)
        {
            for(int i = 0; i < CountOfValues; ++i)
            {
                if(str == _keys[i])
                {
                    value = _values[i];
                    return true;
                }
            }
            value = default;
            return false;
        }

        public override string ValueToString(T value)
        {
            int index = GetValueIndex(value);
            return index == -1 ? $"[undefined value(= {value})]" : _keys[index];
        }

        public override bool ValueEquals(StateProperty<T>? other)
        {
            return other is EnumStateProperty<T> p && _values.SequenceEqual(p._values);
        }

        public override string ToString()
        {
            return base.ToString() + $", values = {from v in _values select Enum.GetName(typeof(T), v).ToLower()}";
        }
    }

    class EnumCheckCache
    {
        public static ConcurrentBag<Type> Enums = new ConcurrentBag<Type>();
    }
}
