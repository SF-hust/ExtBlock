namespace ExtBlock.Core.ChunkDataContainer
{
    public interface IChunkDataContainer<T> where T : class
    {
        public void Set(int x, int y, int z, T value);
        public T Get(int x, int y, int z);
    }
}
