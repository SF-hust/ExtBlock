using System;
using System.Collections.Generic;
using System.Text;
using ExtBlock.Core.Component;
using ExtBlock.Core.Registry;

namespace ExtBlock.Game
{
    public class Item : IRegistryEntry<Item>,
        IComponentHolder
    {
        public Item()
        {
            _components = new ComponentHolder(this);
        }

        /*
         * 作为 RegistryEntry
         */

        private RegistryEntryInfo<Item>? _regInfo;
        public RegistryEntryInfo<Item> RegEntryInfo { get => _regInfo!; set => _regInfo ??= value; }
        public virtual Type AsEntryType => typeof(Item);

        /*
         * 作为 ComponentHolder
         */

        public ComponentHolder Components => _components;
        private readonly ComponentHolder _components;

        /*
         * Item 的内置组件
         */
         
    }
}
