using ModuloCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ElipticCryptography
{
    public class ElipticCurvePoint
    {
        public BigInteger X { get; private set; }

        public BigInteger Y { get; private set; }

        public ElipticCurvePoint(BigInteger x, BigInteger y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            ElipticCurvePoint point = (ElipticCurvePoint)obj;
            return point.X == X && point.Y == Y;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
