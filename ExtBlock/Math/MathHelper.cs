namespace ExtBlock.Math
{
    public static class MathHelper
    {
        public static int DivideDownPositive(int a, int b)
        {
            return a >= 0 ? a / b : (a - b + 1) / b;
        }
    }
}
