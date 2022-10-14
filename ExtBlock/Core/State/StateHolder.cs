using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ExtBlock.Core.State
{
    /// <summary>
    /// a StateHolder holds a list of StateProperty and value(or not), and can access StateHolders with same owner
    /// </summary>
    /// <typeparam name="O">owner type</typeparam>
    /// <typeparam name="S">holder type</typeparam>
    public abstract class StateHolder<O, S> : IStateHolder<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        protected StateHolder(O owner, StatePropertyList? properties)
        {
            _owner = owner;
            _propertyList = properties;
        }

        /// <summary>
        /// set neighbours and followers for this, don't call this in your code
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="followers"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetNeighboursAndFollowers(ReadOnlyDictionary<IStateProperty, ReadOnlyCollection<S>> neighbours, ReadOnlyDictionary<IStateProperty, S> followers)
        {
            if(_neighbours != null || _followers != null)
            {
                throw new InvalidOperationException("can't set neighbours and followers for a StateHolder twice");
            }
            Debug.Assert(neighbours != null && followers != null);
            _neighbours = neighbours;
            _followers = followers;
        }

        private readonly O _owner;
        public O Owner { get => _owner; }

        private ReadOnlyDictionary<IStateProperty, ReadOnlyCollection<S>>? _neighbours = null;
        private ReadOnlyDictionary<IStateProperty, S>? _followers = null;

        /// <summary>
        /// get a StateHolder's reference with targetting value set from neighbours,
        /// this will not modify this StateHolder or create new instance
        /// </summary>
        /// <param name="property"></param>
        /// <param name="valueIndex"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetProperty(IStateProperty property, int valueIndex, [NotNullWhen(true)] out S? state)
        {
            if (_propertyList != null &&
                _neighbours != null &&
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
        /// get a StateHolder's reference next to this by a specified property,
        /// this will not modify this StateHolder or create new instance
        /// </summary>
        /// <param name="property"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool CycleProperty(IStateProperty property, [NotNullWhen(true)] out S? state)
        {
            if(_propertyList != null &&
                _followers != null &&
                _followers.TryGetValue(property, out state))
            {
                return true;
            }
            state = null;
            return false;
        }

        private readonly StatePropertyList? _propertyList;
        public StatePropertyList? PropertyList => _propertyList;
    }
}
