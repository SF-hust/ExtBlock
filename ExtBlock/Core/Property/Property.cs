using System;
using System.Diagnostics.CodeAnalysis;

namespace ExtBlock.Core.Property
{
    public abstract class Property<T> : IProperty, IComparable<Property<T>>
        where T : notnull
    {
        protected Property(string name)
        {
            _name = name;
            _hashCodeCache = GenerateHashCode();
        }
        private int GenerateHashCode()
        {
            return Name.GetHashCode();
        }

        private readonly string _name;
        public string Name { get => _name; }

        public Type ValuesType { get => typeof(T); }
        private readonly int _hashCodeCache;
        public sealed override int GetHashCode()
        {
            return _hashCodeCache;
        }
        public sealed override bool Equals(object? obj)
        {
            if (obj == this)
            {
                return true;
            }
            return obj is Property<T> other && Name == other.Name;
        }

        public int CompareTo(IProperty? other)
        {
            if (this == other)
            {
                return 0;
            }
            return Name.CompareTo(other?.Name);
        }
        public int CompareTo(Property<T>? other)
        {
            if (this == other)
            {
                return 0;
            }
            return Name.CompareTo(other?.Name);
        }

        public bool ParseValue(string str,[NotNullWhen(true)] out object? value)
        {
            if (ParseValue(str, out T vout))
            {
                value = vout;
                return true;
            }
            value = null;
            return false;
        }
        public string ValueToString(object value)
        {
            if(value is T v)
            {
                return ValueToString(v);
            }
            throw new Exception($"Value is not type of {typeof(T)}");
        }
        public bool ValueIsValid(object value)
        {
            if (value is T v)
            {
                return ValueIsValid(v);
            }
            throw new Exception($"Value is not type of {typeof(T)}");
        }

        public abstract bool ParseValue(string str, [NotNullWhen(true)] out T value);
        public abstract string ValueToString(T value);
        public virtual bool ValueIsValid(T value)
        {
            return true;
        }

        public override string ToString()
        {
            return Name + " : " + ValuesType.ToString();
        }
    }
}