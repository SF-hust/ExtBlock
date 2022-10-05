using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtBlock.Core.Property
{
    public class IntStateProperty : StateProperty<int>
    {
        protected IntStateProperty(string name, int from, int to) : base(name)
        {
            if (from > to)
            {
                throw new ArgumentException($"Fail to create IntProperty : from(= {from}) > to(= {to})");
            }
            _from = from;
            _to = to;
            BoxedValues = new object[CountOfValues];
            for(int i = 0; i < CountOfValues; ++i)
            {
                BoxedValues[i] = i + _from;
            }
        }
        public static IntStateProperty Create(string name, int from, int to)
        {
            return new IntStateProperty(name, from, to);
        }
        public static IntStateProperty Create(string name)
        {
            return new IntStateProperty(name, 0, 1);
        }

        protected readonly int _from, _to;
        public override int CountOfValues => _to - _from + 1;
        public override IEnumerable<int> AllValues => Enumerable.Range(_from, _to);

        protected object[] BoxedValues;

        public override bool ParseValue(string str, out int value)
        {
            return int.TryParse(str, out value) && ValueIsValid(value);
        }
        public override string ValueToString(int value)
        {
            return value.ToString();
        }
        public override bool ValueIsValid(int value)
        {
            return value >= _from && value <= _to;
        }

        public override bool EqualWithValues(StateProperty<int>? other)
        {
            if (Equals(other) && other is IntStateProperty p)
            {
                return _from == p._from && _to == p._to;
            }
            return false;
        }

        public override int GetValueIndex(int value)
        {
            if(!ValueIsValid(value))
            {
                return -1;
            }
            return value - _from;
        }

        public override object this[int index] => BoxedValues[index];

        public override int GetValueByIndex(int index)
        {
            if(index >= 0 && index < CountOfValues)
            {
                return index + _from;
            }
            throw new IndexOutOfRangeException();
        }

        public override string ToString()
        {
            return base.ToString() + $", in [{_from}, {_to}]";
        }

    }
}
