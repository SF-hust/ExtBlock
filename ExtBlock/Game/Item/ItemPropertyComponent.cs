using System;
using ExtBlock.Core.Component;
using ExtBlock.Core.Property;
using ExtBlock.Utility;

namespace ExtBlock.Game
{
    public class ItemPropertyComponent : Component
    {
        public const string KEY_EXTRA = "extra";

        public const string KEY_STACKABLE = "stackable";
        public const string KEY_BASE_STACK_SIZE = "baseStackSize";

        public readonly bool stackable = true;
        public readonly int baseStackSize = 64;

        public ItemPropertyComponent(Builder builder)
        {
            stackable = builder.stackable;
            baseStackSize = builder.baseStackSize;
        }

        public ItemPropertyComponent()
        {
        }

        public sealed class Builder
        {
            public static Builder Create()
            {
                return new Builder();
            }

            public ItemPropertyComponent Build()
            {
                return new ItemPropertyComponent(this);
            }

            private Builder() { }

            public void SetStackable(bool stackable)
            {
                this.stackable = stackable;
            }

            public void SetBaseStackSize(int size)
            {
                baseStackSize = size;
            }

            public bool stackable = true;
            public int baseStackSize = 64;
        }
    }
}
