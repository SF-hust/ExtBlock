namespace ExtBlock.Core.State
{
    public interface IStateDefiner
    {
    }

    /// <summary>
    /// 需要定义状态 (如 BlockState) 的类需要继承这个接口, 并实现对应接口, 示例见 Block 类
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IStateDefiner<O, S> : IStateDefiner
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public StateDefinition<O, S> StateDefinition { get; set; }
    }
}
