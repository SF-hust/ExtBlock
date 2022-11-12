using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ExtBlock.Utility.Container
{
    public class Palette<T>
        where T : class
    {
        private PaletteType _type;
        private readonly T _value;
        private List<T>? _valuesById = null;
        private Dictionary<T, int>? _idsByValue = null;

        public enum PaletteType
        {
            Single,
            Linear,
            Hash
        }

        public Palette(T value, int initCapacity)
        {
            _value = value;
            if(initCapacity <= 1)
            {
                _type = PaletteType.Single;
            }
            else if(initCapacity <= 16)
            {
                _type = PaletteType.Linear;
                _valuesById = new List<T>(16) { value };
            }
            else
            {
                _type = PaletteType.Hash;
                _valuesById = new List<T>(16) { value };
                _idsByValue = new Dictionary<T, int> { { value, 0 } };
            }
        }

        /// <summary>
        /// palette 里已存储的值的数量
        /// </summary>
        public int Size => _valuesById == null ? 1 : _valuesById.Count;

        /// <summary>
        /// 根据 id 查找 value, 若 id 超出范围会抛异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T ValueFor(int id)
        {
            if(_valuesById == null)
            {
                if (id != 0)
                {
                    throw new IndexOutOfRangeException();
                }
                return _value;
            }
            return _valuesById[id];
        }

        /// <summary>
        /// 根据 value 查找 id, 如果 value 不在调色盘中则将其加入调色盘, 这会导致调色盘扩张, 扩张有可能导致调色盘类型改变
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IdFor(T value)
        {
            int id;
            switch(_type)
            {
                case PaletteType.Single:
                    if (_value == value)
                    {
                        id = 0;
                    }
                    else
                    {
                        // 执行扩张, 调色盘由单值变为线性
                        _valuesById = new List<T> (16){ _value, value };
                        id = 1;
                        _type = PaletteType.Single;
                    }
                    break;
                case PaletteType.Linear:
                    id = _valuesById!.IndexOf(value);
                    if (id < 0)
                    {
                        // 执行扩张, 调色盘由线性变为 Hash
                        id = _valuesById.Count;
                        _valuesById.Add(value);
                        _idsByValue = new Dictionary<T, int>();
                        for (int i = 0; i <= id; i++)
                        {
                            _idsByValue[_valuesById[i]] = i;
                        }
                        _type = PaletteType.Hash;
                    }
                    break;
                case PaletteType.Hash:
                    if (!_idsByValue!.TryGetValue(value, out id))
                    {
                        // 执行扩张
                        id = _valuesById!.Count;
                        _valuesById.Add(value);
                        _idsByValue[value] = id;
                    }
                    break;
                default:
                    // 不应该执行到这里
                    throw new Exception("invalid PaletteType");
            }
            return id;
        }
    }
}
