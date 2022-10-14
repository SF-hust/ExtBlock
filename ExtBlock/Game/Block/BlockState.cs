﻿using ExtBlock.Core.State;

namespace ExtBlock.Game
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
