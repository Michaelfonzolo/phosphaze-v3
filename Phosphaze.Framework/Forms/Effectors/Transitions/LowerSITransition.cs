using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class LowerSITransition : AbstractTransition
    {

        private double alpha, beta;

        public LowerSITransition(
            string attr
            , double totalIncrement
            , double duration
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) { }

        public LowerSITransition(
            string attr
            , double totalIncrement
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            // 15.707963 is the 6th root of Si'(x).
            // Si(15.707963) = 1.6339648461028329
            alpha = deltaValue / 1.6339648461028329;
            beta = 15.707963 / duration;
        }

        protected override double Function(double time, int frame)
        {
            return alpha
                * (SpecialFunctions.Si(
                    beta * (time - 1))
                    + 1.6339648461028329)
                + initialValue;
        }

    }
}
