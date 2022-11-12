using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ExtBlock.Network
{
    public class ByteBuffer
    {
        private int _curr = 0;
        private readonly List<byte> _data;
        private readonly byte[] _tmpBuffer;

        public ByteBuffer(byte[]? data)
        {
            _tmpBuffer = new byte[16];
            if(data == null)
            {
                _data = new List<byte>();
            }
            else
            {
                _data = data.ToList();
            }
        }

        public void Clear()
        {
            _curr = 0;
            _data.Clear();
        }

        public void Reset()
        {
            _curr = 0;
        }

        public byte ReadByte()
        {
            byte ret = _data[_curr];
            _curr += sizeof(byte);
            return ret;
        }

        public void WriteByte(byte data)
        {
            while(_data.Count < _curr)
            {
                _data.Add(default);
            }
            _data.Add(data);
            _curr += sizeof(byte);
        }

        public short ReadInt16()
        {
            const int size = sizeof(short);
            _data.CopyTo(_curr, _tmpBuffer, 0, size);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(_tmpBuffer, 0, size);
            }
            _curr += size;
            return BitConverter.ToInt16(_tmpBuffer);
        }

        public void WriteInt16(short data)
        {
            while (_data.Count < _curr + sizeof(short))
            {
                _data.Add(default);
            }
            BitConverter.TryWriteBytes(_tmpBuffer, data);
            _data.Add(_tmpBuffer[0]);
            _data.Add(_tmpBuffer[1]);
        }

        public int ReadInt32()
        {
            _data.CopyTo(_curr, _tmpBuffer, 0, sizeof(int));
            _curr += sizeof(int);
            return BitConverter.ToInt32(_tmpBuffer);
        }

        public long ReadInt64()
        {
            _data.CopyTo(_curr, _tmpBuffer, 0, sizeof(long));
            _curr += sizeof(long);
            return BitConverter.ToInt64(_tmpBuffer);
        }
    }
}
