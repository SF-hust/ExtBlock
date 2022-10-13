using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using ExtBlock.Math;

namespace ExtBlock.Game
{
    public class HashChunkSource : IChunkSource
    {
        Dictionary<ChunkPos, IChunk> _chunks = new Dictionary<ChunkPos, IChunk>();

        public bool Get(int x, int y, int z, [NotNullWhen(true)] out IChunk? chunk)
        {
            ChunkPos pos = new ChunkPos(x, y, z);
            return _chunks.TryGetValue(pos, out chunk);
        }
        public bool Get(ChunkPos pos, [NotNullWhen(true)] out IChunk? chunk)
        {
            return _chunks.TryGetValue(pos, out chunk);
        }

        public bool Exist(int x, int y, int z)
        {
            ChunkPos pos = new ChunkPos(x, y, z);
            return _chunks.ContainsKey(pos);
        }

        public bool Exist(ChunkPos pos)
        {
            return _chunks.ContainsKey(pos);
        }

        public void Set(int x, int y, int z, IChunk chunk)
        {
            ChunkPos pos = new ChunkPos(x, y, z);
            _chunks[pos] = chunk;
        }

        public void Set(ChunkPos pos, IChunk chunk)
        {
            _chunks[pos] = chunk;
        }

        public void Remove(int x, int y, int z)
        {
            ChunkPos pos = new ChunkPos(x, y, z);
            _chunks.Remove(pos);
        }

        public void Remove(ChunkPos pos)
        {
            _chunks.Remove(pos);
        }
    }
}