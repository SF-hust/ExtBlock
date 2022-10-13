using System;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// Untyped RegistryEntry interface, don't implementing this directly
    /// </summary>
    public interface IRegistryEntry
    {
        /// <summary>
        /// registry entry infomation as Untyped
        /// </summary>
        public RegistryInfo UntypedRegInfo { get; }
    }

    /// <summary>
    /// indicates this class is a registry entry
    /// </summary>
    /// <example>
    /// when want to create a class which is a registry entry,
    /// you need to implementing this interface,
    /// and add following code into your class :
    /// <code>
    /// private RegistryInfo<Block>? _regInfo;
    /// public RegistryInfo<Block> RegInfo { get => _regInfo!; set => _regInfo ??= value; }
    /// public RegistryInfo UntypedRegInfo => _regInfo!;
    /// </code>
    /// </example>
    /// <typeparam name="ET">the class implementing this interface</typeparam>
    public interface IRegistryEntry<ET> : IRegistryEntry
        where ET : class, IRegistryEntry<ET>
    {
        /// <summary>
        /// typed registry entry infomation
        /// </summary>
        public RegistryInfo<ET> RegInfo { get; set; }
    }
}
