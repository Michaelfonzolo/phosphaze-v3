using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze_V3.Framework.Forms
{
    public class MultiformException : Exception
    {

        public MultiformException() : base() { }
        public MultiformException(string msg) : base(msg) { }
        public MultiformException(string msg, Exception inner) : base(msg, inner) { }

    }
}
