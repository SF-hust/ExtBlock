using System;
using System.Collections.Generic;

using ExtBlock.Utility;

namespace ExtBlock.Resource
{
    public sealed class ResourceKey : IComparable<ResourceKey>
    {
        /// <summary>
        /// 存储所有 ResourceKey, 值相同的 ResourceKey 只会有一个
        /// </summary>
        private static readonly Dictionary<string, ResourceKey> RESOURCE_KEYS = new Dictionary<string, ResourceKey>();

        public readonly ResourceLocation registry;
        public readonly ResourceLocation location;

        private readonly int _hashcodeCache;

        public static ResourceKey Create(ResourceLocation registry, ResourceLocation location)
        {
            string key = registry.ToString() + "/" + location.ToString();
            ResourceKey resourceKey;
            lock (RESOURCE_KEYS)
            {
                if (!RESOURCE_KEYS.TryGetValue(key, out resourceKey))
                {
                    resourceKey = new ResourceKey(registry, location);
                    RESOURCE_KEYS.TryAdd(key, resourceKey);
                }
            }
            return resourceKey;
        }

        public static ResourceKey Create(ResourceKey registryKey, ResourceLocation location)
        {
            return Create(registryKey.location, location);
        }

        public static ResourceKey CreateRegistryKey(ResourceLocation location)
        {
            return Create(Constants.REGISTRY_LOCATION, location);
        }
        
        public static ResourceKey CreateRegistryKey(string name)
        {
            return CreateRegistryKey(ResourceLocation.Create(name));
        }

        private ResourceKey(ResourceLocation registry, ResourceLocation location)
        {
            this.registry = registry;
            this.location = location;
            _hashcodeCache = 31 * registry.GetHashCode() + location.GetHashCode();
        }

        public bool IsFor(ResourceKey registryKey)
        {
            return registry.Equals(registryKey.location);
        }

        public int CompareTo(ResourceKey? other)
        {
            int rns = registry.CompareTo(other?.registry);
            if (rns != 0)
            {
                return rns;
            }
            return location.CompareTo(other?.location);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return _hashcodeCache;
        }

        public override string ToString()
        {
            return registry.ToString() + "/" + location.ToString();
        }
    }
}
