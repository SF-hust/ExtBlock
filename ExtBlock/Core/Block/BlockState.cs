using ExtBlock.Core.State;

namespace ExtBlock.Core
{
    public class BlockState : StateHolder<Block, BlockState>
    {
        public Block Block { get => Owner; }

        public static readonly StateDefinition<Block, BlockState>.Builder.StateFactory Factory = Create;

        protected static BlockState Create(Block block, StateProperties properties)
        {
            return new BlockState(block, properties);
        }

        protected BlockState(Block block, StateProperties properties)
            : base(block, properties)
        {
        }
    }
}
