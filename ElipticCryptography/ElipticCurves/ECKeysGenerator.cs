using System.Numerics;

namespace ElipticCurves
{
    /// <summary>
    /// Provides funcionalities over specified eliptic curve
    /// - Digital signature
    /// - Encryption 
    ///     - by char mapping
    ///     - or by text blocks mapping (optimal)
    /// </summary>
    public class ECKeysGenerator : ECProvider
    {
        public ECKeysGenerator(ElipticCurve curve) : base(curve)
        {
        }

        /// <summary>
        /// Generates random key pair.
        /// </summary>
        /// <param name="privateKey">the private key</param>
        /// <param name="publicKey"> the public key</param>
        public void GenerateKeyPair(out byte[] privateKey, out byte[] publicKey)
        {
            // vygenreuj nahodne 1..N-1
            BigInteger pk = rnd.Next(1, curve.N - 1);
            privateKey = pk.ToByteArray();
            publicKey = serialiser.SerialisePoint(calculator.Multiply(pk, curve.G));
        }
    }
}
