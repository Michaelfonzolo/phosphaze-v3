using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class UpperSISquaredTransition : AbstractTransition
    {

        private Func<double, double> _func =
            x => SpecialFunctions.Si(Math.Sign(x) * x * x);

        private double alpha, beta;

        public UpperSISquaredTransition(
            string attr
            , double totalIncrement
            , double duration
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) { }

        public UpperSISquaredTransition(
            string attr
            , double totalIncrement
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = deltaValue / 1.633964846102816;
            beta = 3.9633272 / duration;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * _func(beta * time) + initialValue;
        }
    }
}
