using System;

using ExtBlock.Core.Component;

namespace ExtBlock.Game.Entity
{
    public class Entity : IComponentHolder
    {
        public Entity()
        {
            _components = new ComponentHolder(this);
        }

        /*
         * 作为 ComponentHolder
         */

        public ComponentHolder Components => _components;
        private readonly ComponentHolder _components;

        /*
         * Entity 自身的属性
         */

        public Guid Guid => _guid;
        private Guid _guid;
    }
}
