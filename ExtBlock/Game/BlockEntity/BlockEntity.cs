using System;
using ExtBlock.Core.Component;
using ExtBlock.Math;

namespace ExtBlock.Game.BlockEntity
{
    public class BlockEntity : IComponentHolder
    {
        public BlockEntity()
        {
            _components = new ComponentHolder(this);
        }

        /*
         * 作为 ComponentHolder
         */

        public ComponentHolder Components => _components;
        private readonly ComponentHolder _components;

        /*
         * BlockEntity 自身的属性
         */

        /// <summary>
        /// BlockEntity 所属的 World
        /// </summary>
        public World? World => _world;
        private World? _world;

        /// <summary>
        /// BlockEntity 在 World 中的方块坐标
        /// </summary>
        public BlockPos Pos => _pos;
        private BlockPos _pos;

        public void OnAddToWorld(World world, BlockPos pos)
        {
            _world = world;
            _pos = pos;
        }

        public void OnRemoveFromWorld()
        {
            _world = null;
        }
    }
}
