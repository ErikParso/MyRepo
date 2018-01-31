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
    public class ECCProvider
    {
        private ElipticCurve curve;

        public ECCProvider(ElipticCurve curve)
        {
            this.curve = curve;
        }

        private ElipticCurvePoint Add(ElipticCurvePoint p, ElipticCurvePoint q)
        {
            ModNum.MODULO = curve.P;
            ModNum xr;
            ModNum yr;
            ModNum s;

            if (p == null)
                return q;
            if (q == null)
                return p;
            if (AreInverse(p, q))
                return null;

            ModNum px = new ModNum(p.X);
            ModNum py = new ModNum(p.Y);
            ModNum qx = new ModNum(q.X);
            ModNum qy = new ModNum(q.Y);
            ModNum a = new ModNum(curve.A);
            if (p.Equals(q))
            {
                s = (new ModNum(3) * px * px + a) / (new ModNum(2) * py);
                xr = s * s - px - px;
                yr = s * (px - xr) - py;
            }
            else
            {
                s = (qy - py) / (qx - px);
                xr = s * s - px - qx;
                yr = s * (px - xr) - py;
            }
            return CreatePoint(xr, yr);
        }

        private ElipticCurvePoint Inverse(ElipticCurvePoint point)
        {
            ModNum.MODULO = curve.P;
            ModNum x = new ModNum(point.X);
            ModNum y = new ModNum(point.Y);
            return CreatePoint(x, y.Negate());
        }

        public ElipticCurvePoint Multiply(BigInteger times, ElipticCurvePoint point)
        {
            ModNum.MODULO = curve.P;
            if (times == 1)
                return point;
            ElipticCurvePoint splited = Multiply(times / 2, point);
            ElipticCurvePoint added = Add(splited, splited);
            if (!times.IsEven)
                added = Add(added, point);
            return added;
        }

        public bool IsCurvePoint(ElipticCurvePoint point)
        {
            ModNum.MODULO = curve.P;
            if (point == null)
                return true;
            return new ModNum(point.Y * point.Y).Equals(new ModNum(point.X * point.X * point.X + curve.A * point.X + curve.B));
        }

        public void GenerateKeyPair(out BigInteger privateKey, out string publicKey)
        {
            ModNum.MODULO = curve.P;
            privateKey = new Random().Next() * curve.N / int.MaxValue; // vygenreuj nahodne 1..N-1 , zatial pre testovanie takto;
            publicKey = ToHex(Multiply(privateKey, curve.G));
        }

        public ElipticCurvePoint[] Encrypt(ElipticCurvePoint message, string publicKey)
        {
            ModNum.MODULO = curve.P;
            ElipticCurvePoint Q = FromHex(publicKey);
            int k = new Random().Next(1, (int)curve.N);
            ElipticCurvePoint C1 = Multiply(k, curve.G);
            ElipticCurvePoint C2 = Add(message, Multiply(k, Q));
            return new ElipticCurvePoint[] { C1, C2 };
        }

        public ElipticCurvePoint Decrypt(ElipticCurvePoint[] cipher, BigInteger privateKey)
        {
            ModNum.MODULO = curve.P;
            return Add(cipher[1], Inverse(Multiply(privateKey, cipher[0])));
        }

        private bool AreInverse(ElipticCurvePoint point1, ElipticCurvePoint point2)
        {
            return point1.X == point2.X && point1.Y + point2.Y == curve.P;
        }

        public ElipticCurvePoint CreatePoint(ModNum X, ModNum Y)
        {
            ModNum.MODULO = curve.P;
            return new ElipticCurvePoint(X.Number, Y.Number);
        }

        public string ToHex(ElipticCurvePoint point, bool compress = false)
        {
            if (!compress)
            {
                string xhex = point.X.ToString("X").TrimStart('0').PadLeft(curve.BitSize / 4, '0');
                string yhex = point.Y.ToString("X").TrimStart('0').PadLeft(curve.BitSize / 4, '0');
                return string.Format("04{0}{1}", xhex, yhex);
            }
            else
                throw new NotSupportedException();
        }

        public ElipticCurvePoint FromHex(string hexString)
        {
            string xhex = "0" + hexString.Substring(2, curve.BitSize / 4);
            string yhex = "0" + hexString.Substring(curve.BitSize / 4 + 2);
            return CreatePoint(new ModNum(BigInteger.Parse(xhex, NumberStyles.HexNumber)), new ModNum(BigInteger.Parse(yhex, NumberStyles.HexNumber)));
        }

        /// <summary>
        /// Creates one eliptic curve point using x coordinate.
        /// If point with x coordinate does not lie on the curve, the excption is thrown.
        /// </summary>
        /// <param name="x">the x coordinate of point</param>
        /// <returns></returns>
        public ElipticCurvePoint[] FindPointByX(ModNum x)
        {
            ModNum.MODULO = curve.P;
            ModNum ySquare = x * x * x + new ModNum(curve.A) * x + new ModNum(curve.B);
            ModNum yRoot = ySquare.Power((curve.P + 1) / 4);
            ElipticCurvePoint result = CreatePoint(x, yRoot);
            if (IsCurvePoint(result))
                return new ElipticCurvePoint[] { result, Inverse(result) };
            else
                throw new ArgumentException("No point with x = " + x + " coordinate lies on the curve.");
        }

        public BigInteger[] Signature(BigInteger privateKey, string message)
        {
            ModNum.MODULO = curve.P;
            BigInteger e = message.GetHashCode();
            int k = new Random().Next(1, (int)curve.N);
            BigInteger r = Multiply(k, curve.G).X;
            throw new NotImplementedException();
        }
    }
}
