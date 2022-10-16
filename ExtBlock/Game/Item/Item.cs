using System;
using System.Collections.Generic;
using System.Text;

using ExtBlock.Core.Registry;

namespace ExtBlock.Game
{
    public class Item : IRegistryEntry<Item>
    {
        // registry entry
        private RegistryInfo<Item>? _regInfo;
        public RegistryInfo<Item> RegInfo { get => _regInfo!; set => _regInfo ??= value; }
        public RegistryInfo UntypedRegInfo => _regInfo!;

        // property
        private readonly ItemProperty _properties;
        public ItemProperty Properties => _properties;

        public Item(ItemProperty properties)
        {
            _properties = properties;
        }
    }
}
