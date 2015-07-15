using System;
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

        [TestMethod]
        public void ShuntingYardTest001()
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
        public void MismatchedBracketsTest001()
        {
            var sy = new ShuntingYard(new List<string>(PROGRAM_4));
            sy.Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void MismatchedBracketsTest002()
        {
            var sy = new ShuntingYard(new List<string>(PROGRAM_5));
            sy.Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void MismatchedBracketsTest003()
        {
            var sy = new ShuntingYard(new List<string>(PROGRAM_6));
            sy.Parse();
        }

        [TestMethod]
        public void AvoidSingleLineCommentsTest001()
        {
            _TestShuntingYard(EXPECTED_RESULT_7, PROGRAM_7);
            _TestShuntingYard(EXPECTED_RESULT_8, PROGRAM_8);
        }

        [TestMethod]
        public void AvoidMultiLineCommentsTest001()
        {
            _TestShuntingYard(EXPECTED_RESULT_9, PROGRAM_9);
            _TestShuntingYard(EXPECTED_RESULT_10, PROGRAM_10);
            _TestShuntingYard(EXPECTED_RESULT_11, PROGRAM_11);
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void MismatchedMultilineCommentsTest001()
        {
            var sy = new ShuntingYard(new List<string>(PROGRAM_12));
            sy.Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void MismatchedMultilineCommentsTest002()
        {
            var sy = new ShuntingYard(new List<string>(PROGRAM_13));
            sy.Parse();
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void MismatchedMultilineCommentsTest003()
        {
            var sy = new ShuntingYard(new List<string>(PROGRAM_14));
            sy.Parse();
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
