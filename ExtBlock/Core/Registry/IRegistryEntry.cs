using System;
using ExtBlock.Core.Tag;
using ExtBlock.Resource;

namespace ExtBlock.Core.Registry
{
    public interface IRegistryEntry
    {
        /// <summary>
        /// 具体类型未知的 RegistryEntry 信息
        /// </summary>
        public RegistryEntryInfo UntypedRegEntryInfo { get; }

        /// <summary>
        /// RegistryEntry的具体类型, 如 Block, 如需允许 override, 需要实现该属性并将其标记为 virtual
        /// </summary>
        public Type AsEntryType => GetType();

        /*
         * 无需 override 的默认实现
         */

        /// <summary>
        /// 该对象注册用的 Key
        /// </summary>
        public ResourceKey Key => UntypedRegEntryInfo.RegKey;

        /// <summary>
        /// 该对象的 字符串id
        /// </summary>
        public ResourceLocation Id => UntypedRegEntryInfo.Id;

        /// <summary>
        /// 该对象的名称
        /// </summary>
        public string Name => UntypedRegEntryInfo.Name;

        /// <summary>
        /// 该对象所属的 modid
        /// </summary>
        public string ModId => UntypedRegEntryInfo.ModId;

        /// <summary>
        /// 该对象的 数字id
        /// </summary>
        public int NumId => UntypedRegEntryInfo.NumId;

        /// <summary>
        /// 该对象的 Registry, 具体类型未知
        /// </summary>
        public Registry UntypedRegistry => UntypedRegEntryInfo.UntypedRegistry;
    }

    /// <summary>
    /// 所有需要被注册到Registry(包含Registry本身)的类应实现该接口, 
    /// 并实现其中的属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRegistryEntry<T> : IRegistryEntry
        where T : class, IRegistryEntry<T>
    {
        /// <summary>
        /// RegistryEntry 的信息, 包括注册名, Registry 实例的引用, Entry 实例的引用等
        /// </summary>
        public RegistryEntryInfo<T> RegEntryInfo { get; set; }

        /*
         * 无需 override 的默认实现
         */

        RegistryEntryInfo IRegistryEntry.UntypedRegEntryInfo => RegEntryInfo;

        public Registry<T> Registry => RegEntryInfo.Registry;

        /*
         * Tag 相关默认实现
         */

        /// <summary>
        /// 该 RegistryEntry 是否拥有某个 Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool ContainsTag(Tag<T> tag)
        {
            return RegEntryInfo.Tags.Contains(tag);
        }
    }
}
