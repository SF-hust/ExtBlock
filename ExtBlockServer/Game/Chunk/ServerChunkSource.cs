using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtBlock.Game;
using ExtBlock.Math;

namespace ExtBlock.Server.Game.Chunk
{
    public class ServerChunkSource : IChunkSource
    {


        public bool Exist(int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        public bool Exist(ChunkPos pos)
        {
            throw new NotImplementedException();
        }

        public bool Get(int x, int y, int z, [NotNullWhen(true)] out IChunk? chunk)
        {
            throw new NotImplementedException();
        }

        public bool Get(ChunkPos pos, [NotNullWhen(true)] out IChunk? chunk)
        {
            throw new NotImplementedException();
        }

        public void Remove(int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        public void Remove(ChunkPos pos)
        {
            throw new NotImplementedException();
        }

        public void Set(int x, int y, int z, IChunk chunk)
        {
            throw new NotImplementedException();
        }

        public void Set(ChunkPos pos, IChunk chunk)
        {
            throw new NotImplementedException();
        }
    }
}
