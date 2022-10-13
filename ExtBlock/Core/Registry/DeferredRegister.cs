using System;
using System.Collections.Generic;

using ExtBlock.Resource;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// a register helper class, you can add entries to a DeferredRegister<>,
    /// and it will do actul register operations in proper time.
    /// </summary>
    /// <example>
    /// <code>
    /// DeferredRegister<Block> BlockRegister = DeferredRegister<Block>.Create("examplemod");
    /// Block ExampleBlock = new Block();
    /// BlockRegister.Register("example_block", ExampleBlock);
    /// </code>
    /// </example>
    /// <typeparam name="ET">class of registry entry</typeparam>
    public class DeferredRegister<ET>
        where ET : class, IRegistryEntry<ET>
    {
        /// <summary>
        /// create a DeferredRegister with your modid
        /// </summary>
        /// <param name="modid">your modid</param>
        /// <returns></returns>
        public static DeferredRegister<ET> Create(string modid, Registry<ET> registry)
        {
            return new DeferredRegister<ET>(modid, registry);
        }

        private readonly string _modid;
        private readonly Registry<ET> _registry;
        private readonly Dictionary<ResourceLocation, ET> _entries = new Dictionary<ResourceLocation, ET>();

        private DeferredRegister(string modid, Registry<ET> registry)
        {
            _modid = modid;
            _registry = registry;
            registry.OnRegisterEvent += DoRegister;
        }

        /// <summary>
        /// add registry entries for register later
        /// </summary>
        /// <param name="name">name of registry entry</param>
        /// <param name="entry">entry to add</param>
        public void Register(string name, ET entry)
        {
            if(_entries.ContainsValue(entry))
            {
                throw new InvalidOperationException("can't add same entry to DeferredRegister");
            }
            if (!_entries.TryAdd(ResourceLocation.Create(_modid, name), entry))
            {
                throw new InvalidOperationException("can't add entries with same name to one DeferredRegister");
            }
        }

        /// <summary>
        /// add registry entries in specific modid for register later, normally you should use Regiser() instead
        /// </summary>
        /// <param name="location">(modid, name) of registry entry</param>
        /// <param name="entry">entry to add</param>
        public void RegisterCustom(ResourceLocation location, ET entry)
        {
            if (_entries.ContainsValue(entry))
            {
                throw new InvalidOperationException("can't add same entry to DeferredRegister");
            }
            if (!_entries.TryAdd(location, entry))
            {
                throw new InvalidOperationException("can't add entries with same name to one DeferredRegister");
            }
        }

        /// <summary>
        /// do actul register opration when _registry.OnRegisterEvent fired
        /// </summary>
        /// <param name="sender">no use</param>
        /// <param name="args">no use</param>
        private void DoRegister(object sender, Registry<ET>.RegisterEventArgs args)
        {
            foreach(var pair in _entries)
            {
                ResourceLocation location = pair.Key;
                ET entry = pair.Value;
                _registry.Add(entry, location);
            }
        }
    }
}
