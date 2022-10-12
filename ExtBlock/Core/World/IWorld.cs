namespace ExtBlock.Core
{
    public interface IWorld
    {
        public bool GetBlockStateAt(int x, int y, int z, [NotNullWhen(true)] out BlockState? state);
        public bool GetBlockStateAt(BlockPos pos, [NotNullWhen(true)] out BlockState? state);

        public bool SetBlockStateAt(int x, int y, int z, BlockState state);
        public bool SetBlockStateAt(BlockPos pos, BlockState state);
    }
}
