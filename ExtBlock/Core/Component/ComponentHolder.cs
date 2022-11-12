using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ExtBlock.Core.Component
{
    /// <summary>
    /// 组件容器, 拥有一个指向所有者的引用, 被添加到本容器的组件其 owner 会被自动设置为容器的 owner, 被移除的组件其 owner 也会被自动设置为 null
    /// </summary>
    public class ComponentHolder
    {
        private readonly Dictionary<Type, IComponent> _componentsByType = new Dictionary<Type, IComponent>();

        private readonly IComponentHolder _owner;

        public ComponentHolder(IComponentHolder owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// 容器中所有组件的集合
        /// </summary>
        public IEnumerable<IComponent> Values => _componentsByType.Values;

        /// <summary>
        /// 尝试向容器中添加某个 component, 若容器中已存在相同类型的组件, 则添加失败
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryAdd(IComponent component)
        {
            component.OnAddTo(_owner);
            return _componentsByType.TryAdd(component.ComponentType, component);
        }

        /// <summary>
        /// 尝试替换容器中的某个 component, 若容器中不存在相同类型的组件, 则替换失败
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryUpdate(IComponent component)
        {
            if(_componentsByType.ContainsKey(component.ComponentType))
            {
                _componentsByType[component.ComponentType].OnRemove();
                _componentsByType[component.ComponentType] = component;
                component.OnAddTo(_owner);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 直接设置某类型的 component, 不管同类组件是否已经在容器中存在
        /// </summary>
        /// <param name="component"></param>
        public void Set(IComponent component)
        {
            if (_componentsByType.TryGetValue(component.ComponentType, out IComponent com))
            {
                if(component == com)
                {
                    return;
                }
                _componentsByType[component.ComponentType] = component;
                component.OnAddTo(_owner);
                com.OnRemove();
            }
            else
            {
                _componentsByType[component.ComponentType] = component;
                component.OnAddTo(_owner);
            }
        }

        /// <summary>
        /// 检测容器中是否存在某类型组件
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public bool Contains(Type componentType)
        {
            return _componentsByType.ContainsKey(componentType);
        }

        /// <summary>
        /// 检测容器中是否有指定组件同类型的组件, 类型的判定以 Component.ComponentType 为准, 注意参数不能为 null
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool Contains(IComponent component)
        {
            return Contains(component.ComponentType);
        }

        /// <summary>
        /// 尝试查找并返回指定类型的 component
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryGet(Type componentType, [NotNullWhen(true)] out IComponent? component)
        {
            return _componentsByType.TryGetValue(componentType, out component);
        }

        /// <summary>
        /// 尝试查找参数所标识类型的 component, 注意这里的类型是 CT 本身, 而非其 ComponentType.
        /// 即存在 B组件, D组件 是 B组件 的 override, 则查找 D组件 应传入 out B component
        /// </summary>
        /// <typeparam name="CT"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryGet<CT>([NotNullWhen(true)] out CT? component)
            where CT : class, IComponent
        {
            component = null;
            if(TryGet(typeof(CT), out IComponent? com) && com is CT c)
            {
                component = c;
            }
            return component != null;
        }

        /// <summary>
        /// 尝试查找参数所标识类型的 component, struct 版本
        /// </summary>
        /// <typeparam name="CT"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryGet<CT>([NotNullWhen(true)] out CT? component)
            where CT : struct, IComponent
        {
            component = null;
            if (TryGet(typeof(CT), out IComponent? com) && com is CT c)
            {
                component = c;
            }
            return component != null;
        }

        /// <summary>
        /// 尝试删除指定类型的 component, 如果不存在则返回 false
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public bool Remove(Type componentType)
        {
            if (_componentsByType.Remove(componentType, out IComponent? com))
            {
                com.OnRemove();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 尝试删除与参数的 ComponentType 类型相同的 component, 注意参数不能为 null
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool Remove(IComponent component)
        {
            return Remove(component.ComponentType);
        }
    }
}
