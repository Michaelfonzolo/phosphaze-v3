using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class CumulativeFunctionalAttributeEffector : AdditiveDoubleFunctionalEffector
    {

        public Func<double, double> func { get; private set; }

        public CumulativeFunctionalAttributeEffector(string attribute, Func<double, double> func)
            : base(attribute)
        {
            this.func = func;
        }

        public CumulativeFunctionalAttributeEffector(string attribute, Func<double, double> func, Form form)
            : base(attribute, form)
        {
            this.func = func;
        }

        protected override double Function(double time, int frame)
        {
            return func(time);
        }

    }
}
