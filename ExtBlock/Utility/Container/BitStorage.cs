using System;
using System.Diagnostics;

using ExtBlock.Math;

namespace ExtBlock.Utility.Container
{
    /// <summary>
    /// 此结构体
    /// </summary>
    public struct BitStorage
    {
        /// <summary>
        /// 容器所能存储整数的数量
        /// </summary>
        public readonly int size;

        private int _level;
        private int _maxValue;
        private uint[]? _data;

        /*
         * level 与 每个整数存储位数 与 最大存储值 的对应关系:
         * -1 => 0  => 0
         *  0 => 1  => 1
         *  1 => 2  => 3
         *  2 => 4  => 15
         *  3 => 8  => 255
         *  4 => 16 => 65535
         *  5 => 32 => 2147483646 (int.MaxValue - 1)
         *  这样可以避免 palette 下标溢出, 应该不会出现这种情况
         */

        public BitStorage(int size, int initLevel = -1)
        {
            Debug.Assert(size > 0);
            this.size = size;
            _level = ClampLevel(initLevel);
            _maxValue = CalMaxValue(initLevel);
            if(_level >= 0)
            {
                _data = new uint[CalBufferSize(this.size, _level)];
            }
            else
            {
                _data = null;
            }
        }

        /// <summary>
        /// 结构是否存储了全 0 的值
        /// </summary>
        public bool IsZero => _data == null;

        /// <summary>
        /// 此结构能存储的最大值
        /// </summary>
        public int MaxValue => _maxValue;

        /// <summary>
        /// level 为 -1 时, get 会返回 0, set 不会进行任何操作
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int this[int index]
        {
            get
            {
                if (_data == null)
                {
                    return 0;
                }
                return GetValue(_data, index, _level);
            }
            set
            {
                if (_data == null)
                {
                    return;
                }
                SetValue(_data, index, value, _level);
            }
        }

        /// <summary>
        /// level == -1 时, 会返回 0, 且不会改变内部的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetAndSet(int index, int value)
        {
            if (_data == null)
            {
                return 0;
            }
            return GetAndSetValue(_data, index, value, _level);
        }

        /// <summary>
        /// 扩展 BitStorage, 令 level += levelCount
        /// </summary>
        /// <param name="levelCount"></param>
        public void Expand(int levelCount = 1)
        {
            Debug.Assert(_level >= 0);
            if(_level >= 5)
            {
                throw new InvalidOperationException();
            }
            int olevel = _level;
            uint[]? odata = _data;
            _level = ClampLevel(_level + levelCount);
            _maxValue = CalMaxValue(_level);
            _data = new uint[CalBufferSize(size, _level)];
            if(odata != null)
            {
                for(int i = 0; i < size; i++)
                {
                    int value = GetValue(odata, i, olevel);
                    SetValue(_data, i, value, _level);
                }
            }
        }

        public void Encode()
        {

        }

        public void Decode()
        {

        }
        // 以下三个方法的 level 参数不能为负

        private static int GetValue(uint[] data, int index, int level)
        {
            int bitCount = 1 << level;
            int valueMask = (1 << bitCount) - 1;
            int indexMask = bitCount - 1;
            int arrayIndex = index >> (5 - level);
            int bitShift = (index & indexMask) << level;

            uint packed = data[arrayIndex];
            int value = (int)((packed >> bitShift) & valueMask);
            return value;
        }

        private static void SetValue(uint[] data, int index, int level, int value)
        {
            int bitCount = 1 << level;
            int valueMask = (1 << bitCount) - 1;
            int indexMask = bitCount - 1;
            int arrayIndex = index >> (5 - level);
            int bitShift = (index & indexMask) << level;

            uint packed = data[arrayIndex];
            packed &= ~((uint)valueMask << bitShift);
            packed |= ((uint)value & (uint)valueMask) << bitShift;
            data[arrayIndex] = packed;
        }
        
        private static int GetAndSetValue(uint[] data, int index, int level, int value)
        {
            int bitCount = 1 << level;
            int valueMask = (1 << bitCount) - 1;
            int indexMask = bitCount - 1;
            int arrayIndex = index >> (5 - level);
            int bitShift = (index & indexMask) << level;

            uint packed = data[arrayIndex];
            int ovalue = (int)((packed >> bitShift) & valueMask);
            packed &= ~((uint)valueMask << bitShift);
            packed |= ((uint)value & (uint)valueMask) << bitShift;
            data[arrayIndex] = packed;
            return ovalue;
        }

        private static int CalBufferSize(int size, int level)
        {
            return (size * (1 << level) + 31) / 32;
        }

        private static int CalByteSize(int size, int level)
        {
            return (size * (1 << level) + 7) / 8;
        }

        private static int CalMaxValue(int level)
        {
            if(level < 0)
            {
                return 0;
            }
            if(level == 5)
            {
                return int.MaxValue - 1;
            }
            return (1 << (1 << level)) - 1;
        }

        private static int ClampLevel(int olevel)
        {
            return System.Math.Clamp(olevel, -1, 5);
        }
    }
}
