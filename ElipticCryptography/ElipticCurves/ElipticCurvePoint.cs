using System.Collections.Generic;
using System.Numerics;

namespace ElipticCurves
{
    /// <summary>
    /// Eliptic curve point
    /// </summary>
    public class ElipticCurvePoint
    {
        /// <summary>
        /// x coordinate
        /// </summary>
        public BigInteger X { get; private set; }

        /// <summary>
        /// y coordinate
        /// </summary>
        public BigInteger Y { get; private set; }

        /// <summary>
        /// Creates point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public ElipticCurvePoint(BigInteger x, BigInteger y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// compares 2 points
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ElipticCurvePoint point = (ElipticCurvePoint)obj;
            return point.X == X && point.Y == Y;
        }

        /// <summary>
        /// displays points information
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }

        /// <summary>
        /// points hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + EqualityComparer<BigInteger>.Default.GetHashCode(X);
            hashCode = hashCode * -1521134295 + EqualityComparer<BigInteger>.Default.GetHashCode(Y);
            return hashCode;
        }
    }
}
