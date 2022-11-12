using System;
using System.Collections.Generic;
using System.Text;

namespace ExtBlock.Core.Tag
{
    public readonly struct TagData
    {
        public readonly bool isOverride;

        public readonly TagBuilder.Entry[] entries;

        public TagData(bool isOverride, TagBuilder.Entry[] entries)
        {
            this.isOverride = isOverride;
            this.entries = entries;
        }
    }
}
