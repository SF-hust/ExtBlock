using ExtBlock.Core.Component;
using ExtBlock.Game;
using ExtBlock.Game.Entity;
using ExtBlock.Game.Interaction;
using ExtBlock.Math;

namespace ExtBlock.CoreMod.Block.Components
{
    public abstract class BlockUseComponent : Component
    {
        public abstract InteractionResult OnUse(Entity user, World world, BlockPos blockPos, BlockState state);
    }
}
