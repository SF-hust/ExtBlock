using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ExtBlock.Core.Property
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

        protected EnumStateProperty(string name, T[] values) : base(name)
        {
            _values = values;
            BoxedValues = new object[values.Length];
            for(int i = 0; i < values.Length; ++i)
            {
                BoxedValues[i] = values[i];
            }
        }

        private static void CheckEnumEntries()
        {
            if(Enum.GetUnderlyingType(typeof(T)) != typeof(int))
            {
                throw new Exception($"Fail to check enum entries : enum \"{typeof(T).FullName}\" must has underlying type of int");
            }
            HashSet<string> keys = new HashSet<string>();
            foreach (string key in Enum.GetNames(typeof(T)))
            {
                string lower = key.ToLower();
                if (keys.Contains(key))
                {
                    throw new Exception($"Fail to check enum entries : enum \"{typeof(T).FullName}\" contains same ignorcase keys \"{lower}\"");
                }
                keys.Add(lower);
            }
        }

        protected T[] _values;
        public override int CountOfValues => _values.Length;
        public override IEnumerable<T> AllValues => _values;

        protected object[] BoxedValues;

        public override bool ParseValue(string str, out T value)
        {
            if (Enum.TryParse(str, true, out T v))
            {
                value = v;
                return ValueIsValid(v);
            }
            value = default;
            return false;
        }
        public override string ValueToString(T value)
        {
            string? key = Enum.GetName(typeof(T), value);
            if(key == null)
            {
                return $"[undefined value(= {value})]";
            }
            return key.ToLower();
        }
        public override bool ValueIsValid(T value)
        {
            return _values.Contains(value);
        }

        public override bool EqualWithValues(StateProperty<T>? other)
        {
            if (Equals(other) && other is EnumStateProperty<T> p)
            {
                return _values.SequenceEqual(p._values);
            }
            return false;
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

        public override object this[int index] => BoxedValues[index];

        public override T GetValueByIndex(int index)
        {
            if(index >= 0 && index < CountOfValues)
            {
                return _values[index];
            }
            throw new IndexOutOfRangeException();
        }

        public override string ToString()
        {
            return base.ToString() + $", values = {from v in _values select Enum.GetName(typeof(T), v)}";
        }
    }
}
