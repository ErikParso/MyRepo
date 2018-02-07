using ElipticCurves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ElipticCryptoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //create curve 
            ElipticCurve curve = ElipticCurve.secp521r1();
            Encoding encoding = Encoding.UTF32;
            ECKeysGenerator keyGen = new ECKeysGenerator(curve);
            ECEncryption krypto = new ECEncryption(curve);

            //generovanie klucoveho paru
            BigInteger privateKey;
            string publicKey;
            keyGen.GenerateKeyPair(out privateKey, out publicKey);

            string test = "ABCDEFGHIJKLMNOPQRSTUVZ\n1234567890\n!@#$%^&*()\n_ {}|\":\t|\"\"\\|€÷×¤ß$><\n++ščťžýýáíé=´*";
            string cipher = krypto.Encrypt(test, publicKey, encoding);
            string enc = krypto.Decrypt(cipher, privateKey, encoding);
            Console.WriteLine(test);
            Console.WriteLine(enc);
            Console.WriteLine(cipher);

            Console.ReadLine();
        }
    }
}
