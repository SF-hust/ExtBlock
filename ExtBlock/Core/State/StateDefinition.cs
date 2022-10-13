using System.Collections.ObjectModel;

namespace ExtBlock.Core.State
{
    public sealed partial class StateDefinition<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        private readonly O _owner;
        /// <summary>
        /// owner of this StateDefinition
        /// </summary>
        public O Owner => _owner;

        private readonly ReadOnlyCollection<S> _states;
        /// <summary>
        /// all states of the owner
        /// </summary>
        public ReadOnlyCollection<S> States => _states;

        private readonly S _defaultState;

        /// <summary>
        /// default state of the owner
        /// </summary>
        public S DefaultState { get => _defaultState; }

        private StateDefinition(O owner, ReadOnlyCollection<S> states, S defaultState)
        {
            _owner = owner;
            _states = states;
            _defaultState = defaultState;
        }
    }
}
