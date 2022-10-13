using ExtBlock.Game.ChunkDataContainer;
using ExtBlock.Math;

namespace ExtBlock.Game
{
    public class StorageChunk : Chunk
    {
        protected IChunkDataContainer<BlockState> _blockStates = new DirectChunkDataContainer<BlockState>(16, 16, 16);

        public override bool Writable => true;

        public override bool IsEmpty => false;

        public StorageChunk(IWorld world, int x, int y, int z) : base(world, x, y, z)
        {
        }

        public StorageChunk(IWorld world, ChunkPos pos) : base(world, pos)
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

        public override void CopyToArray(BlockState[] array)
        {
            _blockStates.CopyToArray(array);
        }
    }
}
