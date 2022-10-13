namespace ExtBlock.Game.ChunkDataContainer
{
    public interface IChunkDataContainer<T> where T : class
    {
        public void Set(int x, int y, int z, T value);
        public T Get(int x, int y, int z);

        public void CopyToArray(T[] array);
    }
}
