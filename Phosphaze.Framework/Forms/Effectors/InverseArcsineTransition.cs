using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class InverseArcsineTransition : AbstractTransition
    {

        private double alpha;

        public InverseArcsineTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public InverseArcsineTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = totalIncrement * 2.0 / Math.PI;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * ( Math.PI / 2 - Math.Asin(1 - time/duration)) + initialValue;
        }

    }
}
