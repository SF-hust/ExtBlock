using System;
using System.Collections.Generic;
using System.Linq;
using ExtBlock.Math;

namespace ExtBlock.Core.State
{
    public abstract class StateProperty<T> : IStateProperty
    {
        public const int MIN_VALUE_COUNT = 2;
        public const int MAX_VALUE_COUNT = 65536;

        protected StateProperty(string name, int count)
        {
            if(count < MIN_VALUE_COUNT || count > MAX_VALUE_COUNT)
            {
                throw new Exception($"value count must be in [{MIN_VALUE_COUNT}, {MAX_VALUE_COUNT}]");
            }
            _name = name;
            _count = count;
            _bitCount = MathHelper.StorageBitCount(count);
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

        public IEnumerable<int> Indices => Enumerable.Range(0, _count - 1);

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
                return ValueEquels(o);
            }
            return false;
        }

        public override string ToString()
        {
            return _name + " : " + ValuesType.ToString();
        }

        public abstract IEnumerable<T> Values { get; }
        public abstract bool ValueIsValid(T value);
        public abstract T GetValueByIndex(int index);
        public abstract int GetValueIndex(T value);
        public abstract bool ParseValue(string str, out T value);
        public abstract string ValueToString(T value);
        public abstract bool ValueEquels(StateProperty<T>? other);

    }
}
