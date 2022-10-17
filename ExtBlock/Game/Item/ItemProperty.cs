using System;
using System.Text.Json.Nodes;

using ExtBlock.Core.Property;
using ExtBlock.Utility;

namespace ExtBlock.Game
{
    public class ItemProperty
    {
        public const string KEY_EXTRA = "extra";

        public const string KEY_STACKABLE = "stackable";
        public const string KEY_BASE_STACK_SIZE = "baseStackSize";

        public readonly bool stackable = true;
        public readonly int baseStackSize = 64;

        public readonly PropertyTable Extra = new PropertyTable();
        public ItemProperty(Builder builder)
        {
            stackable = builder.stackable;
            baseStackSize = builder.baseStackSize;
            Extra = new PropertyTable(builder.extra);
        }

        public ItemProperty(JsonObject data)
        {
            JsonHelper.TryGetBool(data, KEY_STACKABLE, out stackable);
            JsonHelper.TryGetInt(data, KEY_BASE_STACK_SIZE, out baseStackSize);

        }

        public sealed class Builder
        {
            public static Builder Create()
            {
                return new Builder();
            }

            public ItemProperty Build()
            {
                return new ItemProperty(this);
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

            /// <summary>
            /// set a custom property's value
            /// </summary>
            /// <param name="property"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public Builder SetExtra(IProperty property, object value)
            {
                extra.Set(property, value);
                return this;
            }

            public bool stackable = true;
            public int baseStackSize = 64;

            public PropertyTable extra = new PropertyTable();
        }
    }
}
