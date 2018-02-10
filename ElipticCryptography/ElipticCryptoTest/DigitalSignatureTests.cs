using System;
using System.Linq;
using System.Numerics;
using ElipticCurves;

namespace ElipticCryptoTest
{
    public class DigitalSignatureTests
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+ľščťžýáíé=´äúň§ô.,! @#$%^&*()_+}{|\":";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void TestSignature(ElipticCurve curve, int iterations)
        {
            ECKeysGenerator keyGen = new ECKeysGenerator(curve);
            ECSignature signature = new ECSignature(curve);
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"signature curve {curve.Name} test {i}... ");
                //generovanie klucoveho paru
                byte[] privateKey1;
                byte[] publicKey1;
                keyGen.GenerateKeyPair(out privateKey1, out publicKey1);
                byte[] privateKey2;
                byte[] publicKey2;
                keyGen.GenerateKeyPair(out privateKey2, out publicKey2);

                string str1 = RandomString(random.Next(100));
                string str2 = RandomString(random.Next(100));
                while (str1 == str2)
                    str2 = RandomString(random.Next(100));

                byte[] sign1 = signature.Signature(str1, privateKey1);
                byte[] sign2 = signature.Signature(str2, privateKey1);

                foreach(byte b in sign1)
                    Console.Write(b.ToString("X"));
                Console.WriteLine();

                if (!signature.VerifySignature(str1, sign1, publicKey1))
                {
                    Write($"Signature should be valid !!! ", ConsoleColor.Red);
                    throw new Exception("Fatal error");
                }
                if (signature.VerifySignature(str1, sign1, publicKey2))
                {
                    Write($"Signature should not be valid because of wrong public key !!!", ConsoleColor.DarkYellow);
                }
                if (signature.VerifySignature(str2, sign1, publicKey1))
                {
                    Console.WriteLine();
                    Write($"Signature should not be valid because of changed message!!!", ConsoleColor.DarkYellow);
                }
                else
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
