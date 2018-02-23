using System;

namespace ElipticCurves
{
    public class PointNotFoundException: Exception
    {
    }

    public class InvalidPointFormatException : Exception
    {
        public byte[] BytesToDeserialise { get; private set; }

        public InvalidPointFormatException(string m, byte[] bytesToDeserialise) : base(m)
        {
            BytesToDeserialise = bytesToDeserialise;
        }
    }

    public class InvalidCipherTextexception : Exception
    {
    }
}
