using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class LinearTransition : AdditiveDoubleFunctionalEffector
    {

        public double totalIncrement { get; private set; }

        public double duration { get; private set; }

        private double increment;

        public LinearTransition(string name, double totalIncrement, double duration)
            : base(name)
        {
            this.totalIncrement = totalIncrement;
            this.duration = duration;
            increment = totalIncrement / duration * Constants.MIN_DTIME;
        }

        public LinearTransition(string name, double totalIncrement, double duration, Form form)
            : base(name, form)
        {
            this.totalIncrement = totalIncrement;
            this.duration = duration;
            increment = totalIncrement / duration * Constants.MIN_DTIME;
        }

        protected override double Function(double time, int frame)
        {
            return increment;
        }

    }
}
