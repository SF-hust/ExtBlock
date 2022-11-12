using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using ExtBlock.Math;

namespace ExtBlock.Game.BlockEntity
{
    public interface IBlockEntityGetter
    {
        public bool HasBlockEntityAt(int x, int y, int z);
        public bool HasBlockEntityAt(BlockPos pos)
        {
            return HasBlockEntityAt(pos.x, pos.y, pos.z);
        }

        public bool TryGetBlockEntityAt(int x, int y, int z, [NotNullWhen(true)] out BlockEntity? blockEntity);
        public bool TryGetBlockEntityAt(BlockPos pos, [NotNullWhen(true)] out BlockEntity? blockEntity)
        {
            return TryGetBlockEntityAt(pos.x, pos.y, pos.z, out blockEntity);
        }

        public IEnumerable<BlockEntity> GetBlockStateRange(int x1, int y1, int z1, int x2, int y2, int z2);
        public IEnumerable<BlockEntity> GetBlockStateRange(BlockPos pos1, BlockPos pos2)
        {
            return GetBlockStateRange(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);
        }

    }
}
