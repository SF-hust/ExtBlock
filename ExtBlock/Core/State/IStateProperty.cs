using System;
using System.Collections.Generic;

namespace ExtBlock.Core.State
{
    public interface IStateProperty : IEquatable<IStateProperty>
    {
        public string Name { get; }

        public Type ValuesType { get; }

        public Type UnderlyingType { get; }

        public int CountOfValues { get; }

        public int BitCount { get; }

        public int BitMask { get; }

        public IEnumerable<int> Indices { get; }

        public bool IndexIsValid(int index);

        public bool EqualWithValues(IStateProperty? other);
    }
}
