using System;
using System.Collections.Generic;

using ExtBlock.Resource;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// untyped registry class, also a registry entry
    /// </summary>
    public abstract class Registry : IRegistryEntry<Registry>
    {
        /*
         * as RegistryEntry 
         */

        private RegistryInfo<Registry>? _regInfo;
        RegistryInfo<Registry> IRegistryEntry<Registry>.RegInfo { get => _regInfo!; set => _regInfo = value; }
        RegistryInfo IRegistryEntry.UntypedRegInfo => _regInfo!;


        /*
         * as Registry
         */

        /// <summary>
        /// typeof the entry's class
        /// </summary>
        public abstract Type EntryType { get; }

        /// <summary>
        /// registry entires added to this registry as untyped
        /// </summary>
        public abstract IEnumerable<IRegistryEntry> UntypedEntries { get; }

        /// <summary>
        /// get untyped registrty entry by integer id
        /// </summary>
        /// <param name="id">entry id</param>
        /// <param name="entry">untyped entry instance</param>
        /// <returns>true if entry of id exists, or else false</returns>
        public abstract bool TryGetUntypedEntryById(int id, out IRegistryEntry? entry);

        /// <summary>
        /// get untyped registrty entry by ResourceLocation
        /// </summary>
        /// <param name="location">entry's (modid, name)</param>
        /// <param name="entry">untyped entry instance</param>
        /// <returns>true if entry of location exists, or else false</returns>
        public abstract bool TryGetUntypedEntryByLocation(ResourceLocation location, out IRegistryEntry? entry);

        /// <summary>
        /// get ResourceLocation by integer id
        /// </summary>
        /// <param name="location">entry's (modid, name)</param>
        /// <returns>an id (> 0) if entry of location exists, or else -1</returns>
        public abstract int GetIdByLocation(ResourceLocation location);

        /// <summary>
        /// fire register event for registry entries of this registry
        /// </summary>
        public abstract void FireRegisterEvent();
    }

    /// <summary>
    /// typed registry, you can add entries to this
    /// </summary>
    /// <typeparam name="ET">class of registry entry</typeparam>
    public class Registry<ET> : Registry, IRegistryEntry<Registry>
        where ET : class, IRegistryEntry<ET>
    {
        public override Type EntryType => typeof(ET);

        protected List<IRegistryEntry<ET>> _entries = new List<IRegistryEntry<ET>>();
        /// <summary>
        /// typed registry entries
        /// </summary>
        public IEnumerable<IRegistryEntry<ET>> Entries => _entries;
        public override IEnumerable<IRegistryEntry> UntypedEntries => _entries;

        protected Dictionary<ResourceLocation, int> _idsByLocation = new Dictionary<ResourceLocation, int>();

        public override bool TryGetUntypedEntryById(int id, out IRegistryEntry? entry)
        {
            if(id < _entries.Count)
            {
                entry = _entries[id];
                return true;
            }
            entry = null;
            return false;
        }

        /// <summary>
        /// get typed registrty entry by integer id
        /// </summary>
        /// <param name="id">entry id</param>
        /// <param name="entry">entry instance</param>
        /// <returns>true if entry of id exists, or else false</returns>
        public bool TryGetEntryById(int id, out IRegistryEntry<ET>? entry)
        {
            if (id < _entries.Count)
            {
                entry = _entries[id];
                return true;
            }
            entry = null;
            return false;
        }

        public override bool TryGetUntypedEntryByLocation(ResourceLocation location, out IRegistryEntry? entry)
        {
            if (_idsByLocation.TryGetValue(location, out int id))
            {
                entry = _entries[id];
                return true;
            }
            entry = null;
            return false;
        }

        /// <summary>
        /// get typed registrty entry by ResourceLocation
        /// </summary>
        /// <param name="location">entry's (modid, name)</param>
        /// <param name="entry">typed entry instance</param>
        /// <returns>true if entry of location exists, or else false</returns>
        public bool TryGetEntryByLocation(ResourceLocation location, out IRegistryEntry<ET>? entry)
        {
            if (_idsByLocation.TryGetValue(location, out int id))
            {
                entry = _entries[id];
                return true;
            }
            entry = null;
            return false;
        }

        public override int GetIdByLocation(ResourceLocation location)
        {
            if (_idsByLocation.TryGetValue(location, out int id))
            {
                return id;
            }
            return -1;
        }

        private bool _isLocked;
        /// <summary>
        /// indicates if this registry is locked, if true, this registry can't be modified
        /// </summary>
        public bool IsLocked { get => _isLocked; set => _isLocked=value; }

        /// <summary>
        /// add entry to this registry and set RegInfo for this entry, don't do this directly
        /// </summary>
        /// <param name="entry">the entry you want to register</param>
        /// <param name="location">(modid, name) of this entry</param>
        /// <returns>true if operation successed, or else false</returns>
        public bool Add(ET entry, ResourceLocation location)
        {
            if(IsLocked)
            {
                return false;
                //throw new InvalidOperationException("can't add entry to a locked registry");
            }
            int id = _entries.Count;
            _entries.Add(entry);
            ResourceLocation registryLocation = ((IRegistryEntry<Registry>)this).RegInfo.Location;
            RegistryInfo<ET> info = new RegistryInfo<ET>(id, ResourceKey.Create(registryLocation, location), this, entry);
            entry.RegInfo = info;
            return true;
        }

        /// <summary>
        /// used for OnRegisterEvent, empty now
        /// </summary>
        public class RegisterEventArgs : EventArgs
        {
            public static new RegisterEventArgs Empty = new RegisterEventArgs();
            public RegisterEventArgs() : base()
            {
            }
        }

        /// <summary>
        /// this event will be fired when game start registering
        /// </summary>
        public event EventHandler<RegisterEventArgs>? OnRegisterEvent;

        public override void FireRegisterEvent()
        {
            OnRegisterEvent?.Invoke(this, RegisterEventArgs.Empty);
        }
    }
}
