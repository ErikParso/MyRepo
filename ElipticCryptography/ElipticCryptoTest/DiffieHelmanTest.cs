using System;
using System.Linq;
using System.Numerics;
using System.Text;
using ElipticCurves;

namespace ElipticCryptoTest
{
    public class DiffieHelmanTest
    {
        public void TestDiffieHelman(ElipticCurve curve, int iterations)
        {
            ECKeysGenerator keyGen = new ECKeysGenerator(curve);
            ECDiffieHelman diffie = new ECDiffieHelman(curve);
            //generovanie klucoveho paru
            byte[] privateKeyAlice;
            byte[] publicKeyAlice;
            byte[] privateKeyBob;
            byte[] publicKeyBob;
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"diffie-helman curve {curve.Name} test {i}... ");
                keyGen.GenerateKeyPair(out privateKeyAlice, out publicKeyAlice);
                keyGen.GenerateKeyPair(out privateKeyBob, out publicKeyBob);
                string commnonAlices = Convert.ToBase64String(diffie.SharedSecret(privateKeyAlice, publicKeyBob));
                string commnonBobs = Convert.ToBase64String(diffie.SharedSecret(privateKeyBob, publicKeyAlice));
                Console.WriteLine(commnonAlices);
                if (commnonAlices != commnonBobs) 
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
