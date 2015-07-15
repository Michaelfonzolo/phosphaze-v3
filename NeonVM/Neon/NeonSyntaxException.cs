using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public class NeonSyntaxException : Exception
    {

        public NeonSyntaxException() : base() { }
        public NeonSyntaxException(string msg) : base(msg) { }
        public NeonSyntaxException(string msg, Exception inner) : base(msg, inner) { }

    }
}
