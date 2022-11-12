using System;
using System.Collections.Generic;
using System.Text;

namespace ExtBlock.Utility.GuidGenerator
{
    public interface IGuidGenerator
    {
        public Guid NextGuid { get; }
    }
}
