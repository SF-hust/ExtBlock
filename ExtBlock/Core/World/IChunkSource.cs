namespace ExtBlock.Core
{
    public interface IChunkSource
    {
        public bool Get(int x, int y, int z, [NotNullWhen(true)] out IChunk? chunk);
        public bool Get(BlockPos pos, [NotNullWhen(true)] out IChunk? chunk);

        public bool Exist(int x, int y, int z);
        public bool Exist(BlockPos pos);

        public void Set(int x, int y, int z, IChunk chunk);
        public void Set(BlockPos pos, IChunk chunk);

        public void Remove(int x, int y, int z);
        public void Remove(BlockPos pos);
    }
}