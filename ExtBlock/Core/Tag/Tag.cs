using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using ExtBlock.Core.Registry;
using ExtBlock.Resource;

namespace ExtBlock.Core.Tag
{
    /*
     * 标签
     */

    public abstract class Tag
    {
        public Tag(ResourceLocation id)
        {
            this.id = id;
        }

        /// <summary>
        /// Tag 的字符串id
        /// </summary>
        public readonly ResourceLocation id;

        public abstract IEnumerable<ResourceLocation> ElementIds { get; }

        public override string ToString()
        {
            return id.ToString();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tag<T> : Tag
        where T : class, IRegistryEntry<T>
    {
        /// <summary>
        /// 用于此 Tag 的对象
        /// </summary>
        public readonly ImmutableArray<T> elements;

        public Tag(ResourceLocation id, IEnumerable<T> values) : base(id)
        {
            elements = values.ToImmutableArray();
        }

        public override IEnumerable<ResourceLocation> ElementIds => from element in elements select element.Id;

        public override int GetHashCode()
        {
            return id.GetHashCode() + 31 * typeof(T).GetHashCode();
        }
    }
}
