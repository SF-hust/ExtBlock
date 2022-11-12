using System;
using System.Collections.Generic;
using System.Text;

namespace ExtBlock.Network
{
    public interface IPacket
    {
        public void Encode();

        public void Decode();
    }
}
