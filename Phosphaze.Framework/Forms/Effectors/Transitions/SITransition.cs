using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class SITransition : AbstractTransition
    {

        private double alpha, beta;

        public SITransition(
            string attr
            , double totalIncrement
            , double duration
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) { }

        public SITransition(
            string attr
            , double totalIncrement
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = deltaValue / (2 * 1.6339648461);
            beta = 15.707963 * 2 / duration;
        } 

        protected override double Function(double time, int frame)
        {
            return alpha
                * (SpecialFunctions.Si(beta * (time - 0.5))
                    + 1.6339648461)
                + initialValue;
        }

    }
}
