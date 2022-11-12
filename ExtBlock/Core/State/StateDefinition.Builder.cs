using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace ExtBlock.Core.State
{
    public sealed partial class StateDefinition<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public static StateDefinition<O, S> BuildSingle(O owner, StateHolder<O, S>.Factory factory)
        {
            List<S> states;
            states = new List<S>(1);
            S state = factory(owner, null);
            states.Add(state);
            return new StateDefinition<O, S>(owner, states.ToImmutableArray(), state);
        }

        public class Builder
        {

            private readonly O _owner;
            private readonly StatePropertyList _propertyList = new StatePropertyList();
            private readonly StateHolder<O, S>.Factory _factory;

            protected Builder(O owner, StateHolder<O, S>.Factory factory)
            {
                _owner = owner;
                _factory = factory;
            }

            /// <summary>
            /// 创建 Builder
            /// </summary>
            /// <param name="owner">states' owner</param>
            /// <param name="factory">a delegate to create state with a owner and StateProperty-value list</param>
            /// <returns></returns>
            public static Builder Create(O owner, StateHolder<O, S>.Factory factory)
            {
                return new Builder(owner, factory);
            }

            /// <summary>
            /// 添加一个 Property 及其在默认状态中的取值的下标, 推荐使用此版本而非其泛型版本
            /// </summary>
            /// <param name="property"></param>
            /// <param name="defaultValueIndex"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public Builder AddPropertyAndIndex(StateProperty property, int defaultValueIndex)
            {
                if (!property.IndexIsValid(defaultValueIndex))
                {
                    throw new Exception($"In state definition for [{_owner}] :" +
                        $" valueIndex (= {defaultValueIndex}) out of bound for property {property}");
                }
                if (!_propertyList.TryAdd(property, defaultValueIndex))
                {
                    throw new Exception($"In state definition for [{_owner}] :" +
                        $" state property ({property}) already exists");
                }
                return this;
            }

            /// <summary>
            /// 添加一个 Property 及其在默认状态中的取值
            /// </summary>
            /// <typeparam name="VT"></typeparam>
            /// <param name="property"></param>
            /// <param name="defaultValue"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public Builder AddPropertyAndValue<VT> (StateProperty<VT> property, VT defaultValue)
            {
                int index = property.GetIndexByValue(defaultValue);
                if (index == -1)
                {
                    throw new Exception($"In state definition for [{_owner}] :" +
                        $" value (= {property.ValueToString(defaultValue)}) can't be taken by property ({property})");
                }
                if(!_propertyList.TryAdd(property, index))
                {
                    throw new Exception($"In state definition for [{_owner}] :" +
                        $" state property ({property}) already exists");
                }
                return this;
            }

            /// <summary>
            /// 根据各属性的组合构建所有的 State, 并设置好相应的 neighbour 和 follower, 如果没有任何属性则只会构建一个 State
            /// </summary>
            /// <returns></returns>
            public virtual StateDefinition<O, S> Build()
            {
                int stateCount = 1;
                int defaultStateIndex = 0;
                List<int> indexOffsetForProperty = new List<int>(_propertyList.PropertyCount);

                // 计算 State 的数量, 默认 State 在列表中的下标,
                // 以及每个属性顺序变化时对应的 State 在列表中下标的递增值
                foreach (var pair in _propertyList)
                {
                    StateProperty property = pair.Key;
                    int i = pair.Value;
                    Debug.Assert(i >= 0 && i < property.CountOfValues);
                    indexOffsetForProperty.Add(stateCount);
                    defaultStateIndex += i * stateCount;
                    stateCount *= property.CountOfValues;
                }

                List<S> states;
                // 如果只有一个状态
                if (stateCount == 1)
                {
                    states = new List<S>(1);
                    S state = _factory(_owner, null);
                    states.Add(state);
                    return new StateDefinition<O, S>(_owner, states.ToImmutableArray(), state);
                }

                // 创建所有的可能状态
                states = new List<S>(stateCount);
                StatePropertyGenerator propertyGenerator = new StatePropertyGenerator(_propertyList.Properties);
                for(int i = 0; i < stateCount; ++i)
                {
                    S state = _factory(_owner, propertyGenerator.Next.AsImmutable);
                    states.Add(state);
                }

                List<Dictionary<StateProperty, ImmutableArray<S>>> neighboursForStates = new List<Dictionary<StateProperty, ImmutableArray<S>>>(stateCount);
                // 为每个 State 创建对应的 neighbours 和 followers
                for(int i = 0; i < stateCount; ++i)
                {
                    S state = states[i];

                    // 创建 neighbours
                    Dictionary<StateProperty, ImmutableArray<S>> neighbour = new Dictionary<StateProperty, ImmutableArray<S>>();
                    for(int pi = 0; pi < _propertyList.PropertyCount; ++pi)
                    {
                        StateProperty property = _propertyList[pi].Key;
                        ImmutableArray<S> neighbourForProperty;

                        // 如果已经创建过所需的 neighbour 列表了, 直接引用它, 而不是再创建一遍
                        if(i % (indexOffsetForProperty[pi] * property.CountOfValues) > indexOffsetForProperty[pi])
                        {
                            int index = i - indexOffsetForProperty[pi];
                            neighbourForProperty = neighboursForStates[index][property];
                        }
                        // 为一个 property 创建对应的 neighbour 列表
                        else
                        {
                            List<S> neighbourList = new List<S>(property.CountOfValues);
                            for(int j = 0; j < property.CountOfValues; ++j)
                            {
                                neighbourList.Add(states[i + j * indexOffsetForProperty[pi]]);
                            }
                            neighbourForProperty = neighbourList.ToImmutableArray();
                        }
                        neighbour.Add(property, neighbourForProperty);
                    }

                    // 创建 followers
                    Dictionary<StateProperty, S> follower = new Dictionary<StateProperty, S>();
                    for (int pi = 0; pi < _propertyList.PropertyCount; ++pi)
                    {
                        StateProperty property = _propertyList[pi].Key;
                        int followStateIndex = (i + indexOffsetForProperty[pi]) % (property.CountOfValues * indexOffsetForProperty[pi]);
                        follower.Add(property, states[followStateIndex]);
                    }

                    // 设置 neighbours 和 followers
                    state.SetNeighboursAndFollowers(neighbour.ToImmutableDictionary(), follower.ToImmutableDictionary());
                }

                // 获取默认状态
                S defaultState = states[defaultStateIndex];
                return new StateDefinition<O, S>(_owner, states.ToImmutableArray(), defaultState);
            }

            /// <summary>
            /// 用于遍历各个 StateProperty 的不同取值所能组合成的所有 State
            /// </summary>
            private class StatePropertyGenerator
            {
                private readonly List<StateProperty> _properties;
                private readonly List<int> valueIndexes;

                public StatePropertyGenerator(List<StateProperty> properties)
                {
                    _properties = properties;
                    valueIndexes = Enumerable.Repeat(0, properties.Count).ToList();
                }

                public StatePropertyList Next
                {
                    get
                    {
                        StatePropertyList stateProperties = new StatePropertyList();
                        for(int n = 0; n < _properties.Count; ++n)
                        {
                            StateProperty property = _properties[n];
                            bool success = stateProperties.TryAdd(property, valueIndexes[n]);
                            Debug.Assert(success);
                        }
                        UpdateIndexList();
                        return stateProperties;
                    }
                }

                private void UpdateIndexList()
                {
                    bool carry = true;
                    int i = 0;
                    while(carry && i < _properties.Count)
                    {
                        ++valueIndexes[i];
                        carry = false;
                        if (valueIndexes[i] == _properties[i].CountOfValues)
                        {
                            valueIndexes[i] = 0;
                            carry = true;
                        }
                        ++i;
                    }
                }
            }
        }
    }
}
