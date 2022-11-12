using System;
using System.Collections.Generic;
using System.Text;

namespace ExtBlock.Core.Component
{
    public interface IComponentHolder
    {
        /// <summary>
        /// 对象所拥有的组件
        /// </summary>
        public ComponentHolder Components { get; }
    }
}
