using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace ElipticCurves
{
    public class ECDiffieHelman : ECProvider
    {
        public ECDiffieHelman(ElipticCurve curve) : base(curve)
        {
        }

        public byte[] SharedSecret(byte[] privateKey, byte[] publicKey)
        {
            BigInteger privateK = new BigInteger(privateKey);
            ElipticCurvePoint publicK = serialiser.DeserialisePoint(publicKey);
            ElipticCurvePoint P = calculator.Multiply(privateK, publicK);
            if (P == null)
                return null;
            else
                return P.X.ToByteArray();
        }
    }
}
