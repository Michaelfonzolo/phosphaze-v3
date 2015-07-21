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
        public void WordSeparationBySpaces()
        {
            testReader.Run("test001");
        }

        [TestMethod]
        public void SeparationOfWordsAndNumbers()
        {
            testReader.Run("test002");
        }

        [TestMethod]
        public void InclusionOfNewlines()
        {
            testReader.Run("test003");
        }

        [TestMethod]
        public void BasicMonoglyphTokenization()
        {
            testReader.Run("test004");
        }

        [TestMethod]
        public void BasicDiglyphTokenization()
        {
            testReader.Run("test005");
        }

    }
}
