using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using ExtBlock.Core.Tag;
using ExtBlock.Resource;
using ExtBlock.Utility.Logger;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// Registry 是一个注册表, 可以向其注册游戏对象(此操作一般由 DefferedRegister 完成), 或查找已注册的游戏对象
    /// </summary>
    public abstract class Registry : IRegistryEntry<Registry>
    {
        /*
         * Registry 本身也是一个 RegistryEntry, 所以需要实现这些属性
         */

        public RegistryEntryInfo<Registry> RegEntryInfo { get => _regInfo!; set => _regInfo ??= value; }
        private RegistryEntryInfo<Registry>? _regInfo;
        public Type AsEntryType => typeof(Registry);

        /*
         * 作为注册表
         */

        /// <summary>
        /// 此 Registry 内注册的 Entry 的具体类型
        /// </summary>
        public abstract Type EntryType { get; }

        /// <summary>
        /// 已被添加进此 Registry 中的 Entry 集合, 类型未知
        /// </summary>
        public abstract IEnumerable<IRegistryEntry> UntypedEntries { get; }


        /// <summary>
        /// 通过 数字id 获取某个 RegistryEntry, 类型未知
        /// </summary>
        /// <param name="numId">RegistryEntry 的 数字id</param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract bool TryGetUntypedEntryByNumId(int numId, out IRegistryEntry? entry);

        /// <summary>
        /// 通过 字符串id 获取某个 RegistryEntry, 类型未知
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract bool TryGetUntypedEntryById(ResourceLocation id, out IRegistryEntry? entry);

        /// <summary>
        /// 使用 字符串id 获取对应的 数字id
        /// </summary>
        /// <param name="location"></param>
        /// <returns>若对应的 字符串id 不存在则返回 -1</returns>
        public abstract int GetNumIdById(ResourceLocation location);

        /// <summary>
        /// 触发注册事件, Modder 不应使用该方法
        /// </summary>
        public abstract bool FireRegisterEvent();
    }

    /// <summary>
    /// 注册表, 保存了一系列的 RegistryEntry
    /// </summary>
    /// <typeparam name="T">RegistryEntry 的实际类型</typeparam>
    public class Registry<T> : Registry, IRegistryEntry<Registry>
        where T : class, IRegistryEntry<T>
    {
        public override Type EntryType => typeof(T);

        protected List<T> _entries = new List<T>();
        /// <summary>
        /// 已被添加进此 Registry 中的 Entry 集合
        /// </summary>
        public IEnumerable<IRegistryEntry<T>> Entries => _entries;
        public override IEnumerable<IRegistryEntry> UntypedEntries => _entries;

        protected Dictionary<ResourceLocation, int> _numIdByStringId = new Dictionary<ResourceLocation, int>();

        public override bool TryGetUntypedEntryByNumId(int numId, out IRegistryEntry? entry)
        {
            bool ret = TryGetEntryByNumId(numId, out T? e);
            entry = e;
            return ret;
        }

        public override bool TryGetUntypedEntryById(ResourceLocation id, out IRegistryEntry? entry)
        {
            bool ret = TryGetEntryById(id, out T? e);
            entry = e;
            return ret;
        }

        /// <summary>
        /// 通过 数字id 获取某个 RegistryEntry
        /// </summary>
        /// <param name="numId">entry id</param>
        /// <param name="entry">entry instance</param>
        /// <returns>true if entry of id exists, or else false</returns>
        public bool TryGetEntryByNumId(int numId, out T? entry)
        {
            if (numId < _entries.Count)
            {
                entry = _entries[numId];
                return true;
            }
            entry = null;
            return false;
        }

        /// <summary>
        /// 通过 字符串id 获取某个 RegistryEntry
        /// </summary>
        /// <param name="id">entry's (modid, name)</param>
        /// <param name="entry">typed entry instance</param>
        /// <returns>true if entry of location exists, or else false</returns>
        public bool TryGetEntryById(ResourceLocation id, [NotNullWhen(true)] out T? entry)
        {
            if (_numIdByStringId.TryGetValue(id, out int i))
            {
                entry = _entries[i];
                return true;
            }
            entry = null;
            return false;
        }

        public override int GetNumIdById(ResourceLocation location)
        {
            if (_numIdByStringId.TryGetValue(location, out int id))
            {
                return id;
            }
            return -1;
        }

        private bool _isLocked = false;
        /// <summary>
        /// Registry 是否已锁定, 已锁定的 Registry 不能被添加新 Entry, 注册事件完成后, Registry 会被锁定
        /// </summary>
        public bool IsLocked => _isLocked;

        /// <summary>
        /// 锁定此 Registry, Modder不应调用这个方法
        /// </summary>
        public void LockRegistry()
        {
            _isLocked = true;
        }

        /// <summary>
        /// 向 Registry 中添加新的 Entry
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool Add(T entry, ResourceLocation location)
        {
            if(IsLocked)
            {
                return false;
            }
            int id = _entries.Count;
            _entries.Add(entry);
            ResourceLocation registryLocation = RegEntryInfo.Id;
            RegistryEntryInfo<T> info = new RegistryEntryInfo<T>(id, ResourceKey.Create(registryLocation, location), this, entry);
            entry.RegEntryInfo = info;
            LogUtil.Logger.Info($"a new entry added to registry ({RegEntryInfo.Id}) :\n" +
                $"id = {id}, location = {location}");
            return true;
        }

        /// <summary>
        /// Registry 事件的参数
        /// </summary>
        public class RegisterEventArgs : EventArgs
        {
            /// <summary>
            /// 空的 Registry 事件参数
            /// </summary>
            public static new RegisterEventArgs Empty = new RegisterEventArgs(string.Empty);

            public readonly string modid;

            public bool IsRegisterComplete = true;

            private RegisterEventArgs(string modid) : base()
            {
                this.modid = modid;
            }
        }

        /// <summary>
        /// 注册事件, 注册游戏对象时该事件会被触发, 事件的触发者将是这个 Registry 自身
        /// </summary>
        public event EventHandler<RegisterEventArgs>? OnRegisterEvent;

        public override bool FireRegisterEvent()
        {
            OnRegisterEvent?.Invoke(this, RegisterEventArgs.Empty);
            return RegisterEventArgs.Empty.IsRegisterComplete;
        }

        /// <summary>
        /// 获取指定 id 的 Entry 的 RegistryObject 包装, 这个方法可以在静态成员的初始化中调用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RegistryObject<T> GetAsRegistryObject(ResourceLocation id)
        {
            return new RegistryObject<T>(() =>
            {
                if(TryGetEntryById(id, out var entry))
                {
                    return entry.RegEntryInfo.Entry;
                }
                return null;
            });
        }

        /*
         * Tag 相关
         */

        /// <summary>
        /// Registry 里的所有 Entry 含有的所有 Tag
        /// </summary>
        public ImmutableHashSet<Tag<T>> EntriesTags => _entriesTags;
        private ImmutableHashSet<Tag<T>> _entriesTags = ImmutableHashSet<Tag<T>>.Empty;

        /// <summary>
        /// 重新设置 Registry 下所有 Entry 的 Tag
        /// </summary>
        /// <param name="tags"></param>
        public void ResetEntriesTags(IEnumerable<Tag<T>> tags)
        {
            _entriesTags = tags.ToImmutableHashSet();
            // 注意 RegistryEntry 本身是不能
            Dictionary<RegistryEntryInfo<T>, List<Tag<T>>> tagsByEntry = new Dictionary<RegistryEntryInfo<T>, List<Tag<T>>>();
            foreach(var tag in tags)
            {
                foreach (var entry in tag.elements)
                {
                    tagsByEntry[entry.RegEntryInfo].Add(tag);
                }
            }
            foreach(var entry in _entries)
            {
                if(tagsByEntry.TryGetValue(entry.RegEntryInfo, out var tagSet))
                {
                    entry.RegEntryInfo.ResetTags(tagSet);
                }
            }
        }
    }
}
