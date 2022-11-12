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
            return (1<<bitCount) - 1;
        }

        public static int StorageBitCount(int count)
        {
            int i = 0;
            while(count > 1)
            {
                ++i;
                count >>= 1;
            }
            return i;
        }

        public static int Log2i(int n)
        {
            if(n <= 0)
            {
                return -1;
            }
            return Log2i((uint)n);
        }

        public static int Log2i(uint n)
        {
            if (n == 0)
            {
                return -1;
            }
            int i = 0;
            while (n > 1)
            {
                n >>= 1;
                i++;
            }
            return i;
        }
    }
}
