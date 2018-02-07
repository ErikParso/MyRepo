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
    public class ECKeysGenerator
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
        public ECKeysGenerator(ElipticCurve curve)
        {
            this.curve = curve;
            calculator = new PointCalculator(curve);
            serialiser = new PointSerialiser(curve);
            rnd = new RandomBigNumbers();
        }

        /// <summary>
        /// Generates random key pair.
        /// </summary>
        /// <param name="privateKey">the private key</param>
        /// <param name="publicKey"> the public key</param>
        public void GenerateKeyPair(out BigInteger privateKey, out string publicKey)
        {
            // vygenreuj nahodne 1..N-1
            privateKey = rnd.Next(1, curve.N - 1);
            publicKey = serialiser.ToHex(calculator.Multiply(privateKey, curve.G));
        }
    }
}
