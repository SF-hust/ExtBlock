using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using ExtBlock.Resource;

namespace ExtBlock.Core.Tag
{
    public class TagBuilder
    {
        public delegate Tag TagFactory(ResourceLocation id, IEnumerable<(ResourceLocation, bool)> elements);
        public delegate bool TagGetter(ResourceLocation id, [NotNullWhen(true)] out Tag? tag);

        private readonly List<(Entry, string)> _entries = new List<(Entry, string)>();
        private readonly ResourceLocation _tagId;
        private TagBuilder(ResourceLocation id)
        {
            _tagId = id;
        }

        public static TagBuilder Create(ResourceLocation location)
        {
            return new TagBuilder(location);
        }

        public TagBuilder AddEntry(Entry entry, string source)
        {
            _entries.Add((entry, source));
            return this;
        }

        public TagBuilder AddElement(ResourceLocation id, bool optional, string source)
        {
            return AddEntry(new Entry(id, Entry.EntryType.Element, optional), source);
        }

        public TagBuilder AddTag(ResourceLocation id, bool optional, string source)
        {
            return AddEntry(new Entry(id, Entry.EntryType.Tag, optional), source);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public bool TryBuild(TagFactory factory, TagGetter tagGetter,
            [NotNullWhen(true)] out Tag? tag,
            [NotNullWhen(false)] out List<(ResourceLocation, Entry.EntryType, string)>? missingDependencies)
        {
            bool missing = false;
            List<(ResourceLocation, Entry.EntryType, string)> missings = new List<(ResourceLocation, Entry.EntryType, string)>();
            foreach (var entry in _entries)
            {
                
            }
            if (missing)
            {
                _entries.Clear();
            }
            return !missing;
        }

        public IEnumerable<(ResourceLocation, string)> GetRequiredDependencies()
        {
            List<(ResourceLocation, string)> dependencies = new List<(ResourceLocation, string)>();
            foreach (var pair in _entries)
            {
                if(pair.Item1.entryType == Entry.EntryType.Tag && pair.Item1.optional == false)
                {
                    dependencies.Add((pair.Item1.id, pair.Item2));
                }
            }
            return dependencies;
        }

        public IEnumerable<(ResourceLocation, string)> GetOptionalDependencies()
        {
            List<(ResourceLocation, string)> dependencies = new List<(ResourceLocation, string)>();
            foreach (var pair in _entries)
            {
                if (pair.Item1.entryType == Entry.EntryType.Tag && pair.Item1.optional == true)
                {
                    dependencies.Add((pair.Item1.id, pair.Item2));
                }
            }
            return dependencies;
        }

        public class Entry
        {
            public enum EntryType
            {
                Element,
                Tag,
            }

            public readonly ResourceLocation id;

            public readonly EntryType entryType;

            public readonly bool optional;

            public Entry(ResourceLocation id, EntryType entryType, bool optional)
            {
                this.id = id;
                this.entryType = entryType;
                this.optional = optional;
            }

            public override string ToString()
            {
                StringBuilder str = new StringBuilder();
                if(entryType == EntryType.Tag)
                {
                    str.Append("#");
                }
                str.Append(id.ToString());
                if(optional)
                {
                    str.Append("?");
                }
                return str.ToString();
            }
        }
    }
}
