using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phosphaze.Framework.Maths;
using System;

namespace Phosphaze.UnitTests.Maths
{
    [TestClass]
    public class IntegratorTest
    {

        private static readonly double EPSILON = 1e-5;

        private static readonly Func<double, double> FUNC_1 = x => x;
        private static readonly double EXPECTED_RESULT_1 = 0.5;

        private static readonly Func<double, double> FUNC_2 = x => Math.Sin(Math.Sin(x));
        private static readonly double EXPECTED_RESULT_2 = 0.4306061031206906;

        private static readonly Func<double, double> FUNC_3 = x => Math.Sqrt(x * x + Math.Sin(x)) / (Math.Exp(x) - x);
        private static readonly double EXPECTED_RESULT_3 = 1.7433485921143947;

        [TestMethod]
        public void IntegratorTest1()
        {

            Assert.AreEqual(
                EXPECTED_RESULT_1, Integrator.Simpsons(FUNC_1, 0, 1, 10), EPSILON, "Integrator.Test001 failed.");
            Assert.AreEqual(
                EXPECTED_RESULT_2, Integrator.Simpsons(FUNC_2, 0, 1, 100), EPSILON, "Integrator.Test002 failed.");
            Assert.AreEqual( // This one is strangely difficult to calculate.
                EXPECTED_RESULT_3, Integrator.Simpsons(FUNC_3, 0, 7.8317557823642, 10000), EPSILON, "Integrator.Test003 failed.");

        }

    }
}
