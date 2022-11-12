using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtBlock.Utility.Container
{
    public class HashIdMap<T> : IIdMap<T>
        where T : class
    {
        protected readonly Dictionary<T, int> _idByValue = new Dictionary<T, int>();
        protected readonly List<T> _valueById = new List<T>();

        public HashIdMap(IEnumerable<T> values)
        {
            _valueById = values.ToList();
            for (int i = 0; i < _valueById.Count; i++)
            {
                _idByValue.Add(_valueById[i], i);
            }
        }

        public int Size => _valueById.Count;

        public IEnumerator<T> GetEnumerator()
        {
            return _valueById.GetEnumerator();
        }

        public int IdFor(T value)
        {
            if (_idByValue.TryGetValue(value, out int id))
            {
                return id;
            }
            return -1;
        }

        public T ValueFor(int id)
        {
            if (id < 0 || id >= Size)
            {
                throw new IndexOutOfRangeException();
            }
            return _valueById[id];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
