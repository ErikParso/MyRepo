using ModuloCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            rsa(13169004533, 65537, 6029832903);
            rsa(1690428486610429, 65537, 22496913456008);
            rsa(BigInteger.Parse("56341958081545199783"), 65537, 17014716723435111315);
            rsa(BigInteger.Parse("6120215756887394998931731"), 65537, BigInteger.Parse("5077587957348826939798388"));
            rsa(BigInteger.Parse("514261067785300163931552303017"), 65537, BigInteger.Parse("357341101854457993054768343508"));
            Console.WriteLine("press enter for exit...");
            Console.ReadLine();
        }

        private static void rsa(BigInteger n, BigInteger e, BigInteger y)
        {
            DateTime start = DateTime.Now;
            BigInteger p = phi(n);
            BigInteger d = mulinv(e, p);
            if (BigInteger.ModPow(BigInteger.ModPow(y, d, n), e, n) == y)
                Console.WriteLine(d);
            else
                Console.WriteLine("fail");
            Console.WriteLine("Time: " + (DateTime.Now - start));
        }

        private static long nextd(long e, long d, long p)
        {
            long D = d + 1;
            while ((e * D) % p != 1)
                D++;
            return D;
        }

        private static BigInteger phi(BigInteger number)
        {
            BigInteger NUMBER = number;
            BigInteger result = number;
            BigInteger i = 2;
            while (i * i <= NUMBER)
            {
                if (NUMBER % i == 0)
                {
                    while (NUMBER % i == 0)
                        NUMBER = NUMBER / i;
                    result -= result / i;
                }
                i++;
            }
            if (NUMBER > 1)
                result -= result / NUMBER;
            return result;
        }

        private static BigInteger mulinv(BigInteger b, BigInteger n)
        {
            BigInteger[] gcd = xgcd(b, n);
            if (gcd[0] == 1)
                ModNum.MODULO = n;
            return new ModNum(gcd[1]).Number;
            throw new Exception();
        }

        //https://en.wikibooks.org/wiki/Algorithm_Implementation/Mathematics/Extended_Euclidean_algorithm#Iterative_algorithm_3
        private static BigInteger[] xgcd(BigInteger b, BigInteger n)
        {
            BigInteger X0 = 1;
            BigInteger X1 = 0;
            BigInteger Y0 = 0;
            BigInteger Y1 = 1;

            BigInteger B = b;
            BigInteger N = n;
            BigInteger Q;
            BigInteger pom;

            while (N != 0)
            {
                Q = B / N;

                pom = N;
                N = B % N;
                B = pom;

                pom = X1;
                X1 = X0 - Q * X1;
                X0 = pom;

                pom = Y1;
                Y1 = Y0 - Q * Y1;
                Y0 = pom;
            }
            return new BigInteger[] { B, X0, Y0 };
        }
    }
}
