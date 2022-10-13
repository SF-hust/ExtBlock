using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExtBlock.Core.Property;

namespace ExtBlock.Core
{
    public interface IBlockPropertyProvider
    {
        public BlockProperty Properties { get; }
    }
}
