using ModuloExtensions;
using System.Numerics;

namespace ElipticCurves
{
    /// <summary>
    /// provides arithmetic and logic operations over eliptic curve point
    /// </summary>
    public class PointCalculator
    {
        /// <summary>
        /// the eliptic curve
        /// </summary>
        private ElipticCurve curve;

        /// <summary>
        /// creates calculator
        /// </summary>
        /// <param name="curve"></param>
        public PointCalculator(ElipticCurve curve)
        {
            this.curve = curve;
        }

        /// <summary>
        /// adds 2 eliptic curve point
        /// </summary>
        /// <param name="p">point 1</param>
        /// <param name="q">point 2</param>
        /// <returns>eliptic curve point or null if identitt element O</returns>
        internal ElipticCurvePoint Add(ElipticCurvePoint p, ElipticCurvePoint q)
        {
            if (p == null)
                return q;
            if (q == null)
                return p;
            if (AreInverse(p, q))
                return null;

            BigInteger xr;
            BigInteger yr;
            BigInteger s;

            if (p.Equals(q))
            {
                //s = p.X.ModPow(2, curve.P).ModMul(3, curve.P).ModAdd(curve.A, curve.P).ModDiv(p.Y.ModMul(2, curve.P), curve.P);
                s = (BigInteger.ModPow(p.X, 2, curve.P) * 3 + curve.A).ModDiv(p.Y * 2, curve.P);
                //xr = s.ModPow(2, curve.P).ModSub(p.X, curve.P).ModSub(p.X, curve.P);
                xr = (BigInteger.ModPow(s, 2, curve.P) - p.X - p.X).Mod(curve.P);
                //yr = s.ModMul(p.X.ModSub(xr, curve.P), curve.P).ModSub(p.Y, curve.P);
                yr = (s * (p.X - xr) - p.Y).Mod(curve.P);
            }
            else
            {
                s = (q.Y.ModSub(p.Y, curve.P)).ModDiv(q.X.ModSub(p.X, curve.P), curve.P);
                //s = (q.Y - p.Y).ModDiv((q.X - p.X), curve.P); not working
                //xr = s.ModPow(2, curve.P).ModSub(p.X, curve.P).ModSub(q.X, curve.P);
                xr = (BigInteger.ModPow(s, 2, curve.P) - p.X - q.X).Mod(curve.P);
                //yr = s.ModMul(p.X.ModSub(xr, curve.P), curve.P).ModSub(p.Y, curve.P);
                yr = (s * (p.X - xr) - p.Y).Mod(curve.P);
            }
            return new ElipticCurvePoint(xr, yr);
        }

        /// <summary>
        /// gets inverse point to specific point.
        /// </summary>
        /// <param name="point">eliptic curve point</param>
        /// <returns>inverse point</returns>
        internal ElipticCurvePoint Inverse(ElipticCurvePoint point)
        {
            return new ElipticCurvePoint(point.X, point.Y.ModNeg(curve.P));
        }

        /// <summary>
        /// multiplies eliptic curve point X times. Optimised algorhytm.
        /// </summary>
        /// <param name="times">multiplier</param>
        /// <param name="point">liptic curve point</param>
        /// <returns>multiplied eliptic curve point</returns>
        internal ElipticCurvePoint Multiply(BigInteger times, ElipticCurvePoint point)
        {
            if (times == 0)
                return null;
            if (times == 1)
                return point;
            ElipticCurvePoint splited = Multiply(times / 2, point);
            ElipticCurvePoint added = Add(splited, splited);
            if (!times.IsEven)
                added = Add(added, point);
            return added;
        }

        /// <summary>
        /// checks if eliptic curve point lies on curve
        /// </summary>
        /// <param name="p"></param>
        /// <returns>
        /// true - point lies on curve 
        /// false - point doesnt lie on curve
        /// </returns>
        internal bool IsCurvePoint(ElipticCurvePoint p)
        {
            if (p == null)
                return true;
            //return p.Y.ModPow(2, curve.P).ModEquals(p.X.ModPow(3, curve.P).ModAdd(p.X.ModMul(curve.A, curve.P), curve.P).ModAdd(curve.B, curve.P), curve.P);
            return BigInteger.ModPow(p.Y, 2, curve.P) == (BigInteger.ModPow(p.X, 3, curve.P) + p.X * curve.A + curve.B).Mod(curve.P);
        }

        /// <summary>
        /// Checks if 2 points are inverse
        /// </summary>
        /// <param name="point1">point 1</param>
        /// <param name="point2">point 2</param>
        /// <returns></returns>
        internal bool AreInverse(ElipticCurvePoint point1, ElipticCurvePoint point2)
        {
            return point1.X == point2.X && point1.Y + point2.Y == curve.P;
        }

        /// <summary>
        /// Creates one eliptic curve point using x coordinate.
        /// If there is no point with x coordinate null is returned.
        /// </summary>
        /// <param name="x">the x coordinate of point</param>
        /// <returns>Found point or null.</returns>
        internal ElipticCurvePoint FindPointByX(BigInteger x)
        {
            //BigInteger ySquare = x.ModPow(3, curve.P).ModAdd(x.ModMul(curve.A, curve.P), curve.P).ModAdd(curve.B, curve.P);
            BigInteger ySquare = (BigInteger.ModPow(x, 3, curve.P) + curve.A * x + curve.B).Mod(curve.P);
            //BigInteger yRoot = ySquare.ModPow((curve.P + 1) / 4, curve.P);
            BigInteger yRoot = BigInteger.ModPow(ySquare, (curve.P + 1) / 4, curve.P);
            ElipticCurvePoint result = new ElipticCurvePoint(x, yRoot);
            if (IsCurvePoint(result))
                return result;
            else
                return null;
        }
    }
}
