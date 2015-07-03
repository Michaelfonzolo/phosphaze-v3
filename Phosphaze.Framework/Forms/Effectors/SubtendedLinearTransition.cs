using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class SubtendedLinearTransition : AdditiveDoubleFunctionalEffector
    {

        public double finalValue { get; private set; }

        public double duration { get; private set; }

        private double increment;

        public SubtendedLinearTransition(string name, double totalIncrement, double duration)
            : base(name)
        {
            _init(totalIncrement, duration);
        }

        public SubtendedLinearTransition(string name, double totalIncrement, double duration, Form form)
            : base(name, form)
        {
            _init(totalIncrement, duration);
        }

        private void _init(double final, double duration)
        {
            finalValue = final;
            this.duration = duration;
        }

        protected override void Initialize()
        {
            var start = form.Attributes.GetAttr<double>(attrName);
            increment = (finalValue - start) / duration * Constants.MIN_DTIME;
        }

        protected override double Function(double time, int frame)
        {
            return increment;
        }

    }
}
