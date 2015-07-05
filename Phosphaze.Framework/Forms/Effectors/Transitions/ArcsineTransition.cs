using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class ArcsineTransition : AbstractTransition
    {

        private double alpha;

        public ArcsineTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public ArcsineTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = 2.0 / Math.PI * totalIncrement;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * Math.Asin(time / duration) + initialValue;
        }
    }
}
