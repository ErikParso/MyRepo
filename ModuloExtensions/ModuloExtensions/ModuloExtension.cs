using System;
using System.Numerics;

namespace ModuloExtensions
{
    public static class ModuloExtension
    {
        public static BigInteger Mod(this BigInteger num, BigInteger modulo)
        {
            return (num % modulo + modulo) % modulo;
        }

        public static BigInteger ModAdd(this BigInteger num1, BigInteger num2, BigInteger modulo)
        {
            return (num1 + num2).Mod(modulo);
        }

        public static BigInteger ModSub(this BigInteger num1, BigInteger num2, BigInteger modulo)
        {
            return (num1 - num2).Mod(modulo);
        }

        public static BigInteger ModMul(this BigInteger num1, BigInteger num2, BigInteger modulo)
        {
            return (num1 * num2).Mod(modulo);
        }

        public static BigInteger? ModInv(this BigInteger num1, BigInteger modulo)
        {
            BigInteger x, y;
            BigInteger g = gcdExtended(num1, modulo, out x, out y);
            if (g != 1)
                return null;
            else
                return x.Mod(modulo);
        }

        private static BigInteger gcdExtended(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0; y = 1;
                return b;
            }
            BigInteger gcd = gcdExtended(b % a, a, out BigInteger x1, out BigInteger y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return gcd;
        }

        public static BigInteger ModDiv(this BigInteger num1, BigInteger num2, BigInteger modulo)
        {
            BigInteger? inv = num2.ModInv(modulo);
            if (inv == null)
                throw new InvalidOperationException(String.Format("Division by {0} is undefined in modulo {1}.", num2, modulo));
            return num1.ModMul((BigInteger)inv, modulo);
        }

        public static BigInteger ModNeg(this BigInteger num, BigInteger modulo)
        {
            return num.ModMul(-1, modulo);
        }

        public static bool ModEquals(this BigInteger num1, BigInteger num2, BigInteger modulo)
        {
            return num1.Mod(modulo) == num2.Mod(modulo);
        }

        public static BigInteger ModPow(this BigInteger num, BigInteger pow, BigInteger modulo)
        {
            return BigInteger.ModPow(num, pow, modulo);
        }
    }
}
