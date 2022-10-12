using ExtBlock.Core.ChunkDataContainer;

namespace ExtBlock.Core
{
    public class Chunk : AbsChunk
    {
        protected IChunkDataContainer<BlockState> _blockStates = new DirectChunkDataContainer<BlockState>(16, 16, 16);

        public Chunk(int x, int y, int z, IWorld world) : base(x, y, z, world)
        {
        }

        public override void SetBlockState(int x, int y, int z, BlockState blockState)
        {
            _blockStates.Set(x, y, z, blockState);
        }

        public override BlockState GetBlockState(int x, int y, int z)
        {
            return _blockStates.Get(x, y, z);
        }
    }
}
