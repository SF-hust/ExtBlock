using System;
using System.Collections.Generic;
using System.Linq;

using ExtBlock.Math;

namespace ExtBlock.Core.State
{
    public abstract class StateProperty : IEquatable<StateProperty>
    {
        public StateProperty(string name, int valueCount)
        {
            if (valueCount < MIN_VALUE_COUNT || valueCount > MAX_VALUE_COUNT)
            {
                throw new Exception($"value count must be in [{MIN_VALUE_COUNT}, {MAX_VALUE_COUNT}]");
            }
            _name = name;
            _count = valueCount;
            _bitCount = MathHelper.StorageBitCount(valueCount);
            _bitMask = MathHelper.LownBitOne(_bitCount);
        }

        /// <summary>
        /// 一个 StateProperty 取值的最小数量
        /// </summary>
        public const int MIN_VALUE_COUNT = 2;

        /// <summary>
        /// 一个 StateProperty 取值的最大数量
        /// </summary>
        public const int MAX_VALUE_COUNT = 65536;

        /// <summary>
        /// 属性的名称, 名称相同即视为同一属性
        /// </summary>
        public string Name => _name;
        private readonly string _name;

        /// <summary>
        /// 属性值的类型
        /// </summary>
        public abstract Type ValuesType { get; }

        /// <summary>
        /// 属性值的下标的类型
        /// </summary>
        public Type IndexType => typeof(int);

        /// <summary>
        /// 此属性可能取值的数量
        /// </summary>
        public int CountOfValues => _count;
        private readonly int _count;

        /// <summary>
        /// 为表示属性不同取值所需的最少二进制位的数量
        /// </summary>
        public int BitCount => _bitCount;
        private readonly int _bitCount;

        /// <summary>
        /// BitCount 所对应的位掩码
        /// </summary>
        public int BitMask => _bitMask;
        private readonly int _bitMask;

        /// <summary>
        /// 某个属性值对应的下标是否在此属性可取值范围内
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IndexIsValid(int index)
        {
            return index >= 0 && index < _count;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is StateProperty p && Equals(p);
        }

        public bool Equals(StateProperty? other)
        {
            if(this == other)
            {
                return true;
            }
            return _name == other?.Name;
        }

        public override string ToString()
        {
            return _name + " : " + ValuesType.ToString();
        }

        /// <summary>
        /// 此属性是否与另一属性不仅名字相同, 取值范围也相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool EqualWithValues(StateProperty? other);


    }

    public abstract class StateProperty<T> : StateProperty
    {
        public StateProperty(string name, int valueCount) : base(name, valueCount)
        {
        }

        public override Type ValuesType => typeof(T);

        public override bool EqualWithValues(StateProperty? other)
        {
            if (!Equals(other))
            {
                return false;
            }
            if (other is StateProperty<T> o)
            {
                return ValueEquals(o);
            }
            return false;
        }

        /// <summary>
        /// 从字符串中解析出值并获取其下标, 解析失败则返回 -1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int ParseValueToIndex(string str)
        {
            if(ParseValue(str, out T value))
            {
                return GetIndexByValue(value);
            }
            return -1;
        }

        /// <summary>
        /// 通过下标获取对应值并转化为字符串, 超出范围则抛出 IndexOutOfRangeException
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string ValueIndexToString(int i)
        {
            return ValueToString(GetValueByIndex(i));
        }

        /*
         * 以下是每种具体属性都需要实现的部分
         */

        /// <summary>
        /// 此属性所有的可能取值
        /// </summary>
        public abstract IEnumerable<T> Values { get; }

        /// <summary>
        /// 此属性是否可以取某值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool ValueIsValid(T value);

        /// <summary>
        /// 使用下标取得某个取值范围内的值, 如果下标超出范围则应抛 IndexOutOfRangeException
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract T GetValueByIndex(int index);

        /// <summary>
        /// 获取某个值在属性中对应的下标, 如果不能取到则返回 -1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract int GetIndexByValue(T value);

        /// <summary>
        /// 从一个 string 中解析出值, 返回值表示是否解析成功, 与 ValueToString() 对应
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool ParseValue(string str, out T value);

        /// <summary>
        /// 将某个取值转换为字符串, 这个字符串应能被 ParseValue() 解析得到这个值,
        /// 值超出范围时应返回一个不能被 ParseValue() 解析的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract string ValueToString(T value);

        /// <summary>
        /// 两个属性的取值范围以及每个值对应的下标是否相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool ValueEquals(StateProperty<T>? other);

    }
}
