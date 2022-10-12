namespace ExtBlock.Core
{
    public abstract class AbsChunk : IChunk
    {
        private int _x, _y, _z;
        public int X { get => _x; }
        public int Y { get => _y; }
        public int Z { get => _z; }


        private IWorld _world;
        public IWorld World { get => _world; }

        public AbsChunk(int x, int y, int z, IWorld world)
        {
            _x = x;
            _y = y;
            _z = z;
            _world = world;
        }

        public abstract void SetBlockState(int x, int y, int z, BlockState blockState);
        public void SetBlockState(BlockPos pos, BlockState blockState)
        {
            SetBlockState(pos.X, pos.Y, pos.Z, blockState);
        }

        public abstract BlockState GetBlockState(int x, int y, int z);
        public BlockState GetBlockState(BlockPos pos)
        {
            return GetBlockState(pos.X, pos.Y, pos.Z);
        }

        public override int GetHashCode()
        {
            return _world.GetHashCode() * 65535 + _z * 1023 + _y * 31 + _x;
        }

        public override bool Equals(object? obj)
        {
            if(this == obj)
            {
                return true;
            }
            if(obj is AbsChunk chunk)
            {
                return _world == chunk._world && _x == chunk._x && _y == chunk._y && _z == chunk._z;
            }
            return false;
        }


    }
}