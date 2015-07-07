using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phosphaze.Framework.Maths;
using System;

namespace Phosphaze.UnitTests.Maths
{

    [TestClass]
    public class NewtonsMethodTest
    {

        private const double EPSILON = 1e-6;

        private static readonly Func<double, double> FUNC_1 = x => x;
        private static readonly Func<double, double> DF_1 = x => 1;
        private static readonly double EXPECTED_RESULT_1 = 5.0;

        private static readonly Func<double, double> FUNC_2 = x => SpecialFunctions.Gamma(x);
        private static readonly Func<double, double> DF_2 = x => SpecialFunctions.Gamma(x) * SpecialFunctions.Digamma(x);
        private static readonly double EXPECTED_RESULT_2 = 4.76698049453099;

        private static readonly Func<double, double> FUNC_3 =
            x => x * Math.Log(x + Math.Log(1 + Math.Exp(x)))*Math.Exp(x);
        private static readonly Func<double, double> DF_3 =
            x => FUNC_3(x) + Math.Exp(x) * (Math.Exp(x) / (Math.Exp(x) + 1) + 1) * x / (x + Math.Log(Math.Exp(x) + 1)) + FUNC_3(x) / x;
        private static readonly double EXPECTED_RESULT_3 = 6.41488318625337;

        [TestMethod]
        public void NewtonsMethodTest1()
        {

            Assert.AreEqual(
                EXPECTED_RESULT_1, RootSolver.NewtonsMethod(FUNC_1, DF_1, 5, 1), EPSILON, "NewtonsMethod.Test001 failed.");
            Assert.AreEqual(
                EXPECTED_RESULT_2, RootSolver.NewtonsMethod(FUNC_2, DF_2, 17, 4), EPSILON, "NewtonsMethod.Test002 failed.");
            Assert.AreEqual(
                EXPECTED_RESULT_3, RootSolver.NewtonsMethod(FUNC_3, DF_3, 10000, 5), EPSILON, "NewtonsMethod.Test003 failed.");

        }

    }
}
