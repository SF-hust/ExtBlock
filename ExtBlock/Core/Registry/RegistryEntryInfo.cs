﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using ExtBlock.Resource;
using ExtBlock.Core.Tag;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// 无类型的 RegistryEntry 注册信息
    /// </summary>
    public abstract class RegistryEntryInfo
    {
        protected RegistryEntryInfo(int numId, ResourceKey regKey)
        {
            _numId = numId;
            _regKey = regKey;
        }

        protected int _numId;
        /// <summary>
        /// 此 Entry 在 Registry 中的数字Id, 在同一个 Registry 中唯一
        /// </summary>
        public int NumId => _numId;

        protected ResourceKey _regKey;
        /// <summary>
        /// 此 Entry 的 ResourceKey, 在整个游戏中唯一
        /// </summary>
        public ResourceKey RegKey => _regKey;

        /// <summary>
        /// 此 Entry 的字符串 Id, 即 "namespace:name", 在同一个 Registry 中唯一
        /// </summary>
        public ResourceLocation Id => RegKey.location;

        /// <summary>
        /// 此 Entry 的 modid
        /// </summary>
        public string ModId => Id.namspace;

        /// <summary>
        /// 此 Entry 的名字, 在本模组命名空间下, 同一个 Registry 中唯一
        /// </summary>
        public string Name => RegKey.location.path;

        /// <summary>
        /// 此 Entry 的具体类型
        /// </summary>
        public abstract Type EntryType { get; }

        /// <summary>
        /// 此 Entry 被注册到的 Registry 实例, 其中的 Entry 类型未知
        /// </summary>
        public abstract Registry UntypedRegistry { get; }

        /// <summary>
        /// 实际 RegistryEntry 的引用, 没有具体类型信息
        /// </summary>
        public abstract IRegistryEntry UntypedRegistryEntry { get; }

        /// <summary>
        /// RegistryEntryInfo 的 hashcode 被定义为其 ResourceKey 的 hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _regKey.GetHashCode();
        }

        /// <summary>
        /// RegistryEntryInfo 不会出现两个不同实例的值相同的情况
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }
    }

    /// <summary>
    /// 已被注册到 Registry 中的 Entry 的信息,
    /// 当 RegistryEntry 被添加到 Registry 中时, 会创建对应的此类示例
    /// </summary>
    /// <typeparam name="T"> RegistryEntry 的具体类型 </typeparam>
    public class RegistryEntryInfo<T> : RegistryEntryInfo
        where T : class, IRegistryEntry<T>
    {
        public RegistryEntryInfo(int id, ResourceKey key, Registry<T> registry, T entry) : base(id, key)
        {
            _registry = registry;
            _entry = entry;
        }

        public override Type EntryType => typeof(T);

        public override Registry UntypedRegistry => Registry;

        public override IRegistryEntry UntypedRegistryEntry => Entry;

        /// <summary>
        /// 此 Entry 被注册到的 Registry
        /// </summary>
        public Registry<T> Registry => _registry;
        protected Registry<T> _registry;

        /// <summary>
        /// 实际 RegistryEntry 对象的引用
        /// </summary>
        public T Entry => _entry;
        protected T _entry;

        public override string ToString()
        {
            return $"RegistryEntry (type = {EntryType}, registry id = {Registry.RegEntryInfo.Id}, num id = {NumId}, id = {Id})";
        }

        /*
         * Tag 相关
         */

        /// <summary>
        /// RegistryEntry 的所有 Tag
        /// </summary>
        public ImmutableHashSet<Tag<T>> Tags => _tags;
        private ImmutableHashSet<Tag<T>> _tags = ImmutableHashSet<Tag<T>>.Empty;

        /// <summary>
        /// 重新设置本 Entry 的 Tag, 这会发生在数据包加载时
        /// </summary>
        /// <param name="tags"></param>
        public void ResetTags(IEnumerable<Tag<T>> tags)
        {
            _tags = tags.ToImmutableHashSet();
        }
    }
}
