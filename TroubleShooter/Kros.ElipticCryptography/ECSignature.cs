using Kros.ModuloExtensions;
using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace ElipticCurves
{
    public class ECSignature : ECProvider
    {
        public ECSignature(ElipticCurve curve) : base(curve)
        {
        }

        /// <summary>
        /// Creates signature to specified file using private key
        /// </summary>
        /// <param name="file">File to sign</param>
        /// <param name="privateKey">private key</param>
        /// <returns></returns>
        public byte[] Signature(string message, byte[] privateKey)
        {
            BigInteger hash = new BigInteger(HashMessage(message));
            BigInteger r, s;
            BigInteger pk = new BigInteger(privateKey);
            do
            {
                BigInteger k = rnd.Next(1, curve.N - 1);
                ElipticCurvePoint x1y1 = calculator.Multiply(k, curve.G);
                r = x1y1.X.Mod(curve.N);
                //s = ((BigInteger)k.ModInv(curve.N)).ModMul(hash.ModAdd(privateKey.ModMul(r, curve.N), curve.N), curve.N);
                s = (((BigInteger)k.ModInv(curve.N)) * (hash + pk * r)).Mod(curve.N);
            } while (r == 0 || s == 0);

            int size = curve.N.ToByteArray().Length;
            byte[] result = new byte[size * 2];
            byte[] rBytes = r.ToByteArray();
            byte[] sBytes = s.ToByteArray();
            Buffer.BlockCopy(rBytes, 0, result, 0, rBytes.Length);
            Buffer.BlockCopy(sBytes, 0, result, size, sBytes.Length);
            return result;
        }

        /// <summary>
        /// Verifies signature to specified file using public key
        /// </summary>
        /// <param name="file">signed file</param>
        /// <param name="signature">signature</param>
        /// <param name="publicKey">public key</param>
        /// <returns></returns>
        public bool VerifySignature(string message, byte[] signature, byte[] publicKey)
        {
            int size = curve.N.ToByteArray().Length;
            byte[] rBytes = new byte[size];
            byte[] sBytes = new byte[size];
            Buffer.BlockCopy(signature, 0, rBytes, 0, size);
            Buffer.BlockCopy(signature, size, sBytes, 0, size);
            BigInteger r = new BigInteger(rBytes);
            BigInteger s = new BigInteger(sBytes);

            //1. public key point isnt identity element O' and coordinates are valid
            //2. public key point lies on curve (exception is thrown if not)
            ElipticCurvePoint publicKeyPoint = serialiser.DeserialisePoint(publicKey);
            if (publicKeyPoint == null)
                return false;
            //3. N x publicKey == O
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
