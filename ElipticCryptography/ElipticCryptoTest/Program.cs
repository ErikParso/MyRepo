using ElipticCryptography;
using ModuloCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ElipticCryptoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ElipticCurve curve = ElipticCurve.TestCurve();
            ECCProvider calc = new ECCProvider(curve);


            //generovanie klucoveho paru
            BigInteger d;
            string q;
            calc.GenerateKeyPair(out d, out q);
            Console.WriteLine(string.Format("d:{0}  q:{1}", d, q));

            for (int x = 0; x < 23; x++)
            {
                for (int y = 0; y < 23; y++)
                {
                    //zasifrovanie
                    ElipticCurvePoint M = calc.CreatePoint(new ModNum(x), new ModNum(y)); //sprava zmapovana do tohoto bodu
                    if (calc.IsCurvePoint(M))
                    {
                        ElipticCurvePoint[] cipher = calc.Encrypt(M, q);
                        ElipticCurvePoint decrypted = calc.Decrypt(cipher, d);
                        Console.WriteLine(decrypted);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
