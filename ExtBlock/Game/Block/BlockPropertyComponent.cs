using ExtBlock.Core.Component;

namespace ExtBlock.Game.Block
{
    /// <summary>
    /// 方块的基础属性组件, 存储着所有方块共有的属性
    /// </summary>
    public class BlockPropertyComponent : Component
    {
        public readonly bool isAir;
        public readonly bool breakable;
        public readonly float baseBreakTime;

        public BlockPropertyComponent(Builder builder)
        {
            isAir = builder.isAir;
            breakable = builder.breakable;
            baseBreakTime = builder.baseBreakTime;
        }

        public BlockPropertyComponent(BlockPropertyComponent other)
        {
            isAir = other.isAir;
            breakable = other.breakable;
            baseBreakTime = other.baseBreakTime;
        }

        public override IComponent CloneWithoutOwner()
        {
            return new BlockPropertyComponent(this);
        }

        public sealed class Builder
        {
            private Builder() { }

            public static Builder Create()
            {
                return new Builder();
            }

            public BlockPropertyComponent Build()
            {
                return new BlockPropertyComponent(this);
            }

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

            public bool isAir = false;
            public bool breakable = true;
            public float baseBreakTime = 0.0f;
        }
    }
}
