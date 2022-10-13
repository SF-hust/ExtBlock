namespace ExtBlock.Core.State
{
    public interface IStateDefiner
    {
    }

    /// <summary>
    /// a StateDefiner is a State's Owner, for example, Block is a StateDefiner and BlockState is its StateHolder
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IStateDefiner<O, S> : IStateDefiner
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public StateDefinition<O, S> StateDef { get; }
    }
}
