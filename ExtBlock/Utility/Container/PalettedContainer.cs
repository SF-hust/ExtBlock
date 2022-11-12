using System;
using System.Collections.Generic;

using ExtBlock.Math;

namespace ExtBlock.Utility.Container
{
    public class PalettedContainer<T> where T : class
    {
        private BitStorage _bits;
        private Palette<T> _palette;

        public PalettedContainer(int size, T value, int initPaletteSize = 1)
        {
            _bits = new BitStorage(size, MathHelper.Log2i(MathHelper.Log2i(initPaletteSize)));
            _palette = new Palette<T>(value, initPaletteSize);
        }

        public bool IsSingle => _bits.IsZero;

        /// <summary>
        /// get 不会导致内部状态改变, set 可能导致扩张
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                int id = _bits[index];
                return _palette.ValueFor(id);
            }
            set
            {
                int id = _palette.IdFor(value);
                if(id >= _bits.MaxValue)
                {
                    _bits.Expand();
                }
                _bits[index] = id;
            }
        }

        /// <summary>
        /// 压缩 PalettedContainer, 此操作会清除掉调色盘中未使用的值, 并可能重建 BitStorage
        /// </summary>
        public void Compress()
        {
            for(int i = 0; i < _bits.size; ++i)
            {

            }
        }
    }
}
