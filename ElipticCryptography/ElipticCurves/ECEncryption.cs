using ModuloExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ElipticCurves
{
    public class ECEncryption : ECProvider
    {
        public ECEncryption(ElipticCurve curve) : base(curve)
        {
        }

        // maps single character of plain text to eliptic curve point is not optimal
        // - its slow and ciphertext is large :( needs remake
        // - Koblitz’s Method for Encoding Plaintext
        // - http://informatika.stei.itb.ac.id/~rinaldi.munir/Kriptografi/2014-2015/IJCSE10-02-05-08.pdf
        #region ENCRYPTION - char mapping

        /// <summary>
        /// Encrypts chars of plain text to cipher text.
        /// one char message is represented by 2 eliptic curve points 
        /// in comprimed form after encryption.
        /// </summary>
        /// <param name="plainText">the plain text</param>
        /// <param name="publicKey">the public key</param>
        /// <returns>cipherText</returns>
        public string Encrypt(string plainText, string publicKey)
        {
            string cipher = "";
            ElipticCurvePoint Q = serialiser.FromHex(publicKey);
            foreach (char c in plainText)
            {
                BigInteger k = rnd.Next(1, curve.N - 1);
                ElipticCurvePoint C1 = calculator.Multiply(k, curve.G);
                ElipticCurvePoint C2 = calculator.Add(CharToPoint(c), calculator.Multiply(k, Q));
                cipher += serialiser.ToHex(C1) + serialiser.ToHex(C2);
            }
            return cipher;
        }

        /// <summary>
        /// Decrypts cipherText in plainText
        /// </summary>
        /// <param name="cipher">cipher text</param>
        /// <param name="privateKey">private key</param>
        /// <returns>original plain text</returns>
        public string Decrypt(string cipher, string privateKey)
        {
            BigInteger pk = BigInteger.Parse(privateKey, NumberStyles.HexNumber);
            string message = "";
            foreach (string c in Split(cipher, curve.BitSize / 2 + 4))
            {
                string c1 = c.Substring(0, c.Length / 2);
                string c2 = c.Substring(c.Length / 2);
                ElipticCurvePoint C1 = serialiser.FromHex(c1);
                ElipticCurvePoint C2 = serialiser.FromHex(c2);
                message += PointTochar(calculator.Add(C2, calculator.Inverse(calculator.Multiply(pk, C1))));
            }
            return message;
        }

        /// <summary>
        /// maps single utf-16 code unit to eliptic curve point.
        /// see - standard probabilistic encoding
        /// </summary>
        /// <param name="m">the character to be mapped</param>
        /// <returns>eliptic curve point</returns>
        private ElipticCurvePoint CharToPoint(char m)
        {
            BigInteger k = curve.P / int.MaxValue;
            BigInteger mkplus = k.ModMul(m, curve.P);
            while (true)
            {
                mkplus = mkplus.ModAdd(1, curve.P);
                ElipticCurvePoint mappedPoint = calculator.FindPointByX(mkplus);
                if (mappedPoint != null)
                    return mappedPoint;
            }
        }

        /// <summary>
        /// Gets char from eliptic curve point
        /// see - standard probabilistic encoding
        ///     - Koblitz’s Method for Encoding Plaintext
        ///     - http://informatika.stei.itb.ac.id/~rinaldi.munir/Kriptografi/2014-2015/IJCSE10-02-05-08.pdf
        /// </summary>
        /// <param name="point">eliptic curve point</param>
        /// <returns>character</returns>
        private char PointTochar(ElipticCurvePoint point)
        {
            BigInteger k = curve.P / int.MaxValue;
            return (char)((point.X - 1) / k);
        }

        #endregion

        // maps block of text into eliptic curve point
        // faster safier and more optimal for cipher text size and speed
        // - Proposed Message Mapping Scheme
        // - http://onlinelibrary.wiley.com/doi/10.1002/sec.1702/full
        #region ENCRYPTION - textblocks mappig

        /// <summary>
        /// Encrypts chars of plain text to cipher text.
        /// one block of message text is represented by 2 eliptic curve points 
        /// in comprimed form after encryption.
        /// </summary>
        /// <param name="plainText">the plain text</param>
        /// <param name="publicKey">the public key</param>
        /// <returns>cipherText</returns>
        public string Encrypt(string plainText, string publicKey, Encoding enc)
        {
            int encByteSize = enc.GetBytes("a").Length;
            int M = ((curve.BitSize / 8) - 1) / encByteSize * encByteSize;
            int N = curve.BitSize / 8 - M;

            string cipher = "";
            byte[] plainTextBytes = enc.GetBytes(plainText);
            ElipticCurvePoint Q = serialiser.FromHex(publicKey);

            for (int i = 0; i * M <= plainTextBytes.Length; i++)
            {
                byte[] buffer = new byte[M + N];
                Buffer.BlockCopy(plainTextBytes, i * M, buffer, N, Math.Min(M, plainTextBytes.Length - i * M));
                BigInteger x = new BigInteger(buffer);
                ElipticCurvePoint point = calculator.FindPointByX(x);
                while (point == null)
                    point = calculator.FindPointByX(x++);

                BigInteger k = rnd.Next(1, curve.N - 1);
                ElipticCurvePoint C1 = calculator.Multiply(k, curve.G);
                ElipticCurvePoint C2 = calculator.Add(point, calculator.Multiply(k, Q));
                cipher += serialiser.ToHex(C1) + serialiser.ToHex(C2);
            }
            return cipher;
        }

        /// <summary>
        /// Decrypts cipherText in plainText
        /// </summary>
        /// <param name="cipher">cipher text</param>
        /// <param name="privateKey">private key</param>
        /// <returns>original plain text</returns>
        public string Decrypt(string cipher, string privateKey, Encoding enc)
        {
            BigInteger pk = BigInteger.Parse(privateKey, NumberStyles.HexNumber);
            int encByteSize = enc.GetBytes("a").Length;
            int M = ((curve.BitSize / 8) - 1) / encByteSize * encByteSize;
            int N = curve.BitSize / 8 - M;

            List<byte> xByte = new List<byte>();
            foreach (string c in Split(cipher, curve.BitSize / 2 + 4))
            {
                string c1 = c.Substring(0, c.Length / 2);
                string c2 = c.Substring(c.Length / 2);
                ElipticCurvePoint C1 = serialiser.FromHex(c1);
                ElipticCurvePoint C2 = serialiser.FromHex(c2);
                ElipticCurvePoint m = calculator.Add(C2, calculator.Inverse(calculator.Multiply(pk, C1)));
                xByte.AddRange(m.X.ToByteArray().Skip(N));
                while (xByte.Count % encByteSize != 0)
                {
                    xByte.Add(0);
                }
            }
            return enc.GetString(xByte.ToArray());
        }

        #endregion

        /// <summary>
        /// Splits string to blocks of specified size.\
        /// Used to serialise and deserialise cipher points. 
        /// </summary>
        /// <param name="str">string to be splitted</param>
        /// <param name="chunkSize">the size of block</param>
        /// <returns></returns>
        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
