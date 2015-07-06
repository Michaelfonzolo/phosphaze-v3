using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phosphaze.Framework.Maths;
using Phosphaze.UnitTests.TestUtils;

namespace Phosphaze.UnitTests.Maths
{
    [TestClass]
    public class QuadraticRootSolverTest
    {

        private const double EPSILON = 1e-5;

        private static double[] EXPECTED_RESULT_1 = {
            -1,
            3
        };

        private static readonly double[] EXPECTED_RESULT_2 = 
        {
            -0.08601149627711162,
            25.53548402755053
        };

        private static readonly double[] EXPECTED_RESULT_3 =
        {
            5.8
        };

        private static readonly double[] EXPECTED_RESULT_4 =
        {
            -18.511226905710355
        };

        [TestMethod]
        public void Test1()
        {
            DoubleCollectionAssert.AreEqual(
                RootSolver.Quadratic(-1, 2, 3), EXPECTED_RESULT_1, EPSILON, "QuadraticRootSolver.Test001 failed.");
            DoubleCollectionAssert.AreEqual(
                RootSolver.Quadratic(-1.7347, 44.1472, 3.81), EXPECTED_RESULT_2, EPSILON, "QuadraticRootSolver.Test002 failed.");
            DoubleCollectionAssert.AreEqual(
                RootSolver.Quadratic(1, -11.6, 33.64), EXPECTED_RESULT_3, EPSILON, "QuadraticRootSolver.Test003 failed.");
            DoubleCollectionAssert.AreEqual(
                RootSolver.Quadratic(95.60635062250002, 3539.5817, 32761), EXPECTED_RESULT_4, EPSILON, "QuadraticRootSolver.Test004 failed.");
        }

    }
}
