using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeonVMTests.Neon
{
    public static class TestUtils
    {
        public static void AssertContainerEqual(object[] expected, object[] result)
        {
            var message = new StringBuilder();

            message.Append("\nExpected: ");
            foreach (var o1 in expected)
                message.Append(o1.ToString() + ", ");

            message.Append("\nReceived: ");
            foreach (var o2 in result)
                message.Append(o2.ToString() + ", ");

            var error = message.ToString();
            Assert.AreEqual(expected.Length, result.Length, error);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], result[i], error);
        }
    }
}
