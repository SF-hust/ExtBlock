namespace ExtBlock.Core
{
    public class EmptyChunk : AbsChunk
    {
        private BlockState _state;

        public EmptyChunk(int x, int y, int z, IWorld world, BlockState state) : base(x, y, z, world)
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