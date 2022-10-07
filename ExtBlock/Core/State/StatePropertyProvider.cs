using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExtBlock.Core.Property;

namespace ExtBlock.Core.State
{
    public class StatePropertyProvider : IStatePropertyProvider, IEnumerable<KeyValuePair<IStateProperty, int>>
    {
        public const int MaxPackedBitCount = 16;

        public int this[IStateProperty property]
        {
            get => GetIndex(property);
            set
            {
                if(!TrySetIndex(property, value))
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public KeyValuePair<IStateProperty, int> this[int index] => _properties[index];

        public IEnumerator<KeyValuePair<IStateProperty, int>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        public List<IStateProperty> PropertyDefinition => new List<IStateProperty>(from pair in _properties select pair.Key);

        public StateProperties AsInmmutable => new StateProperties(this);

        public StatePropertyProvider Properties => this;

        protected List<KeyValuePair<IStateProperty, int>> _properties = new List<KeyValuePair<IStateProperty, int>>();

        public int PropertyCount => _properties.Count;


        public StatePropertyProvider()
        {
        }

        public int PackedProperties => CalculatePackedProperties();
        public int PackedBitCount => CalculatePackedBitCount();

        public void Decode(int packed)
        {
            for(int i = 0; i < _properties.Count; i++)
            {
                IStateProperty property = _properties[i].Key;
                _properties[i] = new KeyValuePair<IStateProperty, int>(property, packed & property.BitMask);
                packed >>= property.BitCount;
            }
        }

        private int CalculatePackedProperties()
        {
            int packed = 0;
            int bitCount = 0;
            foreach(var pair in _properties)
            {
                packed |= pair.Value << bitCount;
                bitCount += pair.Key.BitCount;
                if(bitCount > MaxPackedBitCount)
                {
                    throw new Exception("StateProperties out of bound");
                }
            }
            return packed;
        }

        private int CalculatePackedBitCount()
        {
            int bitCount = 0;
            foreach (var pair in _properties)
            {
                bitCount += pair.Key.BitCount;
            }
            return bitCount;
        }

        private int FindProperty(IStateProperty property)
        {
            return _properties.FindIndex((KeyValuePair<IStateProperty, int> pair) => property.Equals(pair.Value));
        }

        public bool ContainsProperty(IStateProperty property)
        {
            return FindProperty(property) != -1;
        }

        public int GetIndex(IStateProperty property)
        {
            int i = FindProperty(property);
            return _properties[i].Value;
        }

        public bool TrySetIndex(IStateProperty property, int index)
        {
            if(!property.IndexIsValid(index))
            {
                return false;
            }
            int i = FindProperty(property);
            if(i != -1)
            {
                _properties[i] = new KeyValuePair<IStateProperty, int>(property, index);
            }
            else
            {
                _properties.Add(new KeyValuePair<IStateProperty, int>(property, index));
            }
            return true;
        }

        public bool TryGetValue<VT>(StateProperty<VT> property, out VT value)
        {
            int index = GetIndex(property);
            if(index != -1)
            {
                value = property.GetValueByIndex(index);
                return true;
            }
            value = default;
            return false;
        }

        public bool TrySetValue<VT>(StateProperty<VT> property, VT value)
        {
            int index = property.GetValueIndex(value);
            return TrySetIndex(property, index);
        }

        public bool TryUpdateIndex(IStateProperty property, int index)
        {
            int i = FindProperty(property);
            if (i != -1 && property.IndexIsValid(index))
            {
                _properties[i] = new KeyValuePair<IStateProperty, int>(property, index);
                return true;
            }
            return false;
        }

        public bool TryUpdateValue<VT>(StateProperty<VT> property, VT value)
        {
            int index = property.GetValueIndex(value);
            return TryUpdateIndex(property, index);
        }

        public bool TryAddIndex(IStateProperty property, int index)
        {
            if(FindProperty(property) == -1 && property.IndexIsValid(index))
            {
                _properties.Add(new KeyValuePair<IStateProperty, int>(property, index));
                return true;
            }
            return false;
        }

        public bool TryAddValue<VT>(StateProperty<VT> property, VT value)
        {
            int index = property.GetValueIndex(value);
            return TryAddIndex(property, index);
        }
    }
}
