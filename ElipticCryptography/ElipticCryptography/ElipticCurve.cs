using ModuloCalculation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ElipticCryptography
{
    public class ElipticCurve
    {
        /// <summary>
        /// Integer prime number defined by Fp.
        /// </summary>
        public BigInteger P { get; }

        /// <summary>
        /// The eliptic curve definition parameters. 
        /// </summary>
        public BigInteger A {get;}

        /// <summary>
        /// The eliptic curve definition parameters. 
        /// </summary>
        public BigInteger B { get; }

        /// <summary>
        /// Generartor - point of eliptic curve. 
        /// </summary>
        public ElipticCurvePoint G { get; }

        /// <summary>
        /// G point place value. The smallest n -> n * G = infPoint
        /// </summary>
        public BigInteger N { get; }

        /// <summary>
        /// Cofactor h = #E(F)/n
        /// #E = number of points in eliptic curve E
        /// </summary>
        public BigInteger H { get; }

        /// <summary>
        /// The bitsize of eliptic curve.
        /// </summary>
        public int BitSize { get; }

        private ElipticCurve(BigInteger p, BigInteger a, BigInteger b, ElipticCurvePoint g, BigInteger n, BigInteger h, int bitSize)
        {
            P = p;
            A = a;
            B = b;
            if (!new ECCProvider(this).IsCurvePoint(g))
                throw new ArgumentException("Invalid generator point. curve does not contains defined generator point.");
            G = g;
            N = n; //skontroluj bod 3 v diplomovke aj s G
            H = h;
            BitSize = bitSize;
        }

        public static ElipticCurve TestCurve()
        {
            return new ElipticCurve(23, 1, 1, new ElipticCurvePoint(13, 16), 7, 4, 20);            
        }

        public static ElipticCurve Secp112r1()
        {
            return new ElipticCurve(
                    BigInteger.Parse("0DB7C2ABF62E35E668076BEAD208B", NumberStyles.HexNumber),
                    BigInteger.Parse("0DB7C2ABF62E35E668076BEAD2088", NumberStyles.HexNumber),
                    BigInteger.Parse("0659EF8BA043916EEDE8911702B22", NumberStyles.HexNumber),
                    new ElipticCurvePoint(
                        BigInteger.Parse("009487239995A5EE76B55F9C2F098", NumberStyles.HexNumber),
                        BigInteger.Parse("0A89CE5AF8724C0A23E0E0FF77500", NumberStyles.HexNumber)
                    ),
                    BigInteger.Parse("0DB7C2ABF62E35E7628DFAC6561C5", NumberStyles.HexNumber),
                    BigInteger.Parse("001", NumberStyles.HexNumber),
                    112
                );
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("P:{0}", P));
            sb.AppendLine(string.Format("A:{0}", A));
            sb.AppendLine(string.Format("B:{0}", B));
            sb.AppendLine(string.Format("Gx:{0}", G.X));
            sb.AppendLine(string.Format("Gy:{0}", G.Y));
            sb.AppendLine(string.Format("HexG:{0}", new ECCProvider(this).ToHex(G)));
            sb.AppendLine(string.Format("N:{0}", N));
            sb.AppendLine(string.Format("H:{0}", H));
            sb.AppendLine(string.Format("KeyBitSize:{0}", BitSize));
            return sb.ToString();
        }


    }
}
