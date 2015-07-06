using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phosphaze.Framework.Maths;
using Phosphaze.UnitTests.TestUtils;

namespace Phosphaze.UnitTests.Maths
{
    [TestClass]
    public class CubicRootSolverTest
    {

        private const double EPSILON = 1e-5;

        private static readonly double[] EXPECTED_RESULT_1 = new double[] { 
            -1.87938524157182, 
            0.347296355333861, 
            1.53208888623796 
        };

        private static readonly double[] EXPECTED_RESULT_2 = new double[] {
            -0.218479453370216, 
            0.702433582556207, 
            6.51604587081401
        };

        private static readonly double[] EXPECTED_RESULT_3 = new double[] {
            -1.09136620256721
        };

        private static readonly double[] EXPECTED_RESULT_4 = new double[] {
            0.904535426175899
        };

        [TestMethod]
        public void Test1()
        {

            DoubleCollectionAssert.AreEqual(
                EXPECTED_RESULT_1, RootSolver.Cubic(1, 0, -3, 1), EPSILON, "CubicRootSolver.Test001 failed.");
            DoubleCollectionAssert.AreEqual(
                EXPECTED_RESULT_2, RootSolver.Cubic(1, -7, 3, 1), EPSILON, "CubicRootSolver.Test002 failed.");
            DoubleCollectionAssert.AreEqual(
                EXPECTED_RESULT_3, RootSolver.Cubic(15, 5, 5, 19), EPSILON, "CubicRootSolver.Test003 failed.");
            DoubleCollectionAssert.AreEqual(
                EXPECTED_RESULT_4, RootSolver.Cubic(-71, -3, 0, 55), EPSILON, "CubicRootSolver.Test004 failed.");
            Assert.AreEqual(
                1.0, RootSolver.Cubic(1, 0, 0, -1)[0], EPSILON, "CubicRootSolver.Test005 failed.");
            Assert.AreEqual(
                -1.0, RootSolver.Cubic(1, 0, 0, 1)[0], EPSILON, "CubicRootSolver.Test006 failed.");
            DoubleCollectionAssert.AreEqual(
                RootSolver.Quadratic(1, 2, 3), RootSolver.Cubic(0, 1, 2, 3), EPSILON, "CubicRootSolver.Test007 failed.");

        }
    }
}
