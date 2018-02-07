using System;
using System.Linq;
using System.Numerics;
using ElipticCurves;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eliptic_crypto_test
{
    [TestClass]
    public class DigitalSignatureTests
    {
        private static Random random = new Random(0);
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [TestMethod]
        public void TestSignature112()
        {
            if (!TestSignature(ElipticCurve.Secp112r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestSignature160()
        {
            if (!TestSignature(ElipticCurve.secp160r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestSignature192()
        {
            if (!TestSignature(ElipticCurve.secp192r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestSignature256()
        {
            if (!TestSignature(ElipticCurve.secp256r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestSignature521()
        {
            if (!TestSignature(ElipticCurve.secp521r1()))
                throw new Exception();
        }

        [TestMethod]
        public void TestSignatureTest()
        {
            ECKeysGenerator keyGen = new ECKeysGenerator(ElipticCurve.TestCurve());
            ECSignature signature = new ECSignature(ElipticCurve.TestCurve());
            for (int i = 0; i < 10; i++)
            {
                //generovanie klucoveho paru
                BigInteger privateKey1;
                string publicKey1;
                keyGen.GenerateKeyPair(out privateKey1, out publicKey1);
                BigInteger privateKey2;
                string publicKey2;
                keyGen.GenerateKeyPair(out privateKey2, out publicKey2);

                string str1 = RandomString(random.Next(100));
                string str2 = RandomString(random.Next(100));
                while (str1 == str2)
                    str2 = RandomString(random.Next(100));

                BigInteger[] sign1 = signature.Signature(str1, privateKey1);
                BigInteger[] sign2 = signature.Signature(str2, privateKey1);

                if (!signature.VerifySignature(str1, sign1, publicKey1))
                    throw new Exception();
            }
        }

        private bool TestSignature(ElipticCurve curve)
        {
            ECKeysGenerator keyGen = new ECKeysGenerator(curve);
            ECSignature signature = new ECSignature(curve);
            for (int i = 0; i < 10; i++)
            {
                //generovanie klucoveho paru
                BigInteger privateKey1;
                string publicKey1;
                keyGen.GenerateKeyPair(out privateKey1, out publicKey1);
                BigInteger privateKey2;
                string publicKey2;
                keyGen.GenerateKeyPair(out privateKey2, out publicKey2);

                string str1 = RandomString(random.Next(100));
                string str2 = RandomString(random.Next(100));
                while (str1 == str2)
                    str2 = RandomString(random.Next(100));

                BigInteger[] sign1 = signature.Signature(str1, privateKey1);
                BigInteger[] sign2 = signature.Signature(str2, privateKey1);

                if (!signature.VerifySignature(str1, sign1, publicKey1))
                    return false;
                if (signature.VerifySignature(str1, sign1, publicKey2))
                    return false;
                if (signature.VerifySignature(str2, sign1, publicKey1))
                    return false;
            }
            return true;
        }
    }
}
