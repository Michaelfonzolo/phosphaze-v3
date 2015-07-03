using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class InPlaceDoubleFunctionalEffector : DoubleFunctionalEffector
    {

        public InPlaceDoubleFunctionalEffector(string attr) : base(attr) { }

        public InPlaceDoubleFunctionalEffector(string attr, Form form) : base(attr, form) { }

        protected override double Operate(double a, double b)
        {
            return b; // b is the result of calling Function, so just returning it overrides the
                      // previous value.
        }

    }
}
