using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace ExtBlock.Core.State
{
    /// <summary>
    /// EnumStateProperty<T>: 取值为枚举类型, T 是对应的枚举类型,
    /// 注意在解析字符串与将值转为字符串时会使用全小写, 因此使用的枚举不可以存在相同的成员名(在忽略大小写的情况下)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumStateProperty<T> : StateProperty<T>
        where T : struct, Enum
    {
        /// <summary>
        /// 创建一个指定名字的 EnumStateProperty, 取值范围为枚举的成员与顺序为枚举内成员的声明顺序
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 创建一个指定名字的 EnumStateProperty, 取值与顺序为 values 的声明顺序
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// 检查枚举的成员是否符合要求
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void CheckEnumEntries()
        {
            if (EnumCheckCache.Enums.ContainsKey(typeof(T)))
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
            EnumCheckCache.Enums[typeof(T)] = null;
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

        public override IEnumerable<T> Values => _values;
        protected T[] _values;

        protected string[] _keys;

        public override bool ValueIsValid(T value)
        {
            return _values.Contains(value);
        }

        public override int GetIndexByValue(T value)
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
            int index = GetIndexByValue(value);
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

    /// <summary>
    /// 已被检查过符合要求的 enum 类型
    /// </summary>
    internal static class EnumCheckCache
    {
        public static ConcurrentDictionary<Type, object?> Enums = new ConcurrentDictionary<Type, object?>();
    }
}
