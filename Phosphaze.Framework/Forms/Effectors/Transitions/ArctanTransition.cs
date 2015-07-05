using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class ArctanTransition : AbstractTransition
    {

        private double minSlope;

        private double alpha, beta;

        public ArctanTransition(string attr, double totalIncrement, double duration, double minSlope)
            : base(attr, totalIncrement, duration) 
        {
            this.minSlope = minSlope;
        }

        public ArctanTransition(string attr, double totalIncrement, double duration, double minSlope, Form form)
            : base(attr, totalIncrement, duration, form)
        {
            this.minSlope = minSlope;
        }

        protected override void Initialize()
        {
            base.Initialize();
            beta = RootSolver.NewtonsMethod(
                x => x * Math.Tan(2 * totalIncrement / x),
                x => { 
                    var u = 2 * totalIncrement / x; 
                    return Math.Tan(u) - u*Math.Pow( 1 / Math.Cos(u), 2.0); 
                },
                2 * minSlope * duration,
                initialGuess: 1.0
                );
            alpha = duration / Math.Tan(totalIncrement / beta);
        }

        protected override double Function(double time, int frame)
        {
            return beta * Math.Atan(time / alpha) + initialValue;
        }

    }
}
