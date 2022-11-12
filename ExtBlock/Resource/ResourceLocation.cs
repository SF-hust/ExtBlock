using System;
using System.IO;
using System.Xml.Linq;
using ExtBlock.Utility;

namespace ExtBlock.Resource
{
    public sealed class ResourceLocation : IComparable<ResourceLocation>
    {
        public readonly string namspace;
        public readonly string path;

        public static ResourceLocation Create(string namspace, string path)
        {
            if (!IsValidNamespace(namspace))
            {
                throw new ArgumentException($"Fail to create ResourceLocation : namespace \"{namspace}\" is invalid");
            }
            if (!IsValidPath(path))
            {
                throw new ArgumentException($"Fail to create ResourceLocation : path \"{path}\" is invalid");
            }
            return new ResourceLocation(namspace, path);
        }

        public static ResourceLocation Create(string path)
        {
            return Create(Constants.DEFAULT_NAMESPACE, path);
        }

        public static ResourceLocation? TryCreate(string namspace, string path)
        {
            if (!IsValidNamespace(namspace))
            {
                return null;
            }
            if (!IsValidPath(path))
            {
                return null;
            }
            return new ResourceLocation(namspace, path);
        }

        public static ResourceLocation? TryParse(string location)
        {
            string[] splits = location.Split(':');
            if(splits.Length != 2)
            {
                return null;
            }
            return TryCreate(splits[0], splits[1]);
        }
        
        public static ResourceLocation Parse(string location)
        {
            ResourceLocation? loc = TryParse(location);
            if(loc == null)
            {
                throw new ArgumentException($"Fail to parse ResourceLocation \"{location}\"");
            }
            return loc;
        }

        public static bool IsValidNamespace(string namspace)
        {
            if (string.Empty.Equals(namspace))
            {
                return false;
            }
            foreach(char c in namspace)
            {
                if(!(char.IsLower(c) || char.IsNumber(c) || c == '.' || c == '-' || c == '_'))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidPath(string path)
        {
            if (string.Empty.Equals(path))
            {
                return false;
            }
            foreach (char c in path)
            {
                if (!(char.IsLower(c) || char.IsNumber(c) || c == '.' || c == '-' || c == '_' || c == '/'))
                {
                    return false;
                }
            }
            return true;
        }

        private ResourceLocation(string namspace, string path)
        {
            this.namspace = namspace;
            this.path = path;
        }

        public int CompareTo(ResourceLocation? other)
        {
            if(this == other)
            {
                return 0;
            }
            if(other == null)
            {
                return -1;
            }
            int rns = path.CompareTo(other?.path);
            if (rns != 0)
            {
                return rns;
            }
            return namspace.CompareTo(other?.namspace);
        }

        public int CompareNamespaced(ResourceLocation? other)
        {
            if (this == other)
            {
                return 0;
            }
            if (other == null)
            {
                return -1;
            }
            int rns = namspace.CompareTo(other?.namspace);
            if (rns != 0)
            {
                return rns;
            }
            return path.CompareTo(other?.path);
        }

        public override bool Equals(object? obj)
        {
            return CompareTo(obj as ResourceLocation) == 0;
        }

        public override int GetHashCode()
        {
            return 31 * namspace.GetHashCode() + path.GetHashCode();
        }

        public override string ToString()
        {
            return namspace + ":" + path;
        }
    }
}
