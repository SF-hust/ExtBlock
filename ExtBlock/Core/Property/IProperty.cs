using System;

namespace ExtBlock.Core.Property
{
    public interface IProperty : IComparable<IProperty>
    {
        /// <summary>
        /// name of this property
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// type of this property's value
        /// </summary>
        public Type ValuesType { get; }

        public abstract bool ParseValue(string str, out object? value);
        public abstract string ValueToString(object value);
        public abstract bool ValueIsValid(object value);
    }
}