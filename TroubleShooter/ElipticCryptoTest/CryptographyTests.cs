﻿using System;
using System.Linq;
using System.Text;
using ElipticCurves;

namespace ElipticCryptoTest
{
    public class CryptographyTests
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+ľščťžýáíé=´!@#$%^&*()_+}|\":{><€'÷×ß$Ł<>*ß¤<";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void TestEncryption(ElipticCurve curve, int iterations, Encoding enc)
        {
            ECKeysGenerator keyGen = new ECKeysGenerator(curve);
            ECEncryption crypto = new ECEncryption(curve);
            //generovanie klucoveho paru
            byte[] privateKey;
            byte[] publicKey;
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"encrtyption curve {curve.Name} test {i}... ");
                keyGen.GenerateKeyPair(out privateKey, out publicKey);
                string test = RandomString(random.Next(100));
                byte[] cipher = crypto.Encrypt(test, publicKey, enc);
                Console.WriteLine(Convert.ToBase64String(cipher));
                string decoded = crypto.Decrypt(cipher, privateKey, enc);
                if (test != decoded)
                    throw new Exception("Fatal Error");
                Write("OK", ConsoleColor.Green);
            }
        }

        private void Write(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
