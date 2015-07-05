using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class MutagradeTransition : AbstractTransition
    {

        public double slope { get; private set; }

        private double alpha;

        private double beta;

        public MutagradeTransition(
            string attr
            , double totalIncrement
            , double duration
            , double slope
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative)
        {
            this.slope = slope;
        }

        public MutagradeTransition(
            string attr
            , double totalIncrement
            , double duration
            , double slope
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative)
        {
            this.slope = slope;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Func<double, double> Gm = SpecialFunctions.Gamma;
            Func<double, double> Psi = SpecialFunctions.Digamma;
            var ln4 = Math.Log(4.0);

            beta = RootSolver.NewtonsMethod(
                x => Gm(2 * x) / (Math.Pow(4, x - 1) * Math.Pow(Gm(x), 2.0)),
                x => Math.Pow(4, 1 - x) * Gm(2 * x) * (2 * Psi(x) - 2 * Psi(2 * x) + ln4) / Math.Pow(Gm(x), 2.0),
                slope,
                initialGuess: 1,
                epsilon: 1e-5 // We don't need really accurate results, and the 
                              // derivative is really expensive to calculate.
                );
            alpha = Math.Pow(Gm(beta), 2.0) / Gm(2 * beta);
        }

        protected override double Function(double time, int frame)
        {
            return deltaValue * alpha * Math.Pow(time / duration, beta - 1) + initialValue;
        }

    }
}
