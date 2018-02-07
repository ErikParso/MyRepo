using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuloExtensions;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ModuloTests()
        {
            bool res = 
                new BigInteger(5).Mod(11) == 5 &&
                new BigInteger(-5).Mod(11) == 6 &&
                new BigInteger(-156).Mod(22) == 20 &&
                new BigInteger(156).Mod(22) == 2 &&
                new BigInteger(0).Mod(17) == 0 &&
                new BigInteger(17).Mod(17) == 0;
            if (!res)
                throw new Exception("test failed");
        }

        [TestMethod]
        public void ModuloAddTests()
        {
            bool res =
                new BigInteger(5).ModAdd(5, 11) == 10 &&
                new BigInteger(-5).ModAdd(-5 ,11) == 1 &&
                new BigInteger(-156).ModAdd(22, 147) == 13 &&          
                new BigInteger(156).ModAdd(78, 5) == 4 &&
                new BigInteger(0).ModAdd(17, 47) == 17 &&
                new BigInteger(17).ModAdd(0, 17) == 0;
            if (!res)
                throw new Exception("test failed");
        }

        [TestMethod]
        public void ModuloSubstractTests()
        {
            bool res =
                new BigInteger(5).ModSub(5, 11) == 0 &&
                new BigInteger(-5).ModSub(-5, 11) == 0 &&
                new BigInteger(-156).ModSub(22, 147) == 116 &&
                new BigInteger(156).ModSub(78, 5) == 3 &&
                new BigInteger(0).ModSub(17, 47) == 30 &&
                new BigInteger(17).ModSub(0, 17) == 0;
            if (!res)
                throw new Exception("test failed");
        }

        [TestMethod]
        public void ModuloMultiplyTests()
        {
            bool res =
                new BigInteger(5).ModMul(5, 11) == 3 &&
                new BigInteger(-5).ModMul(-5, 11) == 3 &&
                new BigInteger(-156).ModMul(22, 147) == 96 &&
                new BigInteger(156).ModMul(78, 5) == 3 &&
                new BigInteger(0).ModMul(17, 47) == 0 &&
                new BigInteger(17).ModMul(0, 17) == 0;
            if (!res)
                throw new Exception("test failed");
        }

        [TestMethod]
        public void ModuloInverseTests()
        {
            bool res =
                new BigInteger(5).ModInv(11) == 9 &&
                new BigInteger(-5).ModInv(11) == 2 &&
                new BigInteger(-156).ModInv(147) == null &&
                new BigInteger(156).ModInv(5) == 1 &&
                new BigInteger(0).ModInv(47) == null;
            if (!res)
                throw new Exception("test failed");
            for (int i = 1; i < 701; i++)
                if (((BigInteger)new BigInteger(i).ModInv(701)).ModMul(i, 701) != 1)
                    throw new Exception("test failed");
        }
    }
}
