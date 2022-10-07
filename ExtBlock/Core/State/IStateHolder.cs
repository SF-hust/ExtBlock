using System.Diagnostics.CodeAnalysis;
using ExtBlock.Core.Property;

namespace ExtBlock.Core.State
{
    public interface IStateHolder
    {
    }

    public interface IStateHolder<O, S> : IStateHolder
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public O Owner { get; }

        public StateProperties Properties { get; }

        public bool SetProperty(IStateProperty property, int valueIndex, [NotNullWhen(true)] out S? state);
        public bool CycleProperty(IStateProperty property, [NotNullWhen(true)] out S? state);
    }
}
