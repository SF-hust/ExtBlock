using System.Diagnostics.CodeAnalysis;

using ExtBlock.Math;
using ExtBlock.Game.Block;

namespace ExtBlock.Game.World
{
    public interface IWorld : IBlockGetter
    {
        public bool SetBlockStateAt(int x, int y, int z, BlockState state);
        public bool SetBlockStateAt(BlockPos pos, BlockState state)
        {
            return SetBlockStateAt(pos.x, pos.y, pos.z, state);
        }
    }
}
