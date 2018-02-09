using System;
using System.Collections.Generic;
using System.Text;

namespace ElipticCurves
{
    public class ECProvider
    {
        /// <summary>
        /// Eliptic curve parameters stored here.. 
        /// </summary>
        protected ElipticCurve curve;

        /// <summary>
        /// Eliptic curve point arithemtics
        /// </summary>
        protected PointCalculator calculator;

        /// <summary>
        /// Serialises the eliptic curve point in its comprimed form
        /// </summary>
        protected PointSerialiser serialiser;

        /// <summary>
        /// to generate big integers in specified range
        /// </summary>
        protected RandomBigNumbers rnd;

        /// <summary>
        /// Creates provider for specified eliptic curve technology.
        /// </summary>
        /// <param name="curve"></param>
        public ECProvider(ElipticCurve curve)
        {
            this.curve = curve;
            calculator = new PointCalculator(curve);
            serialiser = new PointSerialiser(curve);
            rnd = new RandomBigNumbers();
        }
    }
}
