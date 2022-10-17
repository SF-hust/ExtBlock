using System.Collections.Generic;
using System.Collections.Immutable;

using ExtBlock.Core.Registry;
using ExtBlock.Resource;

namespace ExtBlock.Core.Tag
{
    public partial class Tag<T>
        where T : class, IRegistryEntry<T>
    {
        public readonly ResourceLocation id;

        public readonly ImmutableList<T> elements;

        private Tag(ResourceLocation id, IEnumerable<T> values)
        {
            this.id = id;
            elements = values.ToImmutableList();
        }
    }
}
