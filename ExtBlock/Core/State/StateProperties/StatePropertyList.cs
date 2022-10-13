namespace ExtBlock.Core.State
{
    /// <summary>
    /// warp of MutableStatePropertyList.
    /// contains a list of, readonly pair of StateProperty and its value's index
    /// </summary>
    public class StatePropertyList
    {
        private MutableStatePropertyList _properties;

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

        public StatePropertyList(MutableStatePropertyList properties)
        {
            _properties = properties;
            _packedProperties = properties.PackedProperties;
            _packedBitCount = properties.PackedBitCount;
        }

        /// <summary>
        /// get value index of a StateProperty
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int this[IStateProperty property]
        {
            get => _properties.Get(property);
        }

        /// <summary>
        /// wether this list contains a property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool ContainsProperty(IStateProperty property)
        {
            return _properties.ContainsProperty(property);
        }

        /// <summary>
        /// get index of StateProperty in this list
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int GetStatePropertyIndex(IStateProperty property)
        {
            return _properties.Get(property);
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
            return _properties.TryGetValue(property, out value);
        }
    }
}
