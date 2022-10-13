using System.Diagnostics.CodeAnalysis;

using ExtBlock.Math;

namespace ExtBlock.Game
{
    public interface IChunkSource
    {
        public bool Get(int x, int y, int z, [NotNullWhen(true)] out IChunk? chunk);
        public bool Get(ChunkPos pos, [NotNullWhen(true)] out IChunk? chunk);

        public bool Exist(int x, int y, int z);
        public bool Exist(ChunkPos pos);

        public void Set(int x, int y, int z, IChunk chunk);
        public void Set(ChunkPos pos, IChunk chunk);

        public void Remove(int x, int y, int z);
        public void Remove(ChunkPos pos);
    }
}