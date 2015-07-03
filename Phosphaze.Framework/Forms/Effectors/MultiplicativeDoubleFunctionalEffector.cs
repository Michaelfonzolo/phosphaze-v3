using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class MultiplicativeDoubleFunctionalEffector : DoubleFunctionalEffector
    {
        protected override double Operate(double a, double b)
        {
            return a * b;
        }
    }
}
