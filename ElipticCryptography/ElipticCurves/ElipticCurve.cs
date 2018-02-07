using System.Globalization;
using System.Numerics;
using System.Text;

namespace ElipticCurves
{
    public class ElipticCurve
    {
        /// <summary>
        /// Integer prime number defined by Fp.
        /// </summary>
        public BigInteger P { get; private set; }

        /// <summary>
        /// The eliptic curve definition parameters. 
        /// </summary>
        public BigInteger A { get; private set; }

        /// <summary>
        /// The eliptic curve definition parameters. 
        /// </summary>
        public BigInteger B { get; private set; }

        /// <summary>
        /// Generartor - point of eliptic curve. 
        /// </summary>
        public ElipticCurvePoint G { get; private set; }

        /// <summary>
        /// G point place value. The smallest n -> n * G = infPoint
        /// </summary>
        public BigInteger N { get; private set; }

        /// <summary>
        /// Cofactor h = #E(F)/n
        /// #E = number of points in eliptic curve E
        /// </summary>
        public BigInteger H { get; private set; }

        /// <summary>
        /// The bitsize of eliptic curve.
        /// </summary>
        public int BitSize { get; private set; }

        /// <summary>
        /// the curve name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// set parameters of eliptic curve
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <param name="n"></param>
        /// <param name="h"></param>
        /// <param name="bitSize"></param>
        private ElipticCurve(BigInteger p, BigInteger a, BigInteger b, ElipticCurvePoint g, BigInteger n, BigInteger h, int bitSize, string name)
        {
            P = p;
            A = a;
            B = b;
            G = g;
            N = n; //skontroluj bod 3 v diplomovke aj s G
            H = h;
            BitSize = bitSize;
            Name = name;
        }

        /// <summary>
        /// Test curve or developement purposes
        /// </summary>
        /// <returns></returns>
        public static ElipticCurve TestCurve()
        {
            return new ElipticCurve(23, 1, 1, new ElipticCurvePoint(13, 16), 7, 4, 20, "example");            
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
                    112, "Secp112r1"
                );
        }

        public static ElipticCurve secp160r1()
        {
            return new ElipticCurve(
                    BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFF", NumberStyles.HexNumber),
                    BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFC", NumberStyles.HexNumber),
                    BigInteger.Parse("01C97BEFC54BD7A8B65ACF89F81D4D4ADC565FA45", NumberStyles.HexNumber),
                    new ElipticCurvePoint(
                        BigInteger.Parse("04A96B5688EF573284664698968C38BB913CBFC82", NumberStyles.HexNumber),
                        BigInteger.Parse("023A628553168947D59DCC912042351377AC5FB32", NumberStyles.HexNumber)
                    ),
                    BigInteger.Parse("00100000000000000000001F4C8F927AED3CA752257", NumberStyles.HexNumber),
                    BigInteger.Parse("001", NumberStyles.HexNumber),
                    160, "secp160r1"
                );
        }

        public static ElipticCurve secp192r1()
        {
            return new ElipticCurve(
                    BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFF", NumberStyles.HexNumber),
                    BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFC", NumberStyles.HexNumber),
                    BigInteger.Parse("064210519E59C80E70FA7E9AB72243049FEB8DEECC146B9B1", NumberStyles.HexNumber),
                    new ElipticCurvePoint(
                        BigInteger.Parse("0188DA80EB03090F67CBF20EB43A18800F4FF0AFD82FF1012", NumberStyles.HexNumber),
                        BigInteger.Parse("007192B95FFC8DA78631011ED6B24CDD573F977A11E794811", NumberStyles.HexNumber)
                    ),
                    BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFF99DEF836146BC9B1B4D22831", NumberStyles.HexNumber),
                    BigInteger.Parse("001", NumberStyles.HexNumber),
                    192, "secp192r1"
                );
        }

        public static ElipticCurve secp256r1()
        {
            return new ElipticCurve(
                    BigInteger.Parse("0FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF", NumberStyles.HexNumber),
                    BigInteger.Parse("0FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC", NumberStyles.HexNumber),
                    BigInteger.Parse("05AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B", NumberStyles.HexNumber),
                    new ElipticCurvePoint(
                        BigInteger.Parse("06B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C296", NumberStyles.HexNumber),
                        BigInteger.Parse("04FE342E2FE1A7F9B8EE7EB4A7C0F9E162BCE33576B315ECECBB6406837BF51F5", NumberStyles.HexNumber)
                    ),
                    BigInteger.Parse("0FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551", NumberStyles.HexNumber),
                    BigInteger.Parse("001", NumberStyles.HexNumber),
                    256, "secp256r1"
                );
        }

        public static ElipticCurve secp521r1()
        {
            return new ElipticCurve(
                    BigInteger.Parse("001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", NumberStyles.HexNumber),
                    BigInteger.Parse("001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC", NumberStyles.HexNumber),
                    BigInteger.Parse("00051953EB9618E1C9A1F929A21A0B68540EEA2DA725B99B315F3B8B489918EF109E156193951EC7E937B1652C0BD3BB1BF073573DF883D2C34F1EF451FD46B503F00", NumberStyles.HexNumber),
                    new ElipticCurvePoint(
                        BigInteger.Parse("000C6858E06B70404E9CD9E3ECB662395B4429C648139053FB521F828AF606B4D3DBAA14B5E77EFE75928FE1DC127A2FFA8DE3348B3C1856A429BF97E7E31C2E5BD66", NumberStyles.HexNumber),
                        BigInteger.Parse("0011839296A789A3BC0045C8A5FB42C7D1BD998F54449579B446817AFBD17273E662C97EE72995EF42640C550B9013FAD0761353C7086A272C24088BE94769FD16650", NumberStyles.HexNumber)
                    ),
                    BigInteger.Parse("001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA51868783BF2F966B7FCC0148F709A5D03BB5C9B8899C47AEBB6FB71E91386409", NumberStyles.HexNumber),
                    BigInteger.Parse("001", NumberStyles.HexNumber),
                    528, "secp521r1"
                );
        }

        /// <summary>
        /// Gets eliptic curve info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Name:{0}", Name));
            sb.AppendLine(string.Format("P:{0}", P));
            sb.AppendLine(string.Format("A:{0}", A));
            sb.AppendLine(string.Format("B:{0}", B));
            sb.AppendLine(string.Format("Gx:{0}", G.X));
            sb.AppendLine(string.Format("Gy:{0}", G.Y));
            sb.AppendLine(string.Format("N:{0}", N));
            sb.AppendLine(string.Format("H:{0}", H));
            sb.AppendLine(string.Format("KeyBitSize:{0}", BitSize));
            return sb.ToString();
        }
    }
}
