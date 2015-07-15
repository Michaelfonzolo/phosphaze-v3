using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeonVM.Neon;

namespace NeonVMTests.Neon
{
    [TestClass]
    public class TokenizerTest
    {

        private static string PROGRAM_1 = "1 + 2 + 3";
        private static string[] EXPECTED_RESULT_1 = new string[] {
            "1",
            "+",
            "2",
            "+",
            "3"
        };

        private static string PROGRAM_2 = "alpha += 3.5";
        private static string[] EXPECTED_RESULT_2 = new string[] {
            "alpha",
            "+=",
            "3.5"
        };

        private static string PROGRAM_3 = "1002572350a";
        private static string[] EXPECTED_RESULT_3 = new string[] {
            "1002572350",
            "a"
        };

        private static string PROGRAM_4 = "+=+ =||<<a|>10->=>>=";
        private static string[] EXPECTED_RESULT_4 = new string[] {
            "+=",
            "+",
            "=",
            "||",
            "<<",
            "a",
            "|>",
            "10",
            "->",
            "=>",
            ">="
        };

        private static string PROGRAM_5 = "StarPolygon = Path({\npolygon({\n<0, 2>,\n<1, -2>,\n<-2, 0.8>,\n<2, 0.8>,\n<-1, -2>,\n<0, 2>\n})\n});";
        private static string[] EXPECTED_RESULT_5 = new string[] {
            "StarPolygon",
            "=",
            "Path",
            "(",
            "{",
            "\n",
            "polygon",
            "(",
            "{",
            "\n",
            "<",
            "0",
            ",",
            "2",
            ">",
            ",",
            "\n",
            "<",
            "1",
            ",",
            "-",
            "2",
            ">",
            ",",
            "\n",
            "<",
            "-",
            "2",
            ",",
            "0.8",
            ">",
            ",",
            "\n",
            "<",
            "2",
            ",",
            "0.8",
            ">",
            ",",
            "\n",
            "<",
            "-",
            "1",
            ",",
            "-",
            "2",
            ">",
            ",",
            "\n",
            "<",
            "0",
            ",",
            "2",
            ">",
            "\n",
            "}",
            ")",
            "\n",
            "}",
            ")",
            ";"
        };

        private static string PROGRAM_6 = "Follow([\"path\" : StarPolygon, \"duration\" : 1000]);";
        private static string[] EXPECTED_RESULT_6 = new string[] {
            "Follow",
            "(",
            "[",
            "\"path\"",
            ":",
            "StarPolygon",
            ",",
            "\"duration\"",
            ":",
            "1000",
            "]",
            ")",
            ";"
        };

        [TestMethod]
        public void NeonTokenizerTest001()
        {
            _TestTokenizer(EXPECTED_RESULT_1, PROGRAM_1);
            _TestTokenizer(EXPECTED_RESULT_2, PROGRAM_2);
            _TestTokenizer(EXPECTED_RESULT_3, PROGRAM_3);
            _TestTokenizer(EXPECTED_RESULT_4, PROGRAM_4);
            _TestTokenizer(EXPECTED_RESULT_5, PROGRAM_5);
            _TestTokenizer(EXPECTED_RESULT_6, PROGRAM_6);
        }

        private void _TestTokenizer(string[] expected, string program)
        {
            var result = new Tokenizer(program).Tokenize().ToArray();

            var message = new StringBuilder();
            message.Append("\nExpected: ");
            foreach (var token in expected)
                message.Append(token + ", ");
            message.Append("\nReceived: ");
            foreach (var token in result)
                message.Append(token + ", ");

            var error = message.ToString();
            for (int i = 0; i < Math.Min(expected.Length, result.Length); i++)
                Assert.AreEqual(expected[i], result[i], error);
        }

    }
}
