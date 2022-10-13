using ExtBlock.Math;

namespace ExtBlock.Game
{
    public interface IChunk
    {
        public ChunkPos ChunkPosition { get; }

        public IWorld World { get; }

        public bool Writable { get; }

        public bool IsEmpty { get; }

        /// <summary>
        /// if not writable, do nothing
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="blockState"></param>
        public void SetBlockState(int x, int y, int z, BlockState blockState);

        /// <summary>
        /// if not writable, do nothing
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="blockState"></param>
        public void SetBlockState(BlockPos pos, BlockState blockState);

        public BlockState GetBlockState(int x, int y, int z);
        public BlockState GetBlockState(BlockPos pos);

        public void CopyToArray(BlockState[] array);
    }
}