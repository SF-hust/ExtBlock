﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ExtBlock.Core.State;

namespace ExtBlock.Core.Property
{
    public class PropertyProvider : IPropertyProvider
    {
        protected Dictionary<IProperty, object> _properties = new Dictionary<IProperty, object>();
        public PropertyProvider Properties => this;

        public PropertyProvider()
        {
        }

        public bool Contains(IProperty property)
        {
            return _properties.ContainsKey(property);
        }

        public bool TryGet(IProperty property, [NotNullWhen(true)] out object? value)
        {
            if (_properties.TryGetValue(property, out object? propertyValue) && property.ValueIsValid(propertyValue))
            {
                value = propertyValue;
                return true;
            }
            value = default;
            return false;
        }
        public bool TryGet<VT>(Property<VT> property, [NotNullWhen(true)] out VT value)
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

        public bool Set(IProperty property, object value)
        {
            if (property.ValueIsValid(value))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }
        public bool Set<VT>(Property<VT> property, VT value)
            where VT : notnull
        {
            if (property.ValueIsValid(value))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }

        public bool TryUpdate(IProperty property, object value)
        {
            if (property.ValueIsValid(value) && _properties.ContainsKey(property))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }
        public bool TryUpdate<VT>(Property<VT> property, VT value)
            where VT : notnull
        {
            if (property.ValueIsValid(value) && _properties.ContainsKey(property))
            {
                _properties[property] = value;
                return true;
            }
            return false;
        }

        public bool TryAdd(IProperty property, object value)
        {
            if (property.ValueIsValid(value) && _properties.TryAdd(property, value))
            {
                return true;
            }
            return false;
        }
        public bool TryAdd<VT>(Property<VT> property, VT value)
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
