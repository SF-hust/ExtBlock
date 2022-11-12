using System;
using System.Collections.Generic;
using System.Text;

using ExtBlock.Core.Registry;

namespace ExtBlock.Game.Entity
{
    public abstract class EntityType : IRegistryEntry<EntityType>
    {
        /*
         * 作为 RegistryEntry
         */

        public RegistryEntryInfo<EntityType> RegEntryInfo { get => _regInfo!; set => _regInfo ??= value; }
        private RegistryEntryInfo<EntityType>? _regInfo;
        public virtual Type AsEntryType => typeof(EntityType);
    }

    public class EntityType<T>
        where T : Entity
    {

    }
}
