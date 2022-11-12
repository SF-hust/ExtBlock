using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ExtBlock.Core.Component
{
    /// <summary>
    /// 组件的接口, 一般继承 Component 类来创建新组件,
    /// 也可以使用结构体继承此接口并实现其中属性和方法, 用于未来可能的 ECS 支持
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Component 的类型, 一个 ComponetHolder 只能含有一个 ComponentType 相同的组件,
        /// 可以把某个组件的 ComponentType 设置为其父类型, 以实现 override
        /// </summary>
        public Type ComponentType => GetType();

        /// <summary>
        /// 拥有此 Component 的游戏对象,
        /// Owner 可以先设为 null, 再赋新值, 以实现对象复用
        /// </summary>
        public IComponentHolder? Owner { get; }

        /// <summary>
        /// 当 component 被添加到某容器中后调用此方法
        /// </summary>
        /// <param name="newOwner"></param>
        public void OnAddTo(IComponentHolder newOwner);

        /// <summary>
        /// 当 component 从某容器中删除后调用此方法
        /// </summary>
        public void OnRemove();

        /// <summary>
        /// 复制自身, 但不复制 owner 信息
        /// </summary>
        /// <returns></returns>
        public IComponent CloneWithoutOwner();
    }
}
