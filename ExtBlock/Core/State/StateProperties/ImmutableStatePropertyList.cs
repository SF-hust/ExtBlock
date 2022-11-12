namespace ExtBlock.Core.State
{
    /// <summary>
    /// 对 StatePropertyList 的不可变包装
    /// </summary>
    public class ImmutableStatePropertyList
    {
        private readonly StatePropertyList _propertyList;

        private readonly int _packedProperties;
        private readonly int _packedBitCount;

        /// <summary>
        /// packs all StateProperty's value into one integer
        /// </summary>
        public int PackedIndices => _packedProperties;

        /// <summary>
        /// bit count PackedIndices would use
        /// </summary>
        public int PackedBitCount => _packedBitCount;

        public ImmutableStatePropertyList(StatePropertyList propertyList)
        {
            _propertyList = propertyList;
            _packedProperties = propertyList.PackedProperties;
            _packedBitCount = propertyList.PackedBitCount;
        }

        /// <summary>
        /// get value index of a StateProperty
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int this[StateProperty property]
        {
            get => _propertyList.Get(property);
        }

        /// <summary>
        /// wether this list contains a property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool ContainsProperty(StateProperty property)
        {
            return _propertyList.ContainsProperty(property);
        }

        /// <summary>
        /// get index of StateProperty in this list
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int GetStatePropertyIndex(StateProperty property)
        {
            return _propertyList.Get(property);
        }

        /// <summary>
        /// get typed value by StateProperty
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue<VT>(StateProperty<VT> property, out VT value)
        {
            return _propertyList.TryGetValue(property, out value);
        }
    }
}
