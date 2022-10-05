using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ExtBlock.Core.Property
{
    public class StatePropertyProvider : IStatePropertyProvider
    {
        protected Dictionary<IStateProperty, object> _properties = new Dictionary<IStateProperty, object>();

        public StatePropertyProvider Properties => this;

        public StatePropertyProvider()
        {
        }

        public bool Contains(IStateProperty property)
        {
            return _properties.ContainsKey(property);
        }

        public bool TryGet(IStateProperty property, [NotNullWhen(true)] out object? value)
        {
            if (_properties.TryGetValue(property, out object? propertyValue) && property.ValueIsValid(propertyValue))
            {
                value = propertyValue;
                return true;
            }
            value = default;
            return false;
        }
        public bool TryGet<VT>(StateProperty<VT> property, [NotNullWhen(true)] out VT value)
            where VT : notnull
        {
            if (_properties.TryGetValue(property, out object? propertyValue) && propertyValue is VT v)
            {
                value = v;
                return true;
            }
            value = default;
            return false;
        }

        public bool Set(IStateProperty property, object value)
        {
            if (property.ValueIsValid(value))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }
        public bool Set<VT>(StateProperty<VT> property, VT value)
            where VT : notnull
        {
            if (property.ValueIsValid(value))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }

        public bool TryUpdate(IStateProperty property, object value)
        {
            if (property.ValueIsValid(value) && _properties.ContainsKey(property))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }
        public bool TryUpdate<VT>(StateProperty<VT> property, VT value)
            where VT : notnull
        {
            if (property.ValueIsValid(value) && _properties.ContainsKey(property))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }

        public bool TryAdd(IStateProperty property, object value)
        {
            if (property.ValueIsValid(value) && _properties.TryAdd(property, value))
            {
                return true;
            }
            return false;
        }

        public bool TryAdd<VT>(StateProperty<VT> property, VT value)
            where VT : notnull
        {
            if (property.ValueIsValid(value) && _properties.TryAdd(property, value))
            {
                return true;
            }
            return false;
        }
    }
}
