using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTests.Neon
{
    public class TestParserException : Exception
    {
        public TestParserException() : base() { }
        public TestParserException(string msg) : base(msg) { }
        public TestParserException(string msg, Exception inner) : base(msg, inner) { }
    }
}
