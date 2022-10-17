using System;
using System.Collections.Generic;
using System.Text;

using ExtBlock.Core.Registry;

namespace ExtBlock.Game
{
    public class Item : IRegistryEntry<Item>
    {
        // registry entry
        private RegistryEntryInfo<Item>? _regInfo;
        public RegistryEntryInfo<Item> RegEntryInfo { get => _regInfo!; set => _regInfo ??= value; }
        public RegistryEntryInfo UntypedRegInfo => _regInfo!;

        // property
        private readonly ItemProperty _properties;
        public ItemProperty Properties => _properties;

        public Item(ItemProperty properties)
        {
            _properties = properties;
        }
    }
}
