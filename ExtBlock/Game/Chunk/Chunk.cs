using System;
using ExtBlock.Math;

namespace ExtBlock.Game
{
    public abstract class Chunk : IChunk, IEquatable<Chunk>
    {
        private readonly IWorld _world;
        public IWorld World { get => _world; }

        private readonly ChunkPos _chunkPos;
        public ChunkPos ChunkPosition => _chunkPos;

        public int X => _chunkPos.x;
        public int Y => _chunkPos.y;
        public int Z => _chunkPos.z;

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
        public void SetBlockState(BlockPos pos, BlockState blockState)
        {
            SetBlockState(pos.x, pos.y, pos.z, blockState);
        }

        public abstract BlockState GetBlockState(int x, int y, int z);
        public BlockState GetBlockState(BlockPos pos)
        {
            return GetBlockState(pos.x, pos.y, pos.z);
        }

        public override int GetHashCode()
        {
            return _world.GetHashCode() * 31 * 31 * 31 + Z * 31 * 31 + Y * 31 + X;
        }

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