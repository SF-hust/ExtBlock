using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExtBlock.Core.Registry
{
    /// <summary>
    /// 对 RegistryEntry 的 Lazy 包装, 通过此类对象可获取一个指定 id 的 T 类型的 RegistryEntry(也有可能不存在对应的 RegistryEntry 实例导致抛出异常)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RegistryObject<T>
        where T : class, IRegistryEntry<T>
    {
        /// <summary>
        /// 获取真正的 RegistryEntry, 这个委托只能被调用一次
        /// </summary>
        private readonly Func<T?> _getter;
        /// <summary>
        /// 是否已执行 _getter()
        /// </summary>
        private bool _constructed = false;
        private T? _value = null;

        /// <summary>
        /// 获取此 RegistryObject 对应的真正的 RegistryEntry, 注意有可能返回 null
        /// </summary>
        public T Value
        {
            get
            {
                if(!_constructed)
                {
                    _constructed = true;
                    _value = _getter();
                }
                Debug.Assert(_value != null);
                return _value;
            }
        }

        /// <summary>
        /// 查看此 RegistryObject 对应的 RegistryEntry 是否存在
        /// </summary>
        public bool Exist
        {
            get
            {
                if (!_constructed)
                {
                    _constructed = true;
                    _value = _getter();
                }
                return _value != null;
            }
        }

        /// <summary>
        /// 一般而言, Modder 不应该自己构造 RegistryObject, 而是通过 Registry.GetAsRegistryObject() 获取
        /// </summary>
        /// <param name="getter"></param>
        public RegistryObject(Func<T?> getter)
        {
            _getter = getter;
        }
    }
}
