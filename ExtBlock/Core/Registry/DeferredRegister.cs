using System;
using System.Collections.Generic;

using ExtBlock.Resource;
using ExtBlock.Utility.Logger;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// 用于管理游戏对象注册, 会执行真正的注册动作
    /// </summary>
    /// <typeparam name="T">class of registry entry</typeparam>
    public class DeferredRegister<T>
        where T : class, IRegistryEntry<T>
    {
        /// <summary>
        /// 创建一个指定 modid 的 DeferredRegister, 后续使用 Register() 注册时会默认使用这个 modid,
        /// 如果想向其中注册一个使用其他 modid 的 Entry, 使用 RegisterCustom()
        /// </summary>
        /// <param name="modid">your modid</param>
        /// <returns></returns>
        public static DeferredRegister<T> Create(Registry<T> registry, string modid)
        {
            return new DeferredRegister<T>(modid, registry);
        }

        private readonly string _modid;
        private readonly Registry<T> _registry;
        private readonly Dictionary<ResourceLocation, T> _entries = new Dictionary<ResourceLocation, T>();

        private DeferredRegister(string modid, Registry<T> registry)
        {
            _modid = modid;
            _registry = registry;
            registry.OnRegisterEvent += DoRegister;
        }

        /// <summary>
        /// 向 DeferredRegister 中添加一个新的 Entry
        /// </summary>
        /// <param name="name">name of registry entry</param>
        /// <param name="entry">entry to add</param>
        public void Register(string name, T entry)
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
        /// 向 DeferredRegister 中添加一个新的 Entry, 但自行指定 modid
        /// </summary>
        /// <param name="location">(modid, name) of registry entry</param>
        /// <param name="entry">entry to add</param>
        public void RegisterCustom(string modid, string name, T entry)
        {
            if (_entries.ContainsValue(entry))
            {
                throw new InvalidOperationException("can't add same entry to DeferredRegister");
            }
            if (!_entries.TryAdd(ResourceLocation.Create(modid, name), entry))
            {
                throw new InvalidOperationException("can't add entries with same name to one DeferredRegister");
            }
        }

        /// <summary>
        /// 执行注册操作, 当 Registry<ET>.OnRegisterEvent 事件触发时被调用
        /// </summary>
        /// <param name="sender">no use</param>
        /// <param name="args">no use</param>
        private void DoRegister(object sender, Registry<T>.RegisterEventArgs args)
        {
            LogUtil.Logger.Info($"DeferredRegister capture a register event:\n" +
                $"modid = ({_modid}), registry = ({_registry.RegEntryInfo.Id}),\n" +
                "entries = {");
            foreach (var pair in _entries)
            {
                LogUtil.Logger.Info(pair.Key.ToString());
            }
            LogUtil.Logger.Info("}");

            foreach (var pair in _entries)
            {
                ResourceLocation location = pair.Key;
                T entry = pair.Value;
                _registry.Add(entry, location);
            }
        }
    }
}
