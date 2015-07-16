﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeonVM.Neon;
using NeonVM.Neon.Instructions;

namespace NeonTests.Neon
{

    [TestClass]
    public class ShuntingYardTest
    {

        public static string[] PROGRAM_1 = new string[] { "1", "+", "2", "+", "3" };
        public static IInstruction[] EXPECTED_RESULT_1 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BIN_ADD.Instance,
            new LDC(new NeonObject()),
            BIN_ADD.Instance
        };

        public static string[] PROGRAM_2 = new string[] { "-", "(", "1", "+", "+", "5", ")", "*", "3" };
        public static IInstruction[] EXPECTED_RESULT_2 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            UN_POS.Instance,
            BIN_ADD.Instance,
            UN_NEG.Instance,
            new LDC(new NeonObject()),
            BIN_MUL.Instance
        };

        public static string[] PROGRAM_3 = new string[] { "3", "+", "4", "*", "2", "/", "(", "1", "-", "5", ")", "^", "2", "^", "3" };
        public static IInstruction[] EXPECTED_RESULT_3 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BIN_SUB.Instance,
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BIN_POW.Instance,
            BIN_POW.Instance,
            BIN_DIV.Instance,
            BIN_MUL.Instance,
            BIN_ADD.Instance
        };

        public static string[] PROGRAM_4 = new string[] { "1", "+", "5", ")" };

        public static string[] PROGRAM_5 = new string[] { "(", "1", "+", "5" };

        public static string[] PROGRAM_6 = new string[] { "1", "+", "5", "*", "(", "3", "-", "7", "^", "(", "7", "-", "1", ")", "*", "(", "5", "+", "3", ")", "^", "(", "8", "-", "2", "*", "(", "7", "-", "+", "-", "(", "-", "3", ")", ")", ")" };

        public static string[] PROGRAM_7 = new string[] { "1", "+", @"//", "comment", "\n", "2" };
        public static IInstruction[] EXPECTED_RESULT_7 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BIN_ADD.Instance
        };

        public static string[] PROGRAM_8 = new string[] { "1", "+", @"//", "comment", "\n", "-", "2" };
        public static IInstruction[] EXPECTED_RESULT_8 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            UN_NEG.Instance,
            BIN_ADD.Instance
        };

        public static string[] PROGRAM_9 = new string[] { "1", "+", "/*", "comment", "*/", "2" };
        public static IInstruction[] EXPECTED_RESULT_9 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BIN_ADD.Instance
        };

        public static string[] PROGRAM_10 = new string[] { "1", "+", "/*", "comment", "\n", "comment2", "*/", "2" };
        public static IInstruction[] EXPECTED_RESULT_10 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BIN_ADD.Instance
        };

        public static string[] PROGRAM_11 = new string[] { "1", "+", "/*", "comment", "\n", "comment2", "*/", "-", "2" };
        public static IInstruction[] EXPECTED_RESULT_11 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            UN_NEG.Instance,
            BIN_ADD.Instance
        };

        public static string[] PROGRAM_12 = new string[] { "1", "+", "/*", "comment", "2" };

        public static string[] PROGRAM_13 = new string[] { "1", "+", "/*", "comment", "\n", "-", "2" };

        public static string[] PROGRAM_14 = new string[] { "1", "+", "comment", "*/", "-", "2" };

        public static string[] PROGRAM_15 = new string[] { "<<", "5", ",", "1", ">>" };
        public static IInstruction[] EXPECTED_RESULT_15 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BUILD_VEC.Instance
        };

        // NOTE: This program would actually fail, as a vector cannot be constructed whose components are non-scalar
        // values, but that type of exception would be caught at run-time, not at compile-time, and so the resulting
        // instructions generated by the shunting yard algorithm are still technically "correct".
        public static string[] PROGRAM_16 = new string[] { "ORIGIN", "+", "<<", "1", ",", "<<", "3", ",", "5", ">>", ">>" };
        public static IInstruction[] EXPECTED_RESULT_16 = new IInstruction[]
        {
            new LDL("ORIGIN"),
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BUILD_VEC.Instance,
            BUILD_VEC.Instance
        };

        public static string[] PROGRAM_17 = new string[] { "<<", "/*", "5", "*/", "5", ",", "/*", "\n", "10", "*/", "10", ">>" };
        public static IInstruction[] EXPECTED_RESULT_17 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BUILD_VEC.Instance
        };

        public static string[] PROGRAM_18 = new string[] { "<|", "5", ",", "1", "|>" };
        public static IInstruction[] EXPECTED_RESULT_18 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BUILD_RVEC.Instance
        };

        public static string[] PROGRAM_19 = new string[] { "ORIGIN", "+", "<|", "1", ",", "<<", "3", ",", "5", ">>", "|>" };
        public static IInstruction[] EXPECTED_RESULT_19 = new IInstruction[]
        {
            new LDL("ORIGIN"),
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BUILD_VEC.Instance,
            BUILD_RVEC.Instance
        };

        public static string[] PROGRAM_20 = new string[] { "<|", "/*", "5", "*/", "5", ",", "/*", "\n", "10", "*/", "10", "|>" };
        public static IInstruction[] EXPECTED_RESULT_20 = new IInstruction[]
        {
            new LDC(new NeonObject()),
            new LDC(new NeonObject()),
            BUILD_RVEC.Instance
        };

        public static string[] PROGRAM_21 = new string[] { "5", ">>" };

        public static string[] PROGRAM_22 = new string[] { "5", "|>" };

        public static string[] PROGRAM_23 = new string[] { "<<", "5", ",", "7" };

        public static string[] PROGRAM_24 = new string[] { "<|", "5", ",", "7" };

        public static string[] PROGRAM_25 = new string[] { "<<", "1", ",", "2", ",", "3", ">>" };

        public static string[] PROGRAM_26 = new string[] { "<|", "1", ",", "2", ",", "3", "|>" };

        [TestMethod]
        public void ShuntingYard_BasicTest()
        {
            _TestShuntingYard(EXPECTED_RESULT_1, PROGRAM_1);
            _TestShuntingYard(EXPECTED_RESULT_2, PROGRAM_2);
            _TestShuntingYard(EXPECTED_RESULT_3, PROGRAM_3);
        }

        /* No Unary operators that are not binary operators yet
        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void BadUnaryOperator()
        {
            
        }
         */

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedBracketsTest1()
        {
            new ShuntingYard(new List<string>(PROGRAM_4)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedBracketsTest2()
        {
            new ShuntingYard(new List<string>(PROGRAM_5)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedBracketsTest3()
        {
            new ShuntingYard(new List<string>(PROGRAM_6)).Parse();
        }

        [TestMethod]
        public void ShuntingYard_AvoidSingleLineCommentsTest()
        {
            _TestShuntingYard(EXPECTED_RESULT_7, PROGRAM_7);
            _TestShuntingYard(EXPECTED_RESULT_8, PROGRAM_8);
        }

        [TestMethod]
        public void ShuntingYard_AvoidMultiLineCommentsTest()
        {
            _TestShuntingYard(EXPECTED_RESULT_9, PROGRAM_9);
            _TestShuntingYard(EXPECTED_RESULT_10, PROGRAM_10);
            _TestShuntingYard(EXPECTED_RESULT_11, PROGRAM_11);
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedMultilineCommentsTest1()
        {
            new ShuntingYard(new List<string>(PROGRAM_12)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedMultilineCommentsTest2()
        {
            new ShuntingYard(new List<string>(PROGRAM_13)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedMultilineCommentsTest3()
        {
            new ShuntingYard(new List<string>(PROGRAM_14)).Parse();
        }

        [TestMethod]
        public void ShuntingYard_BuildVectorTest()
        {
            _TestShuntingYard(EXPECTED_RESULT_15, PROGRAM_15);
            _TestShuntingYard(EXPECTED_RESULT_16, PROGRAM_16);
            _TestShuntingYard(EXPECTED_RESULT_17, PROGRAM_17);
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedOpeningVectorDelimiterTest()
        {
            new ShuntingYard(new List<string>(PROGRAM_23)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedClosingVectorDelimiterTest()
        {
            new ShuntingYard(new List<string>(PROGRAM_21)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_InvalidVectorComponentCountTest()
        {
            new ShuntingYard(new List<string>(PROGRAM_25)).Parse();
        }

        [TestMethod]
        public void ShuntingYard_BuildRelativeVectorTest()
        {
            _TestShuntingYard(EXPECTED_RESULT_18, PROGRAM_18);
            _TestShuntingYard(EXPECTED_RESULT_19, PROGRAM_19);
            _TestShuntingYard(EXPECTED_RESULT_20, PROGRAM_20);
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedOpeningRelativeVectorDelimiterTest()
        {
            new ShuntingYard(new List<string>(PROGRAM_24)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_MismatchedClosingRelativeVectorDelimiterTest()
        {
            new ShuntingYard(new List<string>(PROGRAM_22)).Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void ShuntingYard_InvalidRelativeVectorComponentCountTest()
        {
            new ShuntingYard(new List<string>(PROGRAM_26)).Parse();
        }

        private void _TestShuntingYard(IInstruction[] expected, string[] program)
        {
            var sy = new ShuntingYard(new List<string>(program));
            sy.Parse();
            var result = sy.GetInstructions();

            var message = new StringBuilder();

            message.Append("\nExpected: ");
            foreach (var token in expected)
                message.Append(token + ", ");

            message.Append("\nReceived: ");
            foreach (var token in result)
                message.Append(token + ", ");

            var error = message.ToString();
            for (int i = 0; i < Math.Min(expected.Length, result.Count); i++)
                Assert.AreEqual(expected[i], result[i], error);
        }

    }
}
