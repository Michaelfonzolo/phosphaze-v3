using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public class NeonParseException : Exception
    {

        public NeonParseException() : base() { }
        public NeonParseException(string msg) : base(msg) { }
        public NeonParseException(string msg, Exception inner) : base(msg, inner) { }

    }
}
