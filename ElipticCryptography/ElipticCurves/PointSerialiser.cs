using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace ElipticCurves
{
    /// <summary>
    /// serisalises eliptic curve points into comprimed hexadecimal form
    /// 0X1234567890ABCDEF... X = 2 => Y EVEN
    ///                       X = 3 => Y ODD
    /// </summary>
    internal class PointSerialiser
    {
        /// <summary>
        /// the curve to store the point against
        /// </summary>
        private ElipticCurve curve;

        /// <summary>
        /// we need calculation over specified curve to deserialise
        /// </summary>
        private PointCalculator calc;

        /// <summary>
        /// see class description
        /// </summary>
        /// <param name="curve"></param>
        internal PointSerialiser(ElipticCurve curve)
        {
            this.curve = curve;
            this.calc = new PointCalculator(curve);
        }

        /// <summary>
        /// Creates comprimed hex number of eliptic curve point
        /// </summary>
        /// <param name="point">point</param>
        /// <returns>hex string</returns>
        internal string ToHex(ElipticCurvePoint point)
        {
            if (point == null)
                return "";
            string xhex = point.X.ToString("X").TrimStart('0').PadLeft(curve.BitSize / 4, '0');
            int parity = 2 + (point.Y.IsEven ? 0 : 1);
            return string.Format("0{0}{1}", parity, xhex);
        }

        /// <summary>
        /// creates eliptic curve point from hex string
        /// </summary>
        /// <param name="hexString">hex string</param>
        /// <returns>eliptic curve point</returns>
        internal ElipticCurvePoint FromHex(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
                return null;
            bool yEven = hexString.Substring(1, 1) == "2";
            string xhex = "0" + hexString.Substring(2, curve.BitSize / 4);
            BigInteger xcord = BigInteger.Parse(xhex, NumberStyles.HexNumber);
            ElipticCurvePoint point = calc.FindPointByX(xcord);
            if (point == null)
                throw new PointNotFoundException();
            if (point.Y.IsEven == yEven)
                return point;
            else
                return calc.Inverse(point);
        }
    }
}
