using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ExtBlock.Core.State
{
    public abstract class StateHolder<O, S> : IStateHolder<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        protected StateHolder(O owner, StateProperties properties)
        {
            _owner = owner;
            _properties = properties;
        }

        public void SetNeighboursAndFollowers(ReadOnlyDictionary<IStateProperty, ReadOnlyCollection<S>> neighbours, ReadOnlyDictionary<IStateProperty, S> followers)
        {
            _neighbours = neighbours;
            _followers = followers;
        }

        private readonly O _owner;
        public O Owner { get => _owner; }

        private ReadOnlyDictionary<IStateProperty, ReadOnlyCollection<S>>? _neighbours;
        private ReadOnlyDictionary<IStateProperty, S>? _followers;

        public bool SetProperty(IStateProperty property, int valueIndex, [NotNullWhen(true)] out S? state)
        {
            if (_neighbours != null && property.IndexIsValid(valueIndex) && _neighbours.TryGetValue(property, out var states))
            {
                state = states[valueIndex];
                return true;
            }
            state = null;
            return false;
        }
        public bool CycleProperty(IStateProperty property, [NotNullWhen(true)] out S? state)
        {
            if(_followers != null && _followers.TryGetValue(property, out state))
            {
                return true;
            }
            state = null;
            return false;
        }

        private readonly StateProperties _properties;
        public StateProperties Properties => _properties;
    }
}
