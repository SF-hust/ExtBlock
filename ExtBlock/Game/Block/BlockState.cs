using ExtBlock.Core.State;
using ExtBlock.Core.State.StateProperties;

namespace ExtBlock.Core
{
    public class BlockState : StateHolder<Block, BlockState>
    {
        public Block Block { get => Owner; }

        public static readonly StateDefinition<Block, BlockState>.Builder.StateFactory Factory = Create;

        protected static BlockState Create(Block block, StatePropertyList? properties)
        {
            return new BlockState(block, properties);
        }

        protected BlockState(Block block, StatePropertyList? properties)
            : base(block, properties)
        {
        }
    }
}
