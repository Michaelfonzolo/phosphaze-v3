using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class BiMutagradeTransition : AbstractTransition
    {

        public double startSlope { get; private set; }

        public double endSlope { get; private set; }

        public Mollifier mollifier { get; private set; }

        private Func<double, double> _alteredMollifier;

        private Func<double, double> _linearPiecewise;

        private double epsilon = 1 / 6f;

        private double a, b;

        public BiMutagradeTransition(
            string attr, double totalIncrement, double duration, double startSlope, double endSlope)
            : this(attr, totalIncrement, duration, startSlope, endSlope, Mollifier.Standard) { }

        public BiMutagradeTransition(
            string attr, double totalIncrement, double duration, 
            double startSlope, double endSlope, Mollifier mollifier)
            : base(attr, totalIncrement, duration)
        {
            CheckMollifier(mollifier);
            this.startSlope = startSlope;
            this.endSlope = endSlope;
            this.mollifier = mollifier;
        }

        public BiMutagradeTransition(
            string attr, double totalIncrement, double duration,
            double startSlope, double endSlope, Form form)
            : this(attr, totalIncrement, duration, startSlope, endSlope, Mollifier.Standard, form) { }

        public BiMutagradeTransition(
            string attr, double totalIncrement, double duration,
            double startSlope, double endSlope, Mollifier mollifier, Form form)
            : base(attr, totalIncrement, duration, form)
        {
            CheckMollifier(mollifier);
            this.startSlope = startSlope;
            this.endSlope = endSlope;
            this.mollifier = mollifier;
        }

        private void CheckMollifier(Mollifier mollifier)
        {
            if (Math.Abs(mollifier.Support[0]) == Double.PositiveInfinity ||
                Math.Abs(mollifier.Support[1]) == Double.PositiveInfinity)
                throw new ArgumentException(
                    "Invalid mollifier. BiMutagrade mollifiers must have compact support.");
        }

        protected override void Initialize()
        {
            base.Initialize();

            var gamma = duration * (mollifier.Support[1] - mollifier.Support[0]);
            _alteredMollifier =
                x => mollifier.Call((x + mollifier.Support[0])/gamma);
            // This shifts the mollifier so it's domain is the interval [0, duration].

            a = 0.5 / startSlope;
            b = 1 - 0.5 / endSlope;

            _linearPiecewise =
                x =>
                    x >= b
                    ? endSlope * (x - 1) + 1
                    : (
                        x >= a
                        ? 0.5
                        : startSlope * x
                    ); // yuck

            
        }

        protected override double Function(double time, int frame)
        {
            return totalIncrement / epsilon * Integrator.Simpsons(
                y => _alteredMollifier(y / epsilon) * _linearPiecewise((time - y) / duration),
                epsilon, epsilon, 8) + initialValue;
        }

    }
}
