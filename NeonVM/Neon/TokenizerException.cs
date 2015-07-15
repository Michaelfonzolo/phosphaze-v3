using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public class TokenizerException : Exception
    {

        public TokenizerException() : base() { }
        public TokenizerException(string msg) : base(msg) { }
        public TokenizerException(string msg, Exception inner) : base(msg, inner) { }

    }
}
