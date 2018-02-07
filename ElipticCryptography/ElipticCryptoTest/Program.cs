using ElipticCurves;
using System;

namespace ElipticCryptoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            runTest();
            generateKeys();
            Console.ReadLine();
        }

        private static void runTest()
        {
            CryptographyTests ct = new CryptographyTests();
            ct.TestEncryption(ElipticCurve.Secp112r1(), 10);
            ct.TestEncryption(ElipticCurve.secp160r1(), 10);
            ct.TestEncryption(ElipticCurve.secp192r1(), 10);
            ct.TestEncryption(ElipticCurve.secp256r1(), 10);
            ct.TestEncryption(ElipticCurve.secp521r1(), 10);

            DigitalSignatureTests st = new DigitalSignatureTests();
            st.TestSignature(ElipticCurve.Secp112r1(), 10);
            st.TestSignature(ElipticCurve.secp160r1(), 10);
            st.TestSignature(ElipticCurve.secp192r1(), 10);
            st.TestSignature(ElipticCurve.secp256r1(), 10);
            st.TestSignature(ElipticCurve.secp521r1(), 10);
            st.TestSignature(ElipticCurve.TestCurve(), 10);
        }

        private static void generateKeys()
        {
            generateKey(ElipticCurve.Secp112r1());
            generateKey(ElipticCurve.secp160r1());
            generateKey(ElipticCurve.secp192r1());
            generateKey(ElipticCurve.secp256r1());
            generateKey(ElipticCurve.secp521r1());
            generateKey(ElipticCurve.TestCurve());
        }

        private static void generateKey(ElipticCurve curve)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(curve.Name);
            Console.ResetColor();
            string privateKey;
            string publicKey;
            new ECKeysGenerator(curve).GenerateKeyPair(out privateKey, out publicKey);
            Console.WriteLine($"private:{privateKey}");
            Console.WriteLine($"public :{publicKey}");
        }
    }
}
