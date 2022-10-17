using System;
using ExtBlock.Resource;

namespace ExtBlock.Utility.Exceptions
{
    public class EntryNotFoundException : Exception
    {
        public readonly ResourceLocation Id;

        public EntryNotFoundException(ResourceLocation id) : this(id, string.Empty)
        {
        }

        public EntryNotFoundException(ResourceLocation id, string source)
        {
            Id = id;
            Source = source;
        }

        public override string Message => ToString();

        public override string ToString()
        {
            return "can't find entry \"" + Id.ToString() + "\" from source (" + Source + ")";
        }
    }
}
