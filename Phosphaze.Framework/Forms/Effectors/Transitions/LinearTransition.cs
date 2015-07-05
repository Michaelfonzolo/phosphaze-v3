using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class LinearTransition : AdditiveDoubleFunctionalEffector
    {

        private double finalValue;

        public double totalIncrement { get; private set; }

        public double duration { get; private set; }

        private double increment;

        private bool increasing;

        public LinearTransition(string name, double totalIncrement, double duration)
            : base(name)
        {
            _init(totalIncrement, duration);
        }

        public LinearTransition(string name, double totalIncrement, double duration, Form form)
            : base(name, form)
        {
            _init(totalIncrement, duration);
        }

        private void _init(double totalIncrement, double duration)
        {
            this.totalIncrement = totalIncrement;
            this.duration = duration;
            increment = totalIncrement / duration * Constants.MIN_DTIME;
            increasing = totalIncrement > 0;
        }

        protected override void Initialize()
        {
            finalValue = form.Attributes.GetAttr<double>(attrName) + totalIncrement;
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
