using System;
using System.Collections.Generic;
using System.Linq;

using ExtBlock.Math;

namespace ExtBlock.Core.State
{
    public abstract class StateProperty<T> : IStateProperty
    {
        /// <summary>
        /// min value count a stateProperty can use
        /// </summary>
        public const int MIN_VALUE_COUNT = 2;
        /// <summary>
        /// max value count a stateProperty can use
        /// </summary>
        public const int MAX_VALUE_COUNT = 65536;

        protected StateProperty(string name, int valueCount)
        {
            if(valueCount < MIN_VALUE_COUNT || valueCount > MAX_VALUE_COUNT)
            {
                throw new Exception($"value count must be in [{MIN_VALUE_COUNT}, {MAX_VALUE_COUNT}]");
            }
            _name = name;
            _count = valueCount;
            _bitCount = MathHelper.StorageBitCount(valueCount);
            _bitMask = MathHelper.LownBitOne(_bitCount);
        }

        private readonly string _name;
        public string Name => _name;

        public Type ValuesType => typeof(T);

        public Type UnderlyingType => typeof(int);

        private readonly int _count;
        public int CountOfValues => _count;

        private readonly int _bitCount;
        public int BitCount => _bitCount;

        private readonly int _bitMask;
        public int BitMask => _bitMask;

        public IEnumerable<int> ValueIndices => Enumerable.Range(0, _count - 1);

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
            return obj is IStateProperty p && Equals(p);
        }

        public bool Equals(IStateProperty? other)
        {
            if(this == other)
            {
                return true;
            }
            return _name.Equals(other?.Name);
        }

        public bool EqualWithValues(IStateProperty? other)
        {
            if(!Equals(other))
            {
                return false;
            }
            if(other is StateProperty<T> o)
            {
                return ValueEquals(o);
            }
            return false;
        }

        public override string ToString()
        {
            return _name + " : " + ValuesType.ToString();
        }

        /// <summary>
        /// typed values this StateProperty can take
        /// </summary>
        public abstract IEnumerable<T> Values { get; }

        /// <summary>
        /// weather a value this StateProperty can take
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool ValueIsValid(T value);

        /// <summary>
        /// get typed value with index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract T GetValueByIndex(int index);

        /// <summary>
        /// get index of a value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>-1 if value can't be taken by this property</returns>
        public abstract int GetValueIndex(T value);

        /// <summary>
        /// parse value from a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool ParseValue(string str, out T value);

        /// <summary>
        /// get a string representing the value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract string ValueToString(T value);

        /// <summary>
        /// whether this property's values equals with another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool ValueEquals(StateProperty<T>? other);

    }
}
