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

        public string SharedSecret(string privateKey, string publicKey)
        {
            BigInteger privateK = BigInteger.Parse(privateKey, NumberStyles.HexNumber);
            ElipticCurvePoint publicK = serialiser.FromHex(publicKey);
            ElipticCurvePoint P = calculator.Multiply(privateK, publicK);
            if (P == null)
                return null;
            else
                return P.X.ToString("X");
        }
    }
}
