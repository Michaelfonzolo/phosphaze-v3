using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class QuadraticTransition : AbstractTransition
    {

        private double alpha;

        public QuadraticTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public QuadraticTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = totalIncrement / duration / duration;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * time * time + initialValue;
        }

    }
}
