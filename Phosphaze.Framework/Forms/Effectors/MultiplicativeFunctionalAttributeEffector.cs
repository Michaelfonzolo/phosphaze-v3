using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class MultiplicativeFunctionalAttributeEffector : MultiplicativeDoubleFunctionalEffector
    {

        public Func<double, double> func { get; private set; }

        public MultiplicativeFunctionalAttributeEffector(string attribute, Func<double, double> func)
            : base(attribute)
        {
            this.func = func;
        }

        public MultiplicativeFunctionalAttributeEffector(string attribute, Func<double, double> func, Form form)
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
