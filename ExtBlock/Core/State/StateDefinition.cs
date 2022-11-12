using System.Collections.Immutable;

namespace ExtBlock.Core.State
{
    public sealed partial class StateDefinition<O, S>
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        /// <summary>
        /// 拥有此 StateDefinition 的对象
        /// </summary>
        public O Owner => _owner;
        private readonly O _owner;

        private readonly ImmutableArray<S> _states;
        /// <summary>
        /// Owner 的所有可能状态
        /// </summary>
        public ImmutableArray<S> States => _states;

        /// <summary>
        /// 默认状态
        /// </summary>
        public S DefaultState { get => _defaultState; }
        private readonly S _defaultState;

        private StateDefinition(O owner, ImmutableArray<S> states, S defaultState)
        {
            _owner = owner;
            _states = states;
            _defaultState = defaultState;
        }
    }
}
