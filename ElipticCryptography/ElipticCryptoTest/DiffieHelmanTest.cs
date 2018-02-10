﻿using System;
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
                Console.Write($"diffie-helman curve {curve.Name} test {i}... ");
                keyGen.GenerateKeyPair(out privateKeyAlice, out publicKeyAlice);
                keyGen.GenerateKeyPair(out privateKeyBob, out publicKeyBob);
                string commnonAlices = diffie.SharedSecret(privateKeyAlice, publicKeyBob);
                string commnonBobs = diffie.SharedSecret(privateKeyBob, publicKeyAlice);
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
