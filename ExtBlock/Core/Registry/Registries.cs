using ExtBlock.Resource;

namespace ExtBlock.Core.Registry
{
    public static class Registries
    {
        /// <summary>
        /// registry of registries(except itself), it has id of -1 and ResourceKey of "extblock:registry/extblock:registry"
        /// </summary>
        public static Registry<Registry> RootRegistry = new Registry<Registry>();

        private static readonly DeferredRegister<Registry> deferredRegister = DeferredRegister<Registry>.Create(ResourceLocation.DEFAULT_NAMESPACE, RootRegistry);

        static Registries()
        {
            // can't register to an unregistered registry, so just directly set RegistryInfo for RootRegistry
            ResourceLocation rootRegistryLocation = ResourceLocation.Create(ResourceLocation.DEFAULT_NAMESPACE, ResourceKey.REGISTRY_PATH);
            ResourceKey rootRegistryKey = ResourceKey.Create(rootRegistryLocation, rootRegistryLocation);
            RegistryInfo<Registry> rootRegistryInfo = new RegistryInfo<Registry>(-1, rootRegistryKey, RootRegistry, RootRegistry);
            IRegistryEntry<Registry> rootRegistry = RootRegistry;
            rootRegistry.SetRegistryInfo(rootRegistryInfo);

            // register other extblock registries
            //Add("block", BlockRegistry);
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

        //public static Registry<Block> BlockRegistry = new Registry<Block>();
    }
}
