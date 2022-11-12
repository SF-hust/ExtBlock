using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using ExtBlock.Math;

namespace ExtBlock.Game.Block
{
    public interface IBlockGetter
    {
        public bool HasBlockAt(int x, int y, int z);
        public bool HasBlockAt(BlockPos pos)
        {
            return HasBlockAt(pos.x, pos.y, pos.z);
        }

        public bool TryGetBlockState(int x, int y, int z, [NotNullWhen(true)] out BlockState? state);
        public bool TryGetBlockState(BlockPos pos, [NotNullWhen(true)] out BlockState? state)
        {
            return TryGetBlockState(pos.x, pos.y, pos.z, out state);
        }

        public IEnumerable<(BlockPos, BlockState)> GetBlockStateRange(int x1, int y1, int z1, int x2, int y2, int z2);
        public IEnumerable<(BlockPos, BlockState)> GetBlockStateRange(BlockPos pos1, BlockPos pos2)
        {
            return GetBlockStateRange(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);
        }
    }
}
