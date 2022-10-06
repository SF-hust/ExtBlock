using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ExtBlock.Core.Property;

namespace ExtBlock.Core.State
{
    public sealed partial class StateDefinition<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public class Builder
        {
            public delegate S StateFactory(O owner, StatePropertyProvider properties);

            private readonly O _owner;
            private readonly List<KeyValuePair<IStateProperty, object>> _properties = new List<KeyValuePair<IStateProperty, object>>();
            private readonly StateFactory _factory;

            protected Builder(O owner, StateFactory factory)
            {
                _owner = owner;
                _factory = factory;
            }

            public static Builder Create(O owner, StateFactory factory)
            {
                return new Builder(owner, factory);
            }

            public Builder AddProperty(IStateProperty property, object defaultValue)
            {
                if (!property.ValueIsValid(defaultValue))
                {
                    throw new Exception($"In state definition for [{_owner}] : value (= {property.ValueToString(defaultValue)}) is illegal for state property ({property})");
                }
                if (_properties.Exists((v) => v.Key.Equals(property)))
                {
                    throw new Exception($"In state definition for [{_owner}] : state property ({property}) already exists");
                }
                _properties.Add(new KeyValuePair<IStateProperty, object>(property, defaultValue));
                return this;
            }

            public virtual StateDefinition<O, S> Build()
            {
                int stateCount = 1;
                int defaultStateIndex = 0;
                List<int> indexOffsetForProperty = new List<int>(_properties.Count);

                // calculate count of states and the index of the default state
                // calculate index offset in states list for every property
                foreach (var pair in _properties)
                {
                    IStateProperty property = pair.Key;
                    object value = pair.Value;

                    int i = property.GetValueIndex(value);
                    Debug.Assert(i >= 0 && i < property.CountOfValues);
                    indexOffsetForProperty.Add(stateCount);
                    defaultStateIndex += i * stateCount;
                    stateCount *= property.CountOfValues;
                }

                // gen all states
                List<S> states = new List<S>(stateCount);
                StatePropertyGenerator propertyGenerator = new StatePropertyGenerator(_properties);
                for(int i = 0; i < stateCount; ++i)
                {
                    S state = _factory(_owner, propertyGenerator.Next);
                    states.Add(state);
                }

                List<Dictionary<IStateProperty, ReadOnlyCollection<S>>> neighboursForStates = new List<Dictionary<IStateProperty, ReadOnlyCollection<S>>>(stateCount);
                // gen neighbours and followers for every state
                for(int i = 0; i < stateCount; ++i)
                {
                    S state = states[i];

                    // gen neighbours
                    Dictionary<IStateProperty, ReadOnlyCollection<S>> neighbour = new Dictionary<IStateProperty, ReadOnlyCollection<S>>();
                    for(int pi = 0; pi < _properties.Count; ++pi)
                    {
                        IStateProperty property = _properties[pi].Key;
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
                    for (int pi = 0; pi < _properties.Count; ++pi)
                    {
                        IStateProperty property = _properties[pi].Key;
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
                private readonly List<KeyValuePair<IStateProperty, object>> _properties;
                private readonly List<int> valueIndexes;

                public StatePropertyGenerator(List<KeyValuePair<IStateProperty, object>> properties)
                {
                    _properties = properties;
                    valueIndexes = Enumerable.Repeat(0, properties.Count).ToList();
                }

                public StatePropertyProvider Next
                {
                    get
                    {
                        StatePropertyProvider stateProperties = new StatePropertyProvider();
                        for(int i = 0; i < _properties.Count; ++i)
                        {
                            IStateProperty property = _properties[i].Key;
                            bool success = stateProperties.TryAdd(property, property[valueIndexes[i]]);
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
                        if (valueIndexes[i] == _properties[i].Key.CountOfValues)
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
