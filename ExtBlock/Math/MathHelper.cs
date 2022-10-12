using System;

namespace ExtBlock.Math
{
    public static class MathHelper
    {
        public static int DivideDownPositive(int a, int b)
        {
            return a >= 0 ? a / b : (a - b + 1) / b;
        }

        public static int LownBitOne(int bitCount)
        {
            if (bitCount < 1)
            {
                return 0;
            }
            int i = 1;
            while(bitCount > 1)
            {
                i <<= 1;
                i |= 1;
            }
            return i;
        }

        public static int StorageBitCount(int count)
        {
            int i = 0;
            while(count > 0)
            {
                ++i;
                count >>= 1;
            }
            return i;
        }
    }
}
