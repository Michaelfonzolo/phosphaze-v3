using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class AdditiveDoubleFunctionalEffector : DoubleFunctionalEffector
    {
        public AdditiveDoubleFunctionalEffector(string attr) : base(attr) { }

        public AdditiveDoubleFunctionalEffector(string attr, Form form) : base(attr, form) { }

        protected override double Operate(double a, double b)
        {
            return a + b;
        }
    }
}
