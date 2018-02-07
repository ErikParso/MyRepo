using ElipticCurves;
using System;

namespace ElipticCryptoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            runTest();
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
    }
}
