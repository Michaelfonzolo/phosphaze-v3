using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class AbstractPowerSineTransition : AbstractTransition
    {

        protected abstract double Power { get; set; }

        private double alpha;

        public AbstractPowerSineTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public AbstractPowerSineTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = Math.PI / (2.0 * duration);
        }

        protected override double Function(double time, int frame)
        {
            return totalIncrement * Math.Sin(alpha * time) + initialValue;
        }

    }
}
