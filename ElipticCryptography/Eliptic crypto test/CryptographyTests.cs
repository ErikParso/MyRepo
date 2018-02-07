using System;
using System.Linq;
using System.Numerics;
using System.Text;
using ElipticCurves;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eliptic_crypto_test
{
    [TestClass]
    public class CryptographyTests
    {
        private static Random random = new Random(0);
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+ľščťžýáíé=´!@#$%^&*()_+\n }|\":{><€'÷×ß$Ł<>*ß¤<";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [TestMethod]
        public void TestEnc112()
        {
            if (!TestEncryption(ElipticCurve.Secp112r1()))
                throw new Exception();

        }

        [TestMethod]
        public void TestEnc160()
        {
            if (!TestEncryption(ElipticCurve.secp160r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestEnc192()
        {
            if (!TestEncryption(ElipticCurve.secp192r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestEnc256()
        {
            if (!TestEncryption(ElipticCurve.secp256r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestEnc521()
        {
            if (!TestEncryption(ElipticCurve.secp521r1()))
                throw new Exception();
        }

        private bool TestEncryption(ElipticCurve curve)
        {
            ECKeysGenerator keyGen = new ECKeysGenerator(curve);
            ECEncryption crypto = new ECEncryption(curve);
            //generovanie klucoveho paru
            BigInteger privateKey;
            string publicKey;
            for (int i = 0; i < 10; i++)
            {
                keyGen.GenerateKeyPair(out privateKey, out publicKey);
                string test = RandomString(random.Next(10));
                string cipher = crypto.Encrypt(test, publicKey, Encoding.Unicode);
                string decoded = crypto.Decrypt(cipher, privateKey, Encoding.Unicode);
                if (test != decoded)
                    return false;
            }
            return true;
        }
    }
}
