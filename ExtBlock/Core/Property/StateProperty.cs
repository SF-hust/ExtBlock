using System;
using System.Collections.Generic;

namespace ExtBlock.Core.Property
{
    public abstract class StateProperty<T> : Property<T>, IStateProperty
        where T : notnull
    {
        protected StateProperty(string name) : base(name)
        {
        }

        public bool EqualWithValues(IStateProperty? other)
        {
            if(other is StateProperty<T> o)
            {
                return EqualWithValues(o);
            }
            return false;
        }
        public int GetValueIndex(object value)
        {
            if(value is T v)
            {
                return GetValueIndex(v);
            }
            return -1;
        }

        public virtual object this[int index] => GetValueByIndex(index);

        public abstract int CountOfValues { get; }
        public abstract IEnumerable<T> AllValues { get; }


        public abstract bool EqualWithValues(StateProperty<T>? other);
        public abstract int GetValueIndex(T value);
        public abstract T GetValueByIndex(int index);

        public abstract override bool ParseValue(string str, out T value);
        public abstract override string ValueToString(T value);
        public abstract override bool ValueIsValid(T value);
    }
}
