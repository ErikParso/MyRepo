using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ModuloCalculation
{
    public class ModNum
    {
        public static BigInteger MODULO = 28;

        public BigInteger Number { get; }

        public ModNum(BigInteger number)
        {
            this.Number = (number % MODULO + MODULO) % MODULO;
        }

        public static ModNum operator +(ModNum num1, ModNum num2)
        {
            return new ModNum(num1.Number + num2.Number);
        }

        public static ModNum operator -(ModNum num1, ModNum num2)
        {
            return new ModNum(num1.Number - num2.Number);
        }

        public static ModNum operator *(ModNum num1, ModNum num2)
        {
            return new ModNum(num1.Number * num2.Number);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception> 
        public static ModNum operator /(ModNum num1, ModNum num2)
        {
            ModNum pom = num2.Inverse();
            if (pom == null)
                throw new InvalidOperationException(String.Format("Division by {0} is undefined in modulo {1}.", num2, MODULO));
            return num1 * num2.Inverse();
        }

        public ModNum Negate()
        {
            return this * new ModNum(-1);
        }

        /// <summary>
        /// Gets inverse number of this.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Inverse does not exists.</exception> 
        public ModNum Inverse()
        {
            BigInteger x, y;
            BigInteger g = gcdExtended(Number, MODULO, out x, out y);
            if (g != 1)
                return null;
            else
            {
                return new ModNum(x);
            }
        }
        
        private static BigInteger gcdExtended(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            // Base Case
            if (a == 0)
            {
                x = 0; y = 1;
                return b;
            }
            // To store results of recursive call
            BigInteger x1, y1;
            BigInteger gcd = gcdExtended(b % a, a, out x1, out y1);
            // Update x and y using results of recursive call
            x = y1 - (b / a) * x1;
            y = x1;
            return gcd;
        }

        public override string ToString()
        {
            return Number.ToString();
        }

        public override bool Equals(object obj)
        {
            return ((ModNum)obj).Number == Number;
        }

        public ModNum Power(BigInteger power)
        {
            return new ModNum(BigInteger.ModPow(this.Number, power, MODULO));
            //if (power == 1)
            //    return this;
            //ModNum splited = this.Power(power / 2);
            //ModNum multiplied = splited * splited;
            //if (!power.IsEven)
            //    multiplied = multiplied * this;
            //return multiplied;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }
}
