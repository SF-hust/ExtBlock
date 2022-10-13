namespace ExtBlock.Core
{
    public interface IChunk
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }


        public IWorld World { get; }

        public void SetBlockState(int x, int y, int z, BlockState blockState);
        public void SetBlockState(BlockPos pos, BlockState blockState);

        public BlockState GetBlockState(int x, int y, int z);
        public BlockState GetBlockState(BlockPos pos);
    }
}