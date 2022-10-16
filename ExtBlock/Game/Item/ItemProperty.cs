using System;

using Newtonsoft.Json.Linq;

using ExtBlock.Core.Property;

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

        public ItemProperty(JObject data)
        {
            if (data.TryGetValue(KEY_STACKABLE, out JToken? node))
            {
                if (node.Type != JTokenType.Boolean)
                {
                    throw new ArgumentException("stackable is not Boolean type");
                }
                stackable = (bool)node;
            }
            if (data.TryGetValue(KEY_BASE_STACK_SIZE, out node))
            {
                baseStackSize = (int)node;
            }
            if (data.TryGetValue(KEY_STACKABLE, out node))
            {
                if (node.Type != JTokenType.Object)
                {
                    throw new ArgumentException("extra is not JObject type");
                }
                Extra = new PropertyTable((JObject)node);
            }
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
