using System.Collections.ObjectModel;

namespace ExtBlock.Core.State
{
    public sealed partial class StateDefinition<O, S> : IStateDefiner<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public StateDefinition<O, S> StateDef => this;

        private readonly O _owner;
        public O Owner => _owner;

        private readonly ReadOnlyCollection<S> _states;
        public ReadOnlyCollection<S> States => _states;

        private readonly S _defaultState;
        public S DefaultState { get => _defaultState; }

        private StateDefinition(O owner, ReadOnlyCollection<S> states, S defaultState)
        {
            _owner = owner;
            _states = states;
            _defaultState = defaultState;
        }
    }
}
