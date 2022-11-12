using System;
using System.Collections.Generic;
using System.Text;

namespace ExtBlock.Game.Interaction
{
    public sealed class InteractionResult
    {
        public static InteractionResult SUCCESS = new InteractionResult();
        public static InteractionResult CONSUME = new InteractionResult();
        public static InteractionResult CONSUME_PARTIAL = new InteractionResult();
        public static InteractionResult PASS = new InteractionResult();
        public static InteractionResult FAIL = new InteractionResult();

        private InteractionResult() { }

        public bool ConsumesAction => this == SUCCESS || this == CONSUME || this == CONSUME_PARTIAL;

        public bool ShouldSwing => this == SUCCESS;

        public bool ShouldAwardStats => this == SUCCESS || this == CONSUME;

        public static InteractionResult SidedSuccess(bool pIsClientSide)
        {
            return pIsClientSide ? SUCCESS : CONSUME;
        }
    }
}
