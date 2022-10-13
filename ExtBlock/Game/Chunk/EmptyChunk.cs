using ExtBlock.Math;

namespace ExtBlock.Game
{
    public class EmptyChunk : Chunk
    {
        private readonly BlockState _state;

        public override bool Writable => false;

        public EmptyChunk(IWorld world, BlockState state, int x, int y, int z) : base(world, x, y, z)
        {
            _state = state;
        }

        public EmptyChunk(IWorld world, BlockState state, ChunkPos pos) : base(world, pos)
        {
            _state = state;
        }

        public override void SetBlockState(int x, int y, int z, BlockState blockState)
        {
        }

        public override BlockState GetBlockState(int x, int y, int z)
        {
            return _state;
        }
    }
}