using System;
using ExtBlock.Core.Registry;

namespace ExtBlock.Core.Component
{
    public abstract class ComponentType : IRegistryEntry<ComponentType>
    {
        /*
         * 作为 RegistryEntry
         */

        public RegistryEntryInfo<ComponentType> RegEntryInfo { get => _regInfo!; set => _regInfo ??= value; }
        private RegistryEntryInfo<ComponentType>? _regInfo;
        public Type AsEntryType => typeof(ComponentType);
    }

    public class ComponentType<C> : ComponentType
        where C : IComponent
    {
        private readonly Func<C> _factory;
        private readonly C _defaultValue;

        /// <summary>
        /// 创建一个新的此类型的 component 实例
        /// </summary>
        /// <returns></returns>
        public C CreateInstance()
        {
            return _factory.Invoke();
        }

        /// <summary>
        /// 创建一个此类型的默认值 component 实例
        /// </summary>
        /// <returns></returns>
        public C CreateDefaultInstance()
        {
            return (C)_defaultValue.CloneWithoutOwner();
        }

        public ComponentType(Func<C> factory, C defaultValue)
        {
            _factory = factory;
            _defaultValue = defaultValue;
        }
    }
}
