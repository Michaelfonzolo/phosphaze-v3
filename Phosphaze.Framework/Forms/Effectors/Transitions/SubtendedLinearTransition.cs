using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class SubtendedLinearTransition : AdditiveDoubleFunctionalEffector
    {
        private double initialValue;

        public double finalValue { get; private set; }

        public double duration { get; private set; }

        private double increment;

        private bool increasing;

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
            initialValue = form.Attributes.GetAttr<double>(attrName);
            increment = (finalValue - initialValue) / duration * Constants.MIN_DTIME;
            increasing = increment > 0;
        }

        public override void Update(ServiceLocator serviceLocator)
        {
            base.Update(serviceLocator);
            var currentVal = form.Attributes.GetAttr<double>(attrName);
            if ((increasing && (currentVal >= finalValue)) || currentVal <= finalValue)
            {
                form.Attributes.SetAttr<double>(attrName, finalValue);
                Kill();
            }
        }

        protected override double Function(double time, int frame)
        {
            return increment;
        }

    }
}
