using System;

namespace ExtBlock.Core.Property
{
    public interface IProperty : IComparable<IProperty>
    {
        public string Name { get; }
        public Type TypeOfValue { get; }

        public abstract bool ParseValue(string str, out object? value);
        public abstract string ValueToString(object value);
        public abstract bool ValueIsValid(object value);
    }
}