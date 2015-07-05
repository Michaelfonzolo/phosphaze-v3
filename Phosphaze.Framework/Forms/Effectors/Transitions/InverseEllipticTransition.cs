using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class InverseEllipticTransition : AbstractTransition
    {

        public InverseEllipticTransition(
            string attr
            , double totalIncrement
            , double duration
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) { }

        public InverseEllipticTransition(
            string attr
            , double totalIncrement
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative) { }

        protected override double Function(double time, int frame)
        {
            return deltaValue * (1 - Math.Sqrt(1 - Math.Pow(time / duration, 2.0))) + initialValue;
        }

    }
}
