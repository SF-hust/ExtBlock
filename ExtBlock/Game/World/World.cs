using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using ExtBlock.Math;

namespace ExtBlock.Game
{
    public class World : IWorld
    {
        protected IChunkSource _chunks = new HashChunkSource();

        public bool HasBlockAt(int x, int y, int z)
        {
            ChunkPos chunkPos = GetChunkPosAt(x, y, z);
            return _chunks.Exist(chunkPos);
        }

        public bool TryGetBlockState(int x, int y, int z, out BlockState? state)
        {
            ChunkPos chunkPos = GetPosAt(x, y, z, out BlockPos inChunkPos);
            if (_chunks.Get(chunkPos, out IChunk? chunk))
            {
                state = chunk.GetBlockState(inChunkPos);
                return true;
            }
            state = null;
            return false;
        }

        public IEnumerable<BlockState> GetBlockStateRange(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            throw new System.NotImplementedException();
        }

        public bool SetBlockStateAt(int x, int y, int z, BlockState state)
        {
            ChunkPos chunkPos = GetPosAt(x, y, z, out BlockPos inChunkPos);
            if (_chunks.Get(chunkPos, out IChunk? chunk))
            {
                chunk.SetBlockState(inChunkPos, state);
                return true;
            }
            return false;
        }

        public static ChunkPos GetChunkPosAt(int x, int y, int z)
        {
            return new ChunkPos(x >> 4, y >> 4, z >> 4);
        }
        public static ChunkPos GetChunkPosAt(BlockPos pos)
        {
            return GetChunkPosAt(pos.x, pos.y, pos.z);
        }

        public static ChunkPos GetPosAt(int x, int y, int z, out BlockPos inChunkPos)
        {
            inChunkPos = new BlockPos(x & 0xF, y & 0xF, z & 0xF);
            return new ChunkPos(x >> 4, y >> 4, z >> 4);
        }
        public static ChunkPos GetPosAt(BlockPos pos, out BlockPos inChunkPos)
        {
            return GetPosAt(pos.x, pos.y, pos.z, out inChunkPos);
        }

    }
}
