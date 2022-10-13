namespace ExtBlock.Game
{
    public class World : IWorld
    {
        protected IChunkSource _chunks = new HashChunkSource();

        public bool GetBlockStateAt(int x, int y, int z, [NotNullWhen(true)] out BlockState? state)
        {
            BlockPos chunkPos = GetPosAt(x, y, z, out BlockPos inChunkPos);
            if (_chunks.Get(chunkPos, out IChunk? chunk))
            {
                state = chunk.GetBlockState(inChunkPos);
                return true;
            }
            state = null;
            return false;
        }
        public bool GetBlockStateAt(BlockPos pos, [NotNullWhen(true)] out BlockState? state)
        {
            return GetBlockStateAt(pos.X, pos.Y, pos.Z, out state);
        }

        public bool SetBlockStateAt(int x, int y, int z, BlockState state)
        {
            BlockPos chunkPos = GetPosAt(x, y, z, out BlockPos inChunkPos);
            if (_chunks.Get(chunkPos, out IChunk? chunk))
            {
                chunk.SetBlockState(inChunkPos, state);
                return true;
            }
            return false;
        }
        public bool SetBlockStateAt(BlockPos pos, BlockState state)
        {
            return SetBlockStateAt(pos.X, pos.Y, pos.Z, state);
        }

        public static BlockPos GetPosAt(int x, int y, int z, out BlockPos inChunkPos)
        {
            inChunkPos = new BlockPos(x & 0xF, y & 0xF, z & 0xF);
            return new BlockPos(x >> 4, y >> 4, z >> 4);
        }
        public static BlockPos GetPosAt(BlockPos pos, out BlockPos inChunkPos)
        {
            return GetPosAt(pos.X, pos.Y, pos.Z, out inChunkPos);
        }
    }
}
