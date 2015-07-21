using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NeonVM.Neon;
using NeonVM.Neon.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeonVMTests.Neon
{
    [TestClass]
    public class ShuntingYardParserTest
    {

        public class ShuntingYardParserTestCase : TestCase<string[], IInstruction[]>
        {

            private static Dictionary<string, IInstruction> STR_TO_INSTR
                = new Dictionary<string, IInstruction>()
            {
                {"BIN_ADD",     BIN_ADD.Instance},
                {"BIN_AND",     BIN_AND.Instance},
                {"BIN_DIV",     BIN_DIV.Instance},
                {"BIN_EQ",      BIN_EQ.Instance},
                {"BIN_GE",      BIN_GE.Instance},
                {"BIN_GT",      BIN_GT.Instance},
                {"BIN_LE",      BIN_LT.Instance},
                {"BIN_MOD",     BIN_MOD.Instance},
                {"BIN_MUL",     BIN_MUL.Instance},
                {"BIN_NE",      BIN_NE.Instance},
                {"BIN_OR",      BIN_OR.Instance},
                {"BIN_POW",     BIN_POW.Instance},
                {"BIN_SUB",     BIN_SUB.Instance},
                {"BUILD_RANGE", BUILD_RANGE.Instance},
                {"BUILD_RVEC",  BUILD_RVEC.Instance},
                {"BUILD_VEC",   BUILD_VEC.Instance},
                {"UN_NEG",      UN_NEG.Instance},
                {"UN_NOT",      UN_NOT.Instance},
                {"UN_POS",      UN_POS.Instance}
            };

            private static Dictionary<string, Func<string[], IInstruction>> COMPLEX_INSTRS
                = new Dictionary<string, Func<string[], IInstruction>>()
            {
                {"LDB", _LDBfromStr},
                {"LDL", _LDLfromStr},
                {"LDC", _LDCfromStr},
                {"BUILD_ARRAY", _BUILD_ARRAYfromStr},
                {"BUILD_DICT", _BUILD_DICTfromStr}
            };

            private static IInstruction _LDBfromStr(string[] components)
            {
                return new LDB(components[1]);
            }

            private static IInstruction _LDLfromStr(string[] components)
            {
                return new LDL(components[1]);
            }

            private static IInstruction _LDCfromStr(string[] components)
            {
                // Change this later.
                if (Regex.IsMatch(components[1], @"[0-9]+(\.[0-9]+)?"))
                    return new LDC(new NeonObject());
                else if (Regex.IsMatch(components[1], @"[a-zA-Z_][a-zA-Z0-9_]*"))
                    return new LDC(new NeonObject());
                else if (components[1][0] == '"' && components[1].Last() == '"')
                    return new LDC(new NeonObject());
                throw new TestParserException("Invalid components for LDC instruction.");
            }

            private static IInstruction _BUILD_ARRAYfromStr(string[] components)
            {
                return new BUILD_ARRAY(Int32.Parse(components[1]));
            }

            private static IInstruction _BUILD_DICTfromStr(string[] components)
            {
                return new BUILD_DICT(Int32.Parse(components[1]));
            }

            private static IInstruction[] ParseInstructions(string[] fromFile)
            {
                var instrs = new List<IInstruction>();
                string[] components;
                foreach (var line in fromFile)
                {
                    if (line.Contains(' '))
                    {
                        components = line.Split(' ');
                        instrs.Add(COMPLEX_INSTRS[components[0]](components));
                        continue;
                    }
                    if (STR_TO_INSTR.ContainsKey(line))
                        instrs.Add(STR_TO_INSTR[line]);
                }
                return instrs.ToArray();
            }

            public override string Header { get { return "SYP"; } }

            public override void Prepare(string[] f_input, string[] f_expected)
            {
                Input = new Tokenizer(String.Join("\n", f_input)).Tokenize().ToArray();
                Expected = ParseInstructions(f_expected);
            }

            public override void Run()
            {
                var syp = new ShuntingYardParser(new List<string>(Input));
                syp.Parse();

                TestUtils.AssertContainerEqual(
                    Expected,
                    syp.GetInstructions().ToArray()
                    );
            }
        }

        private static readonly string TEST_DIRECTORY = Path.Combine("Neon", "ShuntingYardParserTestFiles");

        TestParser<ShuntingYardParserTestCase> testReader;

        [TestInitialize]
        public void Initialize()
        {
            testReader = new TestParser<ShuntingYardParserTestCase>();
            testReader.SetDefaultDirectory(TEST_DIRECTORY);
        }

        [TestMethod]
        public void SYP_BasicArithmetic()
        {
            testReader.Run("test001");
        }

        [TestMethod]
        public void SYP_BinaryOperatorPrecedence()
        {
            testReader.Run("test002");
        }

        [TestMethod]
        public void SYP_AdvancedBinaryOperatorPrecedence()
        {
            testReader.Run("test003");
        }

        [TestMethod]
        public void SYP_DifferentiateBetweenAmbiguousUnaryBinaryOperators()
        {
            testReader.Run("test004");
        }

        [TestMethod]
        public void SYP_AdvancedUnaryOperatorChaining()
        {
            testReader.Run("test005");
        }

        [TestMethod]
        public void SYP_BracketsChangeOrderOfEvaluation()
        {
            testReader.Run("test006");
        }

        [TestMethod]
        public void SYP_AdvancedBracketUsage()
        {
            testReader.Run("test007");
        }

        [TestMethod]
        public void SYP_BracketsAndUnaryOperators()
        {
            testReader.Run("test008");
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void SYP_UseOfBinaryAsUnaryOpThrowsException()
        {
            testReader.Run("test009");
        }

        [TestMethod]
        public void SYP_SingleLineCommentsIgnored()
        {
            testReader.Run("test010");
        }

        [TestMethod]
        public void SYP_MultiLineCommentsIgnored()
        {
            testReader.Run("test011");
        }

        [TestMethod]
        public void SYP_SingleLineCommentsToEOFIsOK()
        {
            testReader.Run("test012");
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void SYP_MultiLineCommentsToEOFThrowsException()
        {
            testReader.Run("test013");
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void SYP_MismatchedClosingBracketThrowsException()
        {
            testReader.Run("test014");
        }

        [TestMethod]
        [ExpectedException(typeof(NeonSyntaxException))]
        public void SYP_UnclosedOpeningBracketThrowsException()
        {
            testReader.Run("test015");
        }

    }
}
