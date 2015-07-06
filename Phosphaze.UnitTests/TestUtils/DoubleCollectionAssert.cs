using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Phosphaze.UnitTests.TestUtils
{
    public static class DoubleCollectionAssert
    {

        public static void AreEqual(double[] result, double[] expected, double precision)
        {
            Assert.AreEqual(result.Length, expected.Length);
            for (int i = 0; i < result.Length; i++)
                Assert.AreEqual(result[i], expected[i], precision);
        }

        public static void AreEqual(double[] result, double[] expected, double precision, string message)
        {
            Assert.AreEqual(result.Length, expected.Length, message);
            for (int i = 0; i < result.Length; i++)
                Assert.AreEqual(result[i], expected[i], precision, message);
        }

    }
}
