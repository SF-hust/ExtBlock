using System;
using System.Collections.Generic;

namespace ExtBlock.Core.State
{
    public sealed class BooleanStateProperty : StateProperty<bool>
    {
        public static BooleanStateProperty Create(string name)
        {
            return new BooleanStateProperty(name);
        }
        
        public static readonly IEnumerable<bool> BOOLEAN_PROPERTY_VALUES = new bool[2] { false, true };

        private BooleanStateProperty(string name) : base(name, 2) { }


        public override IEnumerable<bool> Values => BOOLEAN_PROPERTY_VALUES;

        public override bool ValueIsValid(bool value)
        {
            return true;
        }
        public override bool GetValueByIndex(int index)
        {
            return index switch
            {
                0 => false,
                1 => true,
                _ => throw new IndexOutOfRangeException(),
            };
        }

        public override int GetValueIndex(bool value)
        {
            return value? 1 : 0;
        }


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

        public override bool ValueEquals(StateProperty<bool>? other)
        {
            return true;
        }
    }
}
