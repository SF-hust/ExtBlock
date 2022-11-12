using System;

using ExtBlock.Core.Component;
using ExtBlock.Game.Block;
using ExtBlock.Game.World;
using ExtBlock.Math;

namespace ExtBlock.CoreMod.Block.Components
{
    public abstract class RandomTickComponent : Component
    {
        public abstract void OnRandomTick(World world, BlockPos blockPos, BlockState state, Random random);
    }
}
