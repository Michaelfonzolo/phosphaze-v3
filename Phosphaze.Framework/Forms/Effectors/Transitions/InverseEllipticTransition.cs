using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class InverseEllipticTransition : AbstractTransition
    {

        public InverseEllipticTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public InverseEllipticTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

        protected override double Function(double time, int frame)
        {
            return totalIncrement * (1 - Math.Sqrt(1 - Math.Pow(time / duration, 2.0))) + initialValue;
        }

    }
}
