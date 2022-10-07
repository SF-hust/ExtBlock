using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtBlock.Core.State
{
    public class IntStateProperty : StateProperty<int>
    {
        public static IntStateProperty Create(string name, int from, int to)
        {
            return new IntStateProperty(name, from, to);
        }
        public static IntStateProperty Create(string name)
        {
            return new IntStateProperty(name, 0, 1);
        }

        protected IntStateProperty(string name, int from, int to) : base(name, to - from + 1)
        {
            _from = from;
            _to = to;
        }

        protected readonly int _from, _to;

        public override IEnumerable<int> Values => Enumerable.Range(_from, _to);

        public override bool ValueIsValid(int value)
        {
            return value >= _from && value <= _to;
        }

        public override int GetValueByIndex(int index)
        {
            if(index >= 0 && index < CountOfValues)
            {
                return index + _from;
            }
            throw new IndexOutOfRangeException();
        }

        public override int GetValueIndex(int value)
        {
            if(!ValueIsValid(value))
            {
                return -1;
            }
            return value - _from;
        }

        public override bool ParseValue(string str, out int value)
        {
            return int.TryParse(str, out value) && ValueIsValid(value);
        }

        public override string ValueToString(int value)
        {
            return value.ToString();
        }

        public override bool ValueEquels(StateProperty<int>? other)
        {
            return other is IntStateProperty ip && _from == ip._from && _to == ip._to;
        }

        public override string ToString()
        {
            return base.ToString() + $" , in [{_from}, {_to}]";
        }
    }
}
