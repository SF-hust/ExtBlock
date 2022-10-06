using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using ExtBlock.Core.Property;

namespace ExtBlock.Core.State
{
    public abstract class StateHolder<O, S> : IStateHolder<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        protected StateHolder(O owner, StatePropertyProvider properties)
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

        public bool SetProperty(IStateProperty property, object value, [NotNullWhen(true)] out S? state)
        {
            if (_neighbours != null && _neighbours.TryGetValue(property, out var states))
            {
                int i = property.GetValueIndex(value);
                if (i >= 0)
                {
                    state = states[i];
                    return true;
                }
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

        private readonly StatePropertyProvider _properties;
        public StatePropertyProvider Properties => _properties;
    }
}
