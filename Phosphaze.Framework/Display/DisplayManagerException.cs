using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Display
{
    public class DisplayManagerException : Exception
    {

        public DisplayManagerException() : base() { }
        public DisplayManagerException(string msg) : base(msg) { }
        public DisplayManagerException(string msg, Exception inner) : base(msg, inner) { }

    }
}
