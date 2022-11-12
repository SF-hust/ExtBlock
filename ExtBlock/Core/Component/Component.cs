using System;
using System.Diagnostics;

namespace ExtBlock.Core.Component
{
    /// <summary>
    /// 组件的基类, 继承此类以创建新的组件
    /// </summary>
    public abstract class Component : IComponent
    {
        public virtual Type ComponentType => GetType();

        public IComponentHolder? Owner  => _owner;
        private IComponentHolder? _owner = null;

        protected Component()
        {
        }

        public void OnAddTo(IComponentHolder newOwner)
        {
            if(newOwner == _owner)
            {
                return;
            }
            Debug.Assert(_owner == null);
            _owner = newOwner;
        }

        public void OnRemove()
        {
            if(_owner == null)
            {
                return;
            }
            _owner = null;
        }

        public abstract IComponent CloneWithoutOwner();
    }
}
