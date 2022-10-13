using ExtBlock.Core.Property;

namespace ExtBlock.Core
{
    public class BlockProperty : PropertyTable
    {
        public readonly bool breakable;
        public readonly float baseBreakTime;

        public BlockProperty(Builder builder) : base(builder.custom, false)
        {
            breakable = builder.breakable;
            baseBreakTime = builder.baseBreakTime;
        }

        public sealed class Builder
        {
            public Builder Create()
            {
                return new Builder();
            }

            private Builder() { }

            public Builder SetBreakable(bool breakable)
            {
                this.breakable = breakable;
                return this;
            }

            public Builder SetBaseBreakTime(float time)
            {
                baseBreakTime = time;
                return this;
            }

            /// <summary>
            /// set a custom property's value
            /// </summary>
            /// <param name="property"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public Builder SetCustom(IProperty property, object value)
            {
                custom.Set(property, value);
                return this;
            }

            public bool breakable = true;
            public float baseBreakTime;

            public PropertyTable custom = new PropertyTable();
        }
    }
}
