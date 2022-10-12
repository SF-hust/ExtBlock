namespace ExtBlock.Core.ChunkDataContainer
{
    public sealed class DirectChunkDataContainer<T> : IChunkDataContainer<T> where T : class
    {
        private readonly int _xlen;
        private readonly int _ylen;
        private readonly int _zlen;
        private readonly int _levelSize;
        private readonly T[] _values;

        public DirectChunkDataContainer(int xlen, int ylen, int zlen)
        {
            _xlen = xlen;
            _ylen = ylen;
            _zlen = zlen;
            _levelSize = xlen * zlen;
            _values = new T[ylen * _levelSize];
        }
        
        public T Get(int x, int y, int z)
        {
            return _values[x + z * _zlen + y * _levelSize];
        }

        public void Set(int x, int y, int z, T value)
        {
            _values[x + z * _zlen + y * _levelSize] = value;
        }
    }
}
