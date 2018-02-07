using ModuloExtensions;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace ElipticCurves
{
    public class ECSignature
    {
        /// <summary>
        /// Eliptic curve parameters stored here.. 
        /// </summary>
        private ElipticCurve curve;

        /// <summary>
        /// Eliptic curve point arithemtics
        /// </summary>
        private PointCalculator calculator;

        /// <summary>
        /// Serialises the eliptic curve point in its comprimed form
        /// </summary>
        private PointSerialiser serialiser;

        /// <summary>
        /// to generate big integers in specified range
        /// </summary>
        private RandomBigNumbers rnd;

        /// <summary>
        /// Creates provider for specified eliptic curve technology.
        /// </summary>
        /// <param name="curve"></param>
        public ECSignature(ElipticCurve curve)
        {
            this.curve = curve;
            calculator = new PointCalculator(curve);
            serialiser = new PointSerialiser(curve);
            rnd = new RandomBigNumbers();
        }

        /// <summary>
        /// Creates signature to specified file using private key
        /// </summary>
        /// <param name="file">File to sign</param>
        /// <param name="privateKey">private key</param>
        /// <returns></returns>
        public string Signature(string message, BigInteger privateKey)
        {
            BigInteger hash = new BigInteger(HashMessage(message));
            BigInteger r, s;

            do
            {
                BigInteger k = rnd.Next(1, curve.N - 1);
                ElipticCurvePoint x1y1 = calculator.Multiply(k, curve.G);
                r = x1y1.X.Mod(curve.N);
                //s = ((BigInteger)k.ModInv(curve.N)).ModMul(hash.ModAdd(privateKey.ModMul(r, curve.N), curve.N), curve.N);
                s = (((BigInteger)k.ModInv(curve.N)) * (hash + privateKey * r)).Mod(curve.N);
            } while (r == 0 || s == 0);

            return r.ToString("X") + " " + s.ToString("X");
            //return new BigInteger[] { r, s };
        }

        /// <summary>
        /// Verifies signature to specified file using public key
        /// </summary>
        /// <param name="file">signed file</param>
        /// <param name="signature">signature</param>
        /// <param name="publicKey">public key</param>
        /// <returns></returns>
        public bool VerifySignature(string message, string signature, string publicKey)
        {
            string[] rs = signature.Split();
            if (rs.Length != 2)
                return false;
            BigInteger r = BigInteger.Parse(rs[0], NumberStyles.HexNumber);
            BigInteger s = BigInteger.Parse(rs[1], NumberStyles.HexNumber);
            //1. public key point isnt identity element O' and coordinates are valid
            //2. public key point lies on curve (exception is thrown if not)
            ElipticCurvePoint publicKeyPoint = serialiser.FromHex(publicKey);
            if (publicKeyPoint == null)
                return false;
            //3. N x publicKey != O
            ElipticCurvePoint zero = calculator.Multiply(curve.N, publicKeyPoint);
            if (zero != null)
                return false;

            //1. r and s in [1, N-1]
            if (r == 0 || r > curve.N - 1)
                return false;
            if (s == 0 || s > curve.N - 1)
                return false;
            //2. calculate hash of message
            BigInteger hash = new BigInteger(HashMessage(message));
            //3. w = 1/s mod N
            BigInteger w = (BigInteger)s.ModInv(curve.N);
            //4. u1 = hash * w mod N
            //   u2 = r * w mod N
            BigInteger u1 = w.ModMul(hash, curve.N);
            BigInteger u2 = w.ModMul(r, curve.N);
            //5. P(x1,y1) = u1 * G + u2 * publicKey mod N
            //   if P(x1,y1) == O => FALSE
            ElipticCurvePoint x1y1 = calculator.Add(calculator.Multiply(u1, curve.G), calculator.Multiply(u2, publicKeyPoint));
            if (x1y1 == null)
                return false;
            //6. r == x1 mod n => TRUE => ELSE FALSE
            if (r.ModEquals(x1y1.X, curve.N))
                return true;
            else
                return false;
        }

        /// <summary>
        /// creates 256SHA hash to message
        /// </summary>
        /// <param name="value">mesage to get hash from</param>
        /// <returns>hash</returns>
        private byte[] HashMessage(string value)
        {
            using (SHA256 hash = SHA256.Create())
            {
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

    }
}
