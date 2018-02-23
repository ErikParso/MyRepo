using System;
using System.Collections.Generic;
using System.Numerics;

namespace ElipticCurves
{
    /// <summary>
    /// serisalises eliptic curve points into comprimed hexadecimal form
    /// 0X1234567890ABCDEF... X = 2 => Y EVEN
    ///                       X = 3 => Y ODD
    /// </summary>
    public class PointSerialiser
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
        /// serialises ec point into byte array over specified eliptic curve.
        /// first byte value meaning:
        /// 0x00 - zero point
        /// 0x04 - uncomprimed
        /// 0x02 - comprimed with even Y
        /// 0x03 - comprimed with odd Y
        /// </summary>
        /// <param name="point">point to serialise</param>
        /// <param name="comprimend">serialised point</param>
        /// <returns></returns>
        internal byte[] SerialisePoint(ElipticCurvePoint point, bool comprimend)
        {
            int size = curve.P.ToByteArray().Length;
            if (comprimend)
            {
                byte[] result = new byte[1 + size];
                if (point == null)
                    return result;
                byte[] xBytes = point.X.ToByteArray();
                if (point.Y.IsEven)
                    result[0] = 0x02;
                else
                    result[0] = 0x03;
                Buffer.BlockCopy(xBytes, 0, result, 1, xBytes.Length);
                return result;
            }
            else
            {
                byte[] result = new byte[1 + size * 2];
                if (point == null)
                    return result;
                byte[] xBytes = point.X.ToByteArray();
                byte[] yBytes = point.Y.ToByteArray();
                result[0] = 0x04;
                Buffer.BlockCopy(xBytes, 0, result, 1, xBytes.Length);
                Buffer.BlockCopy(yBytes, 0, result, 1 + size, yBytes.Length);
                return result;
            }
        }

        /// <summary>
        /// Deserialise ec point over specified eliptic curve
        /// </summary>
        /// <param name="bytes">serialised point</param>
        /// <returns>deserialised point</returns>
        internal ElipticCurvePoint DeserialisePoint(byte[] bytes)
        {
            //this is error
            if (bytes == null)
                throw new InvalidPointFormatException("Cannot convert null to eliptic curve point. Zero point (null) is serialised with prefix 0x00", bytes);
            if (bytes.Length == 0)
                throw new InvalidPointFormatException("Cannot convert empty byte array to eliptic curve point. Zero point (null) is serialised with prefix 0x00", bytes);
            int size = curve.P.ToByteArray().Length;
            //deserialise zero point
            if (bytes[0] == 0x00)
                return null;
            //deserielse uncomprimed point
            if (bytes[0] == 0x04)
            {
                if (bytes.Length != size * 2 + 1)
                    throw new InvalidPointFormatException($"point serialised over curve {curve.Name} should have {size * 2 + 1} bytes in uncomprimed form.", bytes);
                byte[] xBytes = new byte[size];
                byte[] yBytes = new byte[size];
                Buffer.BlockCopy(bytes, 1, xBytes, 0, size);
                Buffer.BlockCopy(bytes, 1 + size, yBytes, 0, size);
                ElipticCurvePoint point = new ElipticCurvePoint(new BigInteger(xBytes), new BigInteger(yBytes));
                if(!calc.IsCurvePoint(point))
                    throw new InvalidPointFormatException("deserialised point was not found on curve...", bytes);
                return point;
            }
            //deserialise comprimed point
            else
            {
                if (bytes.Length != size + 1)
                    throw new InvalidPointFormatException($"point serialised over curve {curve.Name} should have {size + 1} bytes in comprimed form.", bytes);
                bool yEven;
                switch (bytes[0])
                {
                    case 0x02: yEven = true; break;
                    case 0x03: yEven = false; break;
                    default: throw new InvalidPointFormatException("First byte of serialised point should be 0x00 for zero point, 0x04 for uncomprimed, 0x02 for comprimed with even Y and 0x03 for comprimed with odd Y.", bytes);
                }
                byte[] xBytes = new byte[size];
                Buffer.BlockCopy(bytes, 1, xBytes, 0, size);
                BigInteger x = new BigInteger(xBytes);
                ElipticCurvePoint point = calc.FindPointByX(x);
                if (point == null)
                    throw new InvalidPointFormatException("deserialised point was not found on curve...", bytes);
                if (point.Y.IsEven == yEven)
                    return point;
                else
                    return calc.Inverse(point);
            }
        }

        /// <summary>
        /// Serialises sequence of eliptic curve points over specified curve
        /// </summary>
        /// <param name="points">ec points to serielise</param>
        /// <returns>serialised points</returns>
        internal byte[] SerialisePoints(IEnumerable<ElipticCurvePoint> points, bool encrypted)
        {
            List<byte> bytes = new List<byte>();
            foreach (ElipticCurvePoint point in points)
                bytes.AddRange(SerialisePoint(point, encrypted));
            return bytes.ToArray();
        }

        /// <summary>
        /// splits byte sequence and deserielises blocks ito eliptic curve points over specified curve.
        /// </summary>
        /// <param name="bytes">points serielised in bytes</param>
        /// <param name="comprimed">if points are in comprimed form</param>
        /// <returns></returns>
        internal IEnumerable<ElipticCurvePoint> DeserielisePoints(byte[] bytes, bool comprimed)
        {
            int size = curve.P.ToByteArray().Length;
            if (comprimed)
            {
                if (bytes.Length % (size + 1) != 0)
                    throw new InvalidCipherTextexception();
                for (int i = 0; i < bytes.Length; i += size + 1)
                {
                    byte[] buff = new byte[size + 1];
                    Buffer.BlockCopy(bytes, i, buff, 0, size + 1);
                    yield return DeserialisePoint(buff);
                }
            }
            else
            {
                if (bytes.Length % (size * 2 + 1) != 0)
                    throw new InvalidCipherTextexception();
                for (int i = 0; i < bytes.Length; i += size * 2 + 1)
                {
                    byte[] buff = new byte[size * 2 + 1];
                    Buffer.BlockCopy(bytes, i, buff, 0, size * 2 + 1);
                    yield return DeserialisePoint(buff);
                }
            }
        }
    }
}
