using System.Diagnostics.CodeAnalysis;
using ExtBlock.Core.Property;

namespace ExtBlock.Core.State
{
    public interface IStateHolder
    {
    }

    public interface IStateHolder<O, S> : IStateHolder, IStatePropertyProvider
        where O : class, IStateDefiner<O, S>
        where S : StateHolder<O, S>
    {
        public O Owner { get; }

        public bool SetProperty(IStateProperty property, object value, [NotNullWhen(true)] out S? state);
        public bool CycleProperty(IStateProperty property, [NotNullWhen(true)] out S? state);
    }
}
