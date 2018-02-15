using ElipticCurves;
using System;
using System.Numerics;
using System.Text;

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
            int iterations = 10;

            DiffieHelmanTest dht = new DiffieHelmanTest();
            dht.TestDiffieHelman(ElipticCurve.secp521r1(), iterations);
            dht.TestDiffieHelman(ElipticCurve.Secp112r1(), iterations);
            dht.TestDiffieHelman(ElipticCurve.secp160r1(), iterations);
            dht.TestDiffieHelman(ElipticCurve.secp192r1(), iterations);
            dht.TestDiffieHelman(ElipticCurve.secp256r1(), iterations);
            dht.TestDiffieHelman(ElipticCurve.TestCurve(), iterations);

            CryptographyTests ct = new CryptographyTests();
            ct.TestEncryption(ElipticCurve.secp521r1(), iterations, Encoding.UTF32);
            ct.TestEncryption(ElipticCurve.secp521r1(), iterations, Encoding.Unicode);
            ct.TestEncryption(ElipticCurve.Secp112r1(), iterations, Encoding.Unicode);
            ct.TestEncryption(ElipticCurve.secp160r1(), iterations, Encoding.UTF32);
            ct.TestEncryption(ElipticCurve.secp192r1(), iterations, Encoding.UTF32);
            ct.TestEncryption(ElipticCurve.secp256r1(), iterations, Encoding.Unicode);

            DigitalSignatureTests st = new DigitalSignatureTests();
            st.TestSignature(ElipticCurve.secp521r1(), iterations);
            st.TestSignature(ElipticCurve.Secp112r1(), iterations);
            st.TestSignature(ElipticCurve.secp160r1(), iterations);
            st.TestSignature(ElipticCurve.secp192r1(), iterations);
            st.TestSignature(ElipticCurve.secp256r1(), iterations);
            st.TestSignature(ElipticCurve.TestCurve(), iterations);
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
            byte[] privateKey;
            byte[] publicKey;
            new ECKeysGenerator(curve).GenerateKeyPair(out privateKey, out publicKey);
            Console.WriteLine($"private:{Convert.ToBase64String(privateKey)}");
            Console.WriteLine($"public :{Convert.ToBase64String(publicKey)}");
        }
    }
}
