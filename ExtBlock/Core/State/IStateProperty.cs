using System;
using System.Collections.Generic;

namespace ExtBlock.Core.State
{
    /// <summary>
    /// representing a StateProperty.
    /// a StateProperty has fixed count of values, and every value correspondings to a specified index
    /// </summary>
    public interface IStateProperty : IEquatable<IStateProperty>
    {
        /// <summary>
        /// name of StateProperty
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// type of property's value
        /// </summary>
        public Type ValuesType { get; }

        /// <summary>
        /// type of property's value index
        /// </summary>
        public Type UnderlyingType { get; }

        /// <summary>
        /// count of values this property can be
        /// </summary>
        public int CountOfValues { get; }

        /// <summary>
        /// least count of bit to store all variants of this property's value
        /// </summary>
        public int BitCount { get; }

        /// <summary>
        /// a interger that lowest [BitCount] bits are 1, others are 0
        /// </summary>
        public int BitMask { get; }

        /// <summary>
        /// all values' indices, exactly [0, CountOfValues-1]
        /// </summary>
        public IEnumerable<int> ValueIndices { get; }

        /// <summary>
        /// weather a value's index is valid
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true if index is in range</returns>
        public bool IndexIsValid(int index);

        /// <summary>
        /// weather a StateProperty and its values equals with another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualWithValues(IStateProperty? other);
    }
}
