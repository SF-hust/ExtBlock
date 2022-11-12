using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using ExtBlock.Resource;
using ExtBlock.Utility;
using ExtBlock.Core.Tag;

namespace ExtBlock.Data.Parsers
{
    public class TagParser : IParser<JObject, TagData>
    {
        public bool Parse(JObject input, out TagData output)
        {
            if (!JsonHelper.TryGetBool(input, "override", out bool isOverride))
            {
                isOverride = false;
            }
            List<TagBuilder.Entry> entries = new List<TagBuilder.Entry>();
            if(JsonHelper.TryGetArray(input, "entries", out JArray? array))
            {
                foreach(JToken entry in array)
                {
                    string str = (string)entry!;
                }
            }
            output = new TagData(isOverride, entries.ToArray());
            return true;
        }

        public bool TryParseString(string str, out TagBuilder.Entry entry)
        {
            TagBuilder.Entry.EntryType entryType = TagBuilder.Entry.EntryType.Element;
            bool optional = false;
            int start = 0, end = str.Length;
            if(str.StartsWith("#"))
            {
                entryType = TagBuilder.Entry.EntryType.Tag;
                start = 1;
            }
            if(str.EndsWith("?"))
            {
                optional = true;
                end -= 1;
            }
            entry = new TagBuilder.Entry(ResourceLocation.Parse(str.Substring(start, end)), entryType, optional);
            return true;
        }
    }
}
