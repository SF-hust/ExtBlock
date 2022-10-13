using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ExtBlock.Core.State.StateProperties;

namespace ExtBlock.Core.State
{
    public sealed partial class StateDefinition<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        /// <summary>
        /// 
        /// </summary>
        public class Builder
        {
            public delegate S StateFactory(O owner, StatePropertyList? properties);

            private readonly O _owner;
            private readonly MutableStatePropertyList _propertyList = new MutableStatePropertyList();
            private readonly StateFactory _factory;

            protected Builder(O owner, StateFactory factory)
            {
                _owner = owner;
                _factory = factory;
            }

            /// <summary>
            /// create a StateDefinition.Builder instance for owner
            /// </summary>
            /// <param name="owner">states' owner</param>
            /// <param name="factory">a delegate to create state with a owner and StateProperty-value list</param>
            /// <returns></returns>
            public static Builder Create(O owner, StateFactory factory)
            {
                return new Builder(owner, factory);
            }

            /// <summary>
            /// add a StateProperty and the default value's index, this method is more efficient and generic
            /// </summary>
            /// <param name="property"></param>
            /// <param name="defaultValueIndex"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public Builder AddPropertyAndIndex(IStateProperty property, int defaultValueIndex)
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
            /// add a StateProperty and the default value, this method is more visual
            /// </summary>
            /// <typeparam name="VT"></typeparam>
            /// <param name="property"></param>
            /// <param name="defaultValue"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public Builder AddPropertyAndValue<VT> (StateProperty<VT> property, VT defaultValue)
            {
                int index = property.GetValueIndex(defaultValue);
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
            /// build all possible states from added StateProperties, if no property is added before, just one state will be created
            /// </summary>
            /// <returns></returns>
            public virtual StateDefinition<O, S> Build()
            {
                int stateCount = 1;
                int defaultStateIndex = 0;
                List<int> indexOffsetForProperty = new List<int>(_propertyList.PropertyCount);

                // calculate count of states and the index of the default state
                // calculate index offset in states list for every property
                foreach (var pair in _propertyList)
                {
                    IStateProperty property = pair.Key;
                    int i = pair.Value;
                    Debug.Assert(i >= 0 && i < property.CountOfValues);
                    indexOffsetForProperty.Add(stateCount);
                    defaultStateIndex += i * stateCount;
                    stateCount *= property.CountOfValues;
                }

                List<S> states;
                // if this owner has no StateProperty defined
                if (stateCount < 1)
                {
                    states = new List<S>(1);
                    S state = _factory(_owner, null);
                    states.Add(state);
                    return new StateDefinition<O, S>(_owner, states.AsReadOnly(), state);
                }

                // gen all states
                states = new List<S>(stateCount);
                StatePropertyGenerator propertyGenerator = new StatePropertyGenerator(_propertyList.PropertyDefinition);
                for(int i = 0; i < stateCount; ++i)
                {
                    S state = _factory(_owner, propertyGenerator.Next.AsImmutable);
                    states.Add(state);
                }

                List<Dictionary<IStateProperty, ReadOnlyCollection<S>>> neighboursForStates = new List<Dictionary<IStateProperty, ReadOnlyCollection<S>>>(stateCount);
                // gen neighbours and followers for every state
                for(int i = 0; i < stateCount; ++i)
                {
                    S state = states[i];

                    // gen neighbours
                    Dictionary<IStateProperty, ReadOnlyCollection<S>> neighbour = new Dictionary<IStateProperty, ReadOnlyCollection<S>>();
                    for(int pi = 0; pi < _propertyList.PropertyCount; ++pi)
                    {
                        IStateProperty property = _propertyList[pi].Key;
                        ReadOnlyCollection<S> neighbourForThisProperty;

                        // optimise: if neighbour list for current property exists, don't create again
                        if(i % (indexOffsetForProperty[pi] * property.CountOfValues) > indexOffsetForProperty[pi])
                        {
                            int index = i - indexOffsetForProperty[pi];
                            neighbourForThisProperty = neighboursForStates[index][property];
                        }
                        // create neighbour list for current property
                        else
                        {
                            List<S> neighbourList = new List<S>(property.CountOfValues);
                            for(int j = 0; j < property.CountOfValues; ++j)
                            {
                                neighbourList.Add(states[i + j * indexOffsetForProperty[pi]]);
                            }
                            neighbourForThisProperty = neighbourList.AsReadOnly();
                        }
                        neighbour.Add(property, neighbourForThisProperty);
                    }

                    // gen followers
                    Dictionary<IStateProperty, S> follower = new Dictionary<IStateProperty, S>();
                    for (int pi = 0; pi < _propertyList.PropertyCount; ++pi)
                    {
                        IStateProperty property = _propertyList[pi].Key;
                        int followStateIndex = (i + indexOffsetForProperty[pi]) % (property.CountOfValues * indexOffsetForProperty[pi]);
                        follower.Add(property, states[followStateIndex]);
                    }

                    // set neighbours and followers
                    state.SetNeighboursAndFollowers(
                        new ReadOnlyDictionary<IStateProperty, ReadOnlyCollection<S>>(neighbour),
                        new ReadOnlyDictionary<IStateProperty, S>(follower));
                }

                // set default state
                S defaultState = states[defaultStateIndex];
                return new StateDefinition<O, S>(_owner, states.AsReadOnly(), defaultState);
            }

            private class StatePropertyGenerator
            {
                private readonly List<IStateProperty> _properties;
                private readonly List<int> valueIndexes;

                public StatePropertyGenerator(List<IStateProperty> properties)
                {
                    _properties = properties;
                    valueIndexes = Enumerable.Repeat(0, properties.Count).ToList();
                }

                public MutableStatePropertyList Next
                {
                    get
                    {
                        MutableStatePropertyList stateProperties = new MutableStatePropertyList();
                        for(int n = 0; n < _properties.Count; ++n)
                        {
                            IStateProperty property = _properties[n];
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
