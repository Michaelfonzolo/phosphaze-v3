using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms
{
    public class EffectorException : Exception
    {

        public EffectorException() : base() { }
        public EffectorException(string msg) : base(msg) { }
        public EffectorException(string msg, Exception inner) : base(msg, inner) { }

    }
}
