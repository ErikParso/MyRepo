using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipherBreak
{
    class Program
    {
        private static long key;
        private static long my_randx;

        private static double bestMatch = double.MaxValue;

        private static long textSize;

        private static Dictionary<char, double> Alphabet;

        static void Main(string[] args)
        {
            //eng
            Alphabet = new Dictionary<char, double>()
            {
                { 'A', 0.0657 }, { 'B', 0.0126 }, { 'C', 0.0399 }, { 'D', 0.0322 }, { 'E', 0.0957 },
                { 'F', 0.0175 }, { 'G', 0.0145 }, { 'H', 0.0404 }, { 'I', 0.0701 }, { 'J', 0.0012 },
                { 'K', 0.0049 }, { 'L', 0.0246 }, { 'M', 0.0231 }, { 'N', 0.0551 }, { 'O', 0.0603 },
                { 'P', 0.0298 }, { 'Q', 0.0005 }, { 'R', 0.0576 }, { 'S', 0.0581 }, { 'T', 0.0842 },
                { 'U', 0.0192 }, { 'V', 0.0081 }, { 'W', 0.0086 }, { 'X', 0.0007 }, { 'Y', 0.0167 },
                { 'Z', 0.0005 }
            };
            //sk
            //Alphabet = new Dictionary<char, double>()
            //{
            //    { 'A', 0.0995 }, { 'B', 0.0118 }, { 'C', 0.0266 }, { 'D', 0.0436 }, { 'E', 0.0698 },
            //    { 'F', 0.0113 }, { 'G', 0.0017 }, { 'H', 0.0175 }, { 'I', 0.0711 }, { 'J', 0.0157 },
            //    { 'K', 0.0406 }, { 'L', 0.0262 }, { 'M', 0.0354 }, { 'N', 0.0646 }, { 'O', 0.0812 },
            //    { 'P', 0.0179 }, { 'Q', 0.0001 }, { 'R', 0.0428 }, { 'S', 0.0463 }, { 'T', 0.0432 },
            //    { 'U', 0.0384 }, { 'V', 0.0314 }, { 'W', 0.0001 }, { 'X', 0.0004 }, { 'Y', 0.0170 },
            //    { 'Z', 0.0175 }
            //};

            string cipher = File.ReadAllText(@"sources/text4_enc.txt");
            setTextSize(cipher);
            for (long i = 0; i <= 217728; i++)
            {
                string plain = decrypt(cipher, i);
                if (frequenceAnalysis(plain))
                {
                    Console.Clear();
                    Console.WriteLine("key:" + i);
                    Console.WriteLine();
                    Console.WriteLine(plain);
                }
            }
            Console.ReadLine();
        }

        private static string decrypt(string cipher, long key)
        {
            my_randx = key;
            StringBuilder plain = new StringBuilder();
            foreach (char c in cipher.ToUpper())
            {
                if (c >= 'A' && c <= 'Z')
                {
                    int ch = c - 'A';
                    int k = (int)(26 * next());
                    int p = (ch + (26 - k)) % 26;
                    plain.Append((char)((int)'A' + p));
                }
                else
                {
                    plain.Append(c);
                }
            }
            return plain.ToString();
        }

        private static bool frequenceAnalysis(string plain)
        {
            double sum = 0;
            foreach (char letter in Alphabet.Keys)
            {
                sum += Math.Pow(plain.Where(c => c == letter).Count() - (Alphabet[letter] * textSize), 2) / (Alphabet[letter] * textSize);
            }
            if (sum < bestMatch)
            {
                bestMatch = sum;
                return true;
            }
            return false;
        }

        private static void setTextSize(string cipher)
        {
            textSize = cipher.ToUpper().Where(c => c <= 'Z' && c >= 'A').Count();
        }

        private static double next()
        {
            my_randx = (84589 * my_randx + 45989) % 217728;
            return (double)my_randx / 217728.0;
        }


    }
}
