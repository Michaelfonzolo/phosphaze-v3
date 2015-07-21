using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeonVM.Neon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeonVMTests.Neon
{
 
    [TestClass]
    public class TokenizerTest
    {

        public class TokenizerTestCase : TestCase<string, string[]>
        {
            public override string Header { get { return "TKN"; } }

            public override void Prepare(string[] f_input, string[] f_expected)
            {
                Input = String.Join("\n", f_input);
                var expected = new List<string>();
                foreach (var line in f_expected)
                {
                    if (line == @"\n")
                        expected.Add("\n");
                    else
                        expected.Add(line);
                }
                Expected = expected.ToArray();
            }

            public override void Run()
            {
                TestUtils.AssertContainerEqual(
                    Expected,
                    new Tokenizer(Input).Tokenize().ToArray()
                    );
            }
        }

        private static readonly string TEST_DIRECTORY = Path.Combine("Neon", "TokenizerTestFiles");

        TestParser<TokenizerTestCase> testReader;

        [TestInitialize]
        public void Initialize()
        {
            testReader = new TestParser<TokenizerTestCase>();
            testReader.SetDefaultDirectory(TEST_DIRECTORY);
        }

        [TestMethod]
        public void Tokenizer_WordSeparationBySpaces()
        {
            testReader.Run("test001");
        }

        [TestMethod]
        public void Tokenizer_SeparationOfWordsAndNumbers()
        {
            testReader.Run("test002");
        }

        [TestMethod]
        public void Tokenizer_InclusionOfNewlines()
        {
            testReader.Run("test003");
        }

        [TestMethod]
        public void Tokenizer_BasicMonoglyphTokenization()
        {
            testReader.Run("test004");
        }

        [TestMethod]
        public void Tokenizer_BasicDiglyphTokenization()
        {
            testReader.Run("test005");
        }

        [TestMethod]
        public void Tokenizer_BasicTriglyphTokenization()
        {
            testReader.Run("test006");
        }

        [TestMethod]
        public void Tokenizer_AdvancedTriglyphTokenization()
        {
            testReader.Run("test007");
        }

        [TestMethod]
        public void Tokenizer_AdvancedPolyglyphTokenization()
        {
            testReader.Run("test008");
        }

        [TestMethod]
        public void Tokenizer_StringsParsedProperly()
        {
            testReader.Run("test009");
        }

        [TestMethod]
        [ExpectedException(typeof(NeonParseException))]
        public void Tokenizer_UnknownCharacterThrowsParseException()
        {
            testReader.Run("test010");
        }

        [TestMethod]
        [ExpectedException(typeof(NeonParseException))]
        public void Tokenizer_MultilineStringThrowsParseException()
        {
            testReader.Run("test011");
        }

        [TestMethod]
        [ExpectedException(typeof(NeonParseException))]
        public void Tokenizer_UnclosedStringThrowsParseException()
        {
            testReader.Run("test012");
        }
    }
}
