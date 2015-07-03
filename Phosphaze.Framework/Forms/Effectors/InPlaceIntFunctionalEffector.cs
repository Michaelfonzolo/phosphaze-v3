using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class InPlaceIntFunctionalEffector : IntFunctionalEffector
    {

        public InPlaceIntFunctionalEffector(string attr) : base(attr) { }

        public InPlaceIntFunctionalEffector(string attr, Form form) : base(attr, form) { }

        protected override int Operate(int a, int b)
        {
            return b; // b is the result of calling Function, so just returning it overrides the
            // previous value.
        }

    }
}
