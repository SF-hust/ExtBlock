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
    /// private RegistryInfo<Registry>? _regInfo;
    /// RegistryInfo<Registry> IRegistryEntry<Registry>.RegInfo { get => _regInfo!; set => _regInfo = value; }
    /// RegistryInfo IRegistryEntry.UntypedRegInfo => _regInfo!;
    /// </code>
    /// </example>
    /// <typeparam name="ET">the class implementing this interface</typeparam>
    public interface IRegistryEntry<ET> : IRegistryEntry
        where ET : class, IRegistryEntry<ET>
    {
        /// <summary>
        /// typed registry entry infomation
        /// </summary>
        public RegistryInfo<ET> RegInfo { get; protected set; }

        /// <summary>
        /// set registry entry infomation, this is only called by registry instance
        /// </summary>
        /// <param name="info">generated registry infomation by registry</param>
        /// <exception cref="InvalidOperationException">when <c>RegInfo</c> is not null</exception>
        public void SetRegistryInfo(RegistryInfo<ET> info)
        {
            if(RegInfo == null)
            {
                RegInfo = info;
                return;
            }
            throw new InvalidOperationException($"Can't set RegInfo for {{{RegInfo}}} twice");
        }
    }
}
