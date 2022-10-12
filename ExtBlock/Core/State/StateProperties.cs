namespace ExtBlock.Core.State
{
    public class StateProperties
    {
        private StatePropertyProvider _properties;

        private readonly int _packedProperties;
        private readonly int _packedBitCount;

        public int PackedProperties => _packedProperties;
        public int PackedBitCount => _packedBitCount;


        public StateProperties(StatePropertyProvider properties)
        {
            _properties = properties;
            _packedProperties = properties.PackedProperties;
            _packedBitCount = properties.PackedBitCount;
        }

        public int this[IStateProperty property]
        {
            get => _properties.GetIndex(property);
        }

        public bool ContainsProperty(IStateProperty property)
        {
            return _properties.ContainsProperty(property);
        }

        public int GetIndex(IStateProperty property)
        {
            return _properties.GetIndex(property);
        }

        public bool TryGetValue<VT>(StateProperty<VT> property, out VT value)
        {
            return _properties.TryGetValue(property, out value);
        }
    }
}
