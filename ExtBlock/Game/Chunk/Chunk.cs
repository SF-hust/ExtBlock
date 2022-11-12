using System;

using ExtBlock.Math;

namespace ExtBlock.Game
{
    /// <summary>
    /// Chunk 是一个 16*16*16 正方体范围内的方块集合, 是游戏地形生成, 加载与渲染的单位
    /// </summary>
    public abstract class Chunk : IChunk, IEquatable<Chunk>
    {
        public IWorld World { get => _world; }
        private readonly IWorld _world;

        public ChunkPos ChunkPos => _chunkPos;
        private readonly ChunkPos _chunkPos;

        public abstract bool Writable { get; }

        public abstract bool IsEmpty { get; }

        public Chunk(IWorld world, ChunkPos chunkPos)
        {
            _world = world;
            _chunkPos = chunkPos;
        }

        public Chunk(IWorld world, int x, int y, int z) : this(world, new ChunkPos(x, y, z))
        {
        }

        public abstract void SetBlockState(int x, int y, int z, BlockState blockState);

        public abstract BlockState GetBlockState(int x, int y, int z);

        public override int GetHashCode()
        {
            return _world.GetHashCode() * 31 * 31 * 31 + Z * 31 * 31 + Y * 31 + X;
        }

        /*
         * 需要注意的是只要所属的 World 与 ChunkPos 相等, 那两个 Chunk 就被判定为相等
         */

        public override bool Equals(object obj)
        {
            if(this == obj)
            {
                return true;
            }
            return Equals(obj as Chunk);
        }
        public bool Equals(Chunk? other)
        {
            if(this == other)
            {
                return true;
            }
            if(other == null)
            {
                return false;
            }
            return _world == other._world && _chunkPos.Equals(other._chunkPos);
        }

        public abstract void CopyToArray(BlockState[] array);
    }
}