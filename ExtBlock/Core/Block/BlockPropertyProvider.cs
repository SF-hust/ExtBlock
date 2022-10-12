using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtBlock.Core
{
    public interface IBlockPropertyProvider
    {
        public BlockPropertyProvider Properties { get; }
    }

    public class BlockPropertyProvider : PropertyProvider, IBlockPropertyProvider
    {
        public new BlockPropertyProvider Properties => this;

        
    }
}
