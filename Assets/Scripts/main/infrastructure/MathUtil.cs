using System;

namespace main.infrastructure
{
    public static class MathUtil
    {
        public static int SafeMultiply(int a, int b)
        {
            if (a <= 0 || b <= 0)
            {
                throw new ArgumentException("Both numbers must be positive.");
            }

            // Check for int overflow
            if (a > int.MaxValue / b)
            {
                return int.MaxValue;
            }
            
            // Safe to multiply
            return a * b;
        }
    }
}