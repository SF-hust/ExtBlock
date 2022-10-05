using System;
using System.Collections.Generic;

namespace ExtBlock.Core.Property
{
    public sealed class BooleanStateProperty : StateProperty<bool>
    {
        public static readonly IEnumerable<bool> BOOLEAN_PROPERTY_VALUES = new bool[2] { false, true };
        private static readonly object[] BOXED_VALUES = new object[2] { false, true };

        private BooleanStateProperty(string name) : base(name) { }
        public static BooleanStateProperty Create(string name)
        {
            return new BooleanStateProperty(name);
        }

        public override int CountOfValues => 2;
        public override IEnumerable<bool> AllValues => BOOLEAN_PROPERTY_VALUES;

        public override bool ParseValue(string str, out bool value)
        {
            if (str.Equals(bool.FalseString))
            {
                value = false;
                return true;
            }
            if (str.Equals(bool.TrueString))
            {
                value = true;
                return true;
            }
            value = false;
            return false;
        }
        public override string ValueToString(bool value)
        {
            return value ? bool.TrueString : bool.FalseString;
        }
        public override bool ValueIsValid(bool value)
        {
            return true;
        }

        public override bool EqualWithValues(StateProperty<bool>? other)
        {
            if (other == this)
            {
                return true;
            }
            return other is BooleanStateProperty && Name == other.Name;
        }

        public override int GetValueIndex(bool value)
        {
            return value? 1 : 0;
        }

        public override object this[int index] => BOXED_VALUES[index];

        public override bool GetValueByIndex(int index)
        {
            return index switch
            {
                0 => false,
                1 => true,
                _ => throw new IndexOutOfRangeException(),
            };
        }
    }
}
