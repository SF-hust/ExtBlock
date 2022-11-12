using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ExtBlock.Core.Registry;

namespace ExtBlock.Core.State
{
    /// <summary>
    /// 一个 StateHolder 对象是 Owner 的一个状态实例, 保存着 Owner 所定义的所有状态属性以及在当前状态下各属性的取值, 示例参照 BlockState
    /// </summary>
    /// <typeparam name="O">Owner 类型</typeparam>
    /// <typeparam name="S">Holder 类型</typeparam>
    public abstract class StateHolder<O, S>
        where O : class, IStateDefiner<O, S>, IRegistryEntry<O>
        where S : StateHolder<O, S>
    {
        /// <summary>
        /// 一个可以根据 owner 和 propertyList 创建相应 State 的工厂委托
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public delegate S Factory(O owner, ImmutableStatePropertyList? properties);

        public StateHolder(O owner, ImmutableStatePropertyList? propertyList)
        {
            _owner = owner;
            this.propertyList = propertyList;
            _constructed = propertyList == null;
            _hashcodeCache = 31 * owner.StateDefinition.GetHashCode() + (propertyList == null ? 0 : propertyList.PackedIndices % 31);
        }

        /// <summary>
        /// State 的属性与取值列表
        /// </summary>
        public readonly ImmutableStatePropertyList? propertyList;

        /// <summary>
        /// 此 State 是否是其 Owner 的唯一 State
        /// </summary>
        public bool IsSingle => propertyList == null;

        /// <summary>
        /// State 的 Owner
        /// </summary>
        public O Owner { get => _owner; }
        private readonly O _owner;

        private ImmutableDictionary<StateProperty, ImmutableArray<S>>? _neighbours = null;
        private ImmutableDictionary<StateProperty, S>? _followers = null;

        private readonly int _hashcodeCache;

        /// <summary>
        /// 指示 State 是否已经构造完成
        /// </summary>
        private bool _constructed;

        /// <summary>
        /// 为
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="followers"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetNeighboursAndFollowers(ImmutableDictionary<StateProperty, ImmutableArray<S>> neighbours, ImmutableDictionary<StateProperty, S> followers)
        {
            if(_constructed)
            {
                throw new InvalidOperationException("can't set neighbours and followers for a StateHolder twice");
            }
            Debug.Assert(neighbours != null && followers != null);
            _constructed = true;
            _neighbours = neighbours;
            _followers = followers;
        }

        /// <summary>
        /// 获取此 State 在指定 property 上取到指定下标的值的 neighbour
        /// </summary>
        /// <param name="property"></param>
        /// <param name="valueIndex"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetProperty(StateProperty property, int valueIndex, [NotNullWhen(true)] out S? state)
        {
            if (_neighbours != null &&
                property.IndexIsValid(valueIndex) &&
                _neighbours.TryGetValue(property, out var states))
            {
                state = states[valueIndex];
                return true;
            }
            state = null;
            return false;
        }

        /// <summary>
        /// 获取此 State 在指定 property 上的 follower
        /// </summary>
        /// <param name="property"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool CycleProperty(StateProperty property, [NotNullWhen(true)] out S? state)
        {
            if(_followers != null && _followers!.TryGetValue(property, out state))
            {
                return true;
            }
            state = null;
            return false;
        }

        public override int GetHashCode()
        {
            return _hashcodeCache;
        }
    }
}
