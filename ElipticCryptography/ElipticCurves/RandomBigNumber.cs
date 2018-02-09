using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ElipticCurves
{
    /// <summary>
    /// Randoms over big integer
    /// </summary>
    public class RandomBigNumbers
    {
        /// <summary>
        /// rnd instance
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// cretaes random number in specified interval
        /// </summary>
        /// <param name="min">minimum genersted number</param>
        /// <param name="max">maximum generated number</param>
        /// <returns>generated number</returns>
        public BigInteger Next(BigInteger min, BigInteger max)
        {
            byte[] bytes = max.ToByteArray();
            BigInteger R;
            while (true)
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
                if (R >= min && R <= max)
                    return R;
            }
        }
    }
}
