﻿using ExtBlock.Game;
using ExtBlock.Resource;

namespace ExtBlock.Core.Registry
{
    public static class Registries
    {
        /// <summary>
        /// registry of registries(except itself), it has id of -1 and ResourceKey of "extblock:registry/extblock:registry"
        /// </summary>
        public static readonly Registry<Registry> RootRegistry = new Registry<Registry>();

        private static readonly DeferredRegister<Registry> deferredRegister = DeferredRegister<Registry>.Create(ResourceLocation.DEFAULT_NAMESPACE, RootRegistry);

        public static void InitInternalRegistries()
        {
            // can't register to an unregistered registry, so just directly set RegistryInfo for RootRegistry
            ResourceKey rootRegistryKey = ResourceKey.Create(ResourceKey.REGISTRY, ResourceKey.REGISTRY);
            RegistryInfo<Registry> rootRegistryInfo = new RegistryInfo<Registry>(-1, rootRegistryKey, RootRegistry, RootRegistry);
            IRegistryEntry<Registry> rootRegistry = RootRegistry;
            rootRegistry.RegInfo = rootRegistryInfo;

            // register other extblock registries
            Add("block", BlockRegistry);
        }

        public static void FireRootRegisterEvent()
        {
            RootRegistry.FireRegisterEvent();
        }

        public static void FireRegisterEvents()
        {
            foreach(var registry in RootRegistry.Entries)
            {
                registry.RegInfo.Entry.FireRegisterEvent();
            }
        }

        /// <summary>
        /// add new extblock Registry to Registries
        /// </summary>
        /// <param name="name">name of new Registry</param>
        /// <param name="registry">the registry instance</param>
        public static void Add(string name, Registry registry)
        {
            deferredRegister.Register(name, registry);
        }

        /// <summary>
        /// add new custom Registry to Registries
        /// </summary>
        /// <param name="modid">your modid</param>
        /// <param name="name">name of new Registry</param>
        /// <param name="registry">the registry instance</param>
        public static void AddCustom(string modid, string name, Registry registry)
        {
            deferredRegister.RegisterCustom(ResourceLocation.Create(modid, name), registry);
        }

        /*
         * extblock Registries
         */

        public static Registry<Block> BlockRegistry = new Registry<Block>();
    }
}
