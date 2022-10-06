namespace ExtBlock.Core.State
{
    public interface IStateDefiner
    {
    }

    public interface IStateDefiner<O, S> : IStateDefiner
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public StateDefinition<O, S> StateDef { get; }
    }
}
