using ElipticCurves;
using Kros.ModuloExtensions;
using System;
using System.Numerics;
using System.Text;

namespace Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            ElipticCurve curve = ElipticCurve.Secp112r1();
            PointCalculator calculator = new PointCalculator(curve);
            PointSerialiser ser = new PointSerialiser(curve);
            ECKeysGenerator gen = new ECKeysGenerator(curve);
            ECEncryption enc = new ECEncryption(curve);
            byte[] publick;
            byte[] privatek;
            gen.GenerateKeyPair(out privatek, out publick);
            write(Encoding.Unicode.GetBytes("cr"));
            write(enc.Encrypt("", publick, Encoding.UTF32));

            //ElipticCurvePoint P = new ElipticCurvePoint(6, 4);
            //ElipticCurvePoint Q = P;

            //for (BigInteger x = 1; x <= 100; x++)
            //{
            //    Console.WriteLine(Q == null ? "O" : Q.ToString());
            //    Q = calculator.Add(Q, P);
            //}



            Console.ReadLine();
        }

        private static void write(byte[] bytes)
        {
            foreach (byte b in bytes)
                Console.Write(b.ToString("X") + " ");
            Console.WriteLine();
        }
    }
}
