using System;
using System.Collections.Generic;

namespace ExtBlock.Resource
{
    public sealed class ResourceKey : IComparable<ResourceKey>
    {
        public const string DEFAULT_REGISTRY_PATH = "registry";

        public static readonly ResourceLocation REGISTRY = ResourceLocation.Create(DEFAULT_REGISTRY_PATH);

        private static readonly Dictionary<string, ResourceKey> RESOURCE_KEYS = new Dictionary<string, ResourceKey>();

        public readonly ResourceLocation Registry;
        public readonly ResourceLocation Location;

        public static ResourceKey Create(ResourceLocation registry, ResourceLocation location)
        {
            string key = registry.ToString() + ":" + location.ToString();
            if (RESOURCE_KEYS.TryGetValue(key, out ResourceKey resourceKey))
            {
                return resourceKey;
            }
            resourceKey = new ResourceKey(registry, location);
            RESOURCE_KEYS.Add(key, resourceKey);
            return resourceKey;
        }

        public static ResourceKey Create(ResourceKey registryKey, ResourceLocation location)
        {
            return Create(registryKey.Location, location);
        }

        public static ResourceKey CreateRegistryKey(ResourceLocation location)
        {
            return Create(REGISTRY, location);
        }
        
        public static ResourceKey CreateRegistryKey(string name)
        {
            return CreateRegistryKey(ResourceLocation.Create(name));
        }

        private ResourceKey(ResourceLocation registry, ResourceLocation location)
        {
            Registry = registry;
            Location = location;
        }

        public bool IsFor(ResourceKey registryKey)
        {
            return Registry.Equals(registryKey.Location);
        }

        public int CompareTo(ResourceKey? other)
        {
            int rns = Registry.CompareTo(other?.Registry);
            if (rns != 0)
            {
                return rns;
            }
            return Location.CompareTo(other?.Location);
        }

        public sealed override bool Equals(object? obj)
        {
            return CompareTo(obj as ResourceKey) == 0;
        }

        public sealed override int GetHashCode()
        {
            return 31 * Registry.GetHashCode() + Location.GetHashCode();
        }

        public sealed override string ToString()
        {
            return Registry.ToString() + "/" + Location.ToString();
        }
    }
}
