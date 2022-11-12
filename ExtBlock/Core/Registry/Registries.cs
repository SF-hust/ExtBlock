using ExtBlock.Game;
using ExtBlock.Resource;
using ExtBlock.Utility;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// 静态类, 其中包含了所有 CoreMod 使用到的 Registry
    /// </summary>
    public static class Registries
    {
        /// <summary>
        /// 所有 Registry 的 Registry, 注意 RootRegistry 不会被注册到自身, RootRegistry 的 ResourceKey 为"extblock:registry/extblock:registry", 数字id 为 -1
        /// </summary>
        public static readonly Registry<Registry> RootRegistry = new Registry<Registry>();

        private static readonly DeferredRegister<Registry> deferredRegistryRegister = DeferredRegister<Registry>.Create(RootRegistry, Constants.DEFAULT_NAMESPACE);

        /// <summary>
        /// 初始化内部 Registry(即 RootRegistry 和 CoreMod 用到的 Registry)
        /// </summary>
        public static void InitInternalRegistries()
        {
            // 由于不能向一个没有注册的 Registry 中添加 Entry, 因此要手动设置 RootRegistry 的注册信息
            ResourceKey rootRegistryKey = ResourceKey.Create(Constants.REGISTRY_LOCATION, Constants.REGISTRY_LOCATION);
            RegistryEntryInfo<Registry> rootRegistryInfo = new RegistryEntryInfo<Registry>(-1, rootRegistryKey, RootRegistry, RootRegistry);
            IRegistryEntry<Registry> rootRegistry = RootRegistry;
            rootRegistry.RegEntryInfo = rootRegistryInfo;

            // 注册 CoreMod 中用到的 Registry
            Add("block", BlockRegistry);
        }

        /// <summary>
        /// 触发 RootRegistry 的注册事件
        /// </summary>
        public static void FireRootRegisterEvent()
        {
            RootRegistry.FireRegisterEvent();
        }

        /// <summary>
        /// 触发所有被注册到 RootRegistry 的 Registry 的注册事件
        /// </summary>
        public static void FireRegisterEvents()
        {
            foreach(var registry in RootRegistry.Entries)
            {
                registry.RegEntryInfo.Entry.FireRegisterEvent();
            }
        }

        /// <summary>
        /// 向 RootRegistry 的 DeferredRegister 中添加以 extblock 为 modid 的 Registry
        /// </summary>
        /// <param name="name">name of new Registry</param>
        /// <param name="registry">the registry instance</param>
        public static void Add(string name, Registry registry)
        {
            deferredRegistryRegister.Register(name, registry);
        }

        /// <summary>
        /// 向 RootRegistry 的 DeferredRegister 中添加 mod 自定义的 Registry
        /// </summary>
        /// <param name="modid"></param>
        /// <param name="name"></param>
        /// <param name="registry"></param>
        public static void AddCustom(string modid, string name, Registry registry)
        {
            deferredRegistryRegister.RegisterCustom(modid, name, registry);
        }

        /*
         * CoreMod 使用的所有 Registry
         */

        public static Registry<Block> BlockRegistry = new Registry<Block>();

        public static Registry<Item> ItemRegistry = new Registry<Item>();
    }
}
