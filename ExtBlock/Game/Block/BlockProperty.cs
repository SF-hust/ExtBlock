using ExtBlock.Core.Property;

namespace ExtBlock.Game
{
    public class BlockProperty : PropertyTable
    {
        public readonly bool isAir;
        public readonly bool breakable;
        public readonly float baseBreakTime;

        public BlockProperty(Builder builder) : base(builder.custom, false)
        {
            isAir = builder.isAir;
            breakable = builder.breakable;
            baseBreakTime = builder.baseBreakTime;
        }

        public sealed class Builder
        {
            public static Builder Create()
            {
                return new Builder();
            }

            public BlockProperty Build()
            {
                return new BlockProperty(this);
            }

            private Builder() { }

            public Builder SetIsAir(bool isAir)
            {
                this.isAir = isAir;
                return this;
            }

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

            public bool isAir = false;
            public bool breakable = true;
            public float baseBreakTime;

            public PropertyTable custom = new PropertyTable();
        }
    }
}
