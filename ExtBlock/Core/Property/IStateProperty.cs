using System;

namespace ExtBlock.Core.Property
{
    public interface IStateProperty : IProperty
    {
        public int CountOfValues { get; }

        public bool EqualWithValues(IStateProperty? other);

        public int GetValueIndex(object value);

        public object this[int index] { get; }
    }
}
