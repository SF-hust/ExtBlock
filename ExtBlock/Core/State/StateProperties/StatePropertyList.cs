using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtBlock.Core.State
{
    public class StatePropertyList : IEnumerable<KeyValuePair<StateProperty, int>>
    {
        public const int MaxPackedBitCount = 16;

        /// <summary>
        /// 获取一个 StateProperty 在此列表中的值的下标, 若不存在给定的 StateProperty 则抛出 IndexOutOfRangeException
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public int this[StateProperty property]
        {
            get => Get(property);
            set
            {
                if(!TrySet(property, value))
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 根据下标返回 StateProperty 及其值的下标
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public KeyValuePair<StateProperty, int> this[int index] => _propertyIndexPairs[index];

        public IEnumerator<KeyValuePair<StateProperty, int>> GetEnumerator()
        {
            return _propertyIndexPairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _propertyIndexPairs.GetEnumerator();
        }

        /// <summary>
        /// 获取存储的 StateProperty 列表
        /// </summary>
        public List<StateProperty> Properties => new List<StateProperty>(from pair in _propertyIndexPairs select pair.Key);

        /// <summary>
        /// 获取自身的 Immutable 包装
        /// </summary>
        public ImmutableStatePropertyList AsImmutable => new ImmutableStatePropertyList(this);

        protected List<KeyValuePair<StateProperty, int>> _propertyIndexPairs = new List<KeyValuePair<StateProperty, int>>();

        /// <summary>
        /// 获取已有的 StateProperty 数量
        /// </summary>
        public int PropertyCount => _propertyIndexPairs.Count;


        public StatePropertyList()
        {
        }

        /// <summary>
        /// 获取所有属性值打包成的整数
        /// </summary>
        public int PackedProperties => CalculatePackedProperties();

        /// <summary>
        /// 获取属性值打包完成后所需占用的二进制位数
        /// </summary>
        public int PackedBitCount => CalculatePackedBitCount();

        /// <summary>
        /// 根据现有属性列表解码打包后的整数
        /// </summary>
        /// <param name="packed"></param>
        public void Decode(int packed)
        {
            for(int i = 0; i < _propertyIndexPairs.Count; i++)
            {
                StateProperty property = _propertyIndexPairs[i].Key;
                _propertyIndexPairs[i] = new KeyValuePair<StateProperty, int>(property, packed & property.BitMask);
                packed >>= property.BitCount;
            }
        }

        private int CalculatePackedProperties()
        {
            int packed = 0;
            int bitCount = 0;
            foreach(var pair in _propertyIndexPairs)
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
            foreach (var pair in _propertyIndexPairs)
            {
                bitCount += pair.Key.BitCount;
            }
            return bitCount;
        }

        public int FindProperty(StateProperty property)
        {
            return _propertyIndexPairs.FindIndex((KeyValuePair<StateProperty, int> pair) => property.Equals(pair.Value));
        }

        public bool ContainsProperty(StateProperty property)
        {
            return FindProperty(property) != -1;
        }

        public int Get(StateProperty property)
        {
            int i = FindProperty(property);
            return _propertyIndexPairs[i].Value;
        }

        public bool TrySet(StateProperty property, int index)
        {
            if(!property.IndexIsValid(index))
            {
                return false;
            }
            int i = FindProperty(property);
            if(i != -1)
            {
                _propertyIndexPairs[i] = new KeyValuePair<StateProperty, int>(property, index);
            }
            else
            {
                _propertyIndexPairs.Add(new KeyValuePair<StateProperty, int>(property, index));
            }
            return true;
        }

        public bool TryUpdate(StateProperty property, int index)
        {
            int i = FindProperty(property);
            if (i != -1 && property.IndexIsValid(index))
            {
                _propertyIndexPairs[i] = new KeyValuePair<StateProperty, int>(property, index);
                return true;
            }
            return false;
        }

        public bool TryAdd(StateProperty property, int index)
        {
            if(FindProperty(property) == -1 && property.IndexIsValid(index))
            {
                _propertyIndexPairs.Add(new KeyValuePair<StateProperty, int>(property, index));
                return true;
            }
            return false;
        }

        public bool TryGetValue<V>(StateProperty<V> property, out V value)
        {
            int index = Get(property);
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
            int index = property.GetIndexByValue(value);
            return TrySet(property, index);
        }

        public bool TryUpdateValue<VT>(StateProperty<VT> property, VT value)
        {
            int index = property.GetIndexByValue(value);
            return TryUpdate(property, index);
        }

        public bool TryAddValue<VT>(StateProperty<VT> property, VT value)
        {
            int index = property.GetIndexByValue(value);
            return TryAdd(property, index);
        }
    }
}
