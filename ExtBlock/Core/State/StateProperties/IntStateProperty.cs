using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtBlock.Core.State
{
    /// <summary>
    /// IntStateProperty: 可以取指定连续范围内的 int 值(可以是负值)
    /// </summary>
    public class IntStateProperty : StateProperty<int>
    {
        /// <summary>
        /// 创建一个指定名字的 IntStateProperty, 取值范围 [from, to]
        /// </summary>
        /// <param name="name"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static IntStateProperty Create(string name, int from, int to)
        {
            return new IntStateProperty(name, from, to);
        }

        /// <summary>
        /// 创建一个指定名字的 IntStateProperty, 取值范围 [0, 1]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        public override int GetIndexByValue(int value)
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
            if(!ValueIsValid(value))
            {
                return "!# " + value.ToString(); 
            }
            return value.ToString();
        }

        public override bool ValueEquals(StateProperty<int>? other)
        {
            return other is IntStateProperty ip && _from == ip._from && _to == ip._to;
        }

        public override string ToString()
        {
            return base.ToString() + $" , values: [{_from}, {_to}]";
        }
    }
}
