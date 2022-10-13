using System.Collections.Generic;

using ExtBlock.Math;

namespace ExtBlock.Game
{
    public interface IBlockGetter
    {
        public bool HasBlockAt(BlockPos pos);

        public bool TryGetBlockState(BlockPos pos, out BlockState state);

        public bool HasBlockEntityAt(BlockPos pos);

        public bool TryGetBlockEntity(BlockPos pos);

        public IEnumerable<BlockState> GetBlockStateRange(BlockPos pos1, BlockPos pos2);
    }
}
