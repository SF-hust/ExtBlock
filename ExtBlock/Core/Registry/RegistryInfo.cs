﻿using System;
using System.Diagnostics;
using ExtBlock.Resource;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// Untyped registry entry infomation
    /// </summary>
    public abstract class RegistryInfo
    {
        protected RegistryInfo(int id, ResourceKey key)
        {
            _id = id;
            _key = key;
        }

        protected int _id;
        /// <summary>
        /// an integer id, for fast access
        /// </summary>
        public int Id => _id;

        protected ResourceKey _key;
        /// <summary>
        /// (registry modid, registry name, modid, name) of this registry entry
        /// </summary>
        public ResourceKey Key => _key;

        /// <summary>
        /// (modid, name) of this registry entry
        /// </summary>
        public ResourceLocation Location => Key.Location;

        /// <summary>
        /// name of this registry entry
        /// </summary>
        public string Path => Key.Location.Path;

        /// <summary>
        /// typeof the registry entry class
        /// </summary>
        public abstract Type EntryType { get; }

        /// <summary>
        /// registry instance as untyped
        /// </summary>
        public abstract Registry UntypedReg { get; }

        /// <summary>
        /// registry entry instance as IRegistryEntry
        /// </summary>
        public abstract IRegistryEntry UntypedEntry { get; }
    }

    /// <summary>
    /// registry entry infomation created and saved by registry
    /// </summary>
    /// <typeparam name="ET">class of registry entry</typeparam>
    public class RegistryInfo<ET> : RegistryInfo
        where ET : class, IRegistryEntry<ET>
    {
        public RegistryInfo(int id, ResourceKey key, Registry<ET> registry, ET entry) : base(id, key)
        {
            _reg = registry;
            _entry = entry;
        }

        public override Type EntryType => typeof(ET);

        public override Registry UntypedReg => Reg;

        public override IRegistryEntry UntypedEntry => Entry;

        protected Registry<ET> _reg;
        /// <summary>
        /// typed registry instance
        /// </summary>
        public Registry<ET> Reg => _reg;

        protected ET _entry;
        /// <summary>
        /// typed registry entry instance
        /// </summary>
        public ET Entry => _entry;

        public override string ToString()
        {
            return $"RegistryEntry (Type = {EntryType}, Registry = {((IRegistryEntry<Registry>)Reg).RegInfo.Location}, id = {Id}, Location = {Location})";
        }
    }
}
