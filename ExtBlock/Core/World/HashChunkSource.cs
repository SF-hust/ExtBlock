namespace ExtBlock.Core
{
    public class HashChunkSource : IChunkSource
    {
        Dictionary<BlockPos, IChunk> _chunks = new();

        public bool Get(int x, int y, int z, [NotNullWhen(true)] out IChunk? chunk)
        {
            BlockPos pos = new(x, y, z);
            return _chunks.TryGetValue(pos, out chunk);
        }
        public bool Get(BlockPos pos, [NotNullWhen(true)] out IChunk? chunk)
        {
            return _chunks.TryGetValue(pos, out chunk);
        }

        public bool Exist(int x, int y, int z)
        {
            BlockPos pos = new(x, y, z);
            return _chunks.ContainsKey(pos);
        }

        public bool Exist(BlockPos pos)
        {
            return _chunks.ContainsKey(pos);
        }

        public void Set(int x, int y, int z, IChunk chunk)
        {
            BlockPos pos = new(x, y, z);
            _chunks[pos] = chunk;
        }

        public void Set(BlockPos pos, IChunk chunk)
        {
            _chunks[pos] = chunk;
        }

        public void Remove(int x, int y, int z)
        {
            BlockPos pos = new(x, y, z);
            _chunks.Remove(pos);
        }

        public void Remove(BlockPos pos)
        {
            _chunks.Remove(pos);
        }
    }
}