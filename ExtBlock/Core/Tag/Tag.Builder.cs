using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Text.Json.Nodes;

using ExtBlock.Core.Registry;
using ExtBlock.Resource;
using ExtBlock.Utility;

namespace ExtBlock.Core.Tag
{
    public partial class Tag<T>
        where T : class, IRegistryEntry<T>
    {
        public class Builder
        {
            public delegate bool ElementChecker(ResourceLocation location);
            public delegate bool ElementGetter(ResourceLocation location, [NotNullWhen(true)] out T? element);
            public delegate bool TagGetter(ResourceLocation location, [NotNullWhen(true)] out Tag<T>? tag);

            private readonly List<BuilderEntry> _entries = new List<BuilderEntry>();
            private readonly ResourceLocation _tagId;
            private readonly ElementGetter _elementGetter;
            private readonly TagGetter _tagGetter;

            private Builder(ResourceLocation location, ElementGetter elementGetter, TagGetter tagGetter)
            {
                _tagId = location;
                _elementGetter = elementGetter;
                _tagGetter = tagGetter;
            }

            public static Builder Create(ResourceLocation location, ElementGetter elementGetter, TagGetter tagGetter)
            {
                return new Builder(location, elementGetter, tagGetter);
            }

            public Builder AddEntry(IEntry entry, string source)
            {
                _entries.Add(new BuilderEntry(entry, source));
                return this;
            }

            public Builder AddElement(ResourceLocation elementId, string source, bool optional = false)
            {
                return AddEntry(new ElementEntry(elementId, optional, _elementGetter), source);
            }

            public Builder AddTag(ResourceLocation tagId, string source, bool optional = false)
            {
                return AddEntry(new TagEntry(tagId, optional, _tagGetter), source);
            }

            public Builder AddFromJson(JsonObject json, string source)
            {
                if(JsonHelper.TryGetBool(json, "replace", out bool replace) && replace)
                {
                    _entries.Clear();
                }
                if(JsonHelper.TryGetArray(json, "values", out JsonArray? array))
                {
                    foreach (JsonNode? node in array)
                    {
                        if(node is JsonValue value)
                        {
                            string rawId = value.GetValue<string>();
                            string id = rawId;
                            bool isTag = false;
                            bool optional = false;
                            if (rawId.StartsWith("#"))
                            {
                                id = rawId[1..];
                                isTag = true;
                            }
                            if(rawId.EndsWith('?'))
                            {
                                id = id[..(id.Length - 1)];
                                optional = true;
                            }
                            if(isTag)
                            {
                                AddTag(ResourceLocation.Parse(id), source, optional);
                            }
                            else
                            {
                                AddElement(ResourceLocation.Parse(id), source, optional);
                            }
                        }
                    }
                }
                return this;
            }

            public Tag<T> Build(Action<BuilderEntry> missingAction)
            {
                List<T> values = new List<T>();
                bool missing = false;
                foreach (var item in _entries)
                {
                    if(!item.entry.Build(values.Add))
                    {
                        missingAction(item);
                        missing = true;
                    }
                }
                if(missing)
                {
                    values.Clear();
                }
                return new Tag<T>(_tagId, values);
            }

            public void VisitRequiredDependencies(Action<ResourceLocation> action)
            {
                foreach(var item in _entries)
                {
                    item.entry.VisitRequiredDependencies(action);
                }
            }

            public void VisitOptionalDependencies(Action<ResourceLocation> action)
            {
                foreach (var item in _entries)
                {
                    item.entry.VisitOptionalDependencies(action);
                }
            }

            public sealed class BuilderEntry
            {
                public readonly IEntry entry;

                public readonly string source;

                public BuilderEntry(IEntry entry, string source)
                {
                    this.entry = entry;
                    this.source = source;
                }
            }

            public interface IEntry
            {
                public bool Build(Action<T> action);

                public void VisitRequiredDependencies(Action<ResourceLocation> action);

                public void VisitOptionalDependencies(Action<ResourceLocation> action);

                public bool VerifyIfElementPresent(ElementChecker elementChecker);

                public void AddToJson(JsonArray jArray);
            }

            public class ElementEntry : IEntry
            {
                private readonly ResourceLocation _id;
                private readonly bool _optional;
                private readonly ElementGetter _elementGetter;
                public bool Optional => _optional;

                public bool Build(Action<T> action)
                {
                    if(_elementGetter(_id, out T? element))
                    {
                        action(element);
                        return true;
                    }
                    return _optional;
                }

                public void VisitRequiredDependencies(Action<ResourceLocation> action)
                {
                }

                public void VisitOptionalDependencies(Action<ResourceLocation> action)
                {
                }

                public bool VerifyIfElementPresent(ElementChecker elementChecker)
                {
                    return elementChecker(_id);
                }

                public ElementEntry(ResourceLocation location, bool optional, ElementGetter elementGetter)
                {
                    _id = location;
                    _optional = optional;
                    _elementGetter = elementGetter;
                }

                public void AddToJson(JsonArray jArray)
                {
                    jArray.Add(_id.ToString() + (_optional ? string.Empty : "?"));
                }

                public override string ToString()
                {
                    return _id.ToString() + (_optional ? string.Empty : "?");
                }
            }

            public class TagEntry : IEntry
            {
                private readonly ResourceLocation _id;
                private readonly bool _optional;
                private readonly TagGetter _tagGetter;
                public bool Optional => _optional;

                public bool Build(Action<T> action)
                {
                    if(_tagGetter(_id, out Tag<T>? tag))
                    {
                        tag.elements.ForEach(action);
                        return true;
                    }
                    return _optional;
                }

                public void VisitRequiredDependencies(Action<ResourceLocation> action)
                {
                    if(!_optional)
                    {
                        action(_id);
                    }
                }

                public void VisitOptionalDependencies(Action<ResourceLocation> action)
                {
                    if (_optional)
                    {
                        action(_id);
                    }
                }

                public bool VerifyIfElementPresent(Tag<T>.Builder.ElementChecker elementChecker)
                {
                    return true;
                }

                public TagEntry(ResourceLocation location, bool optional, TagGetter tagGetter)
                {
                    _id = location;
                    _optional = optional;
                    _tagGetter = tagGetter;
                }

                public void AddToJson(JsonArray jArray)
                {
                    jArray.Add("#" + _id.ToString() + (_optional ? string.Empty : "?"));
                }

                public override string ToString()
                {
                    return "#" + _id.ToString() + (_optional ? string.Empty : "?");
                }
            }
        }
    }
}
