using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public abstract class AbstractTransition : InPlaceDoubleFunctionalEffector
    {

        public double totalIncrement { get; private set; }

        public double initialValue { get; private set; }

        public double finalValue { get; private set; }

        public double duration { get; private set; }

        public bool increasing { get; private set; }

        public AbstractTransition(string attr, double totalIncrement, double duration)
            : base(attr) 
        {
            this.totalIncrement = totalIncrement;
            this.duration = duration;
        }

        public AbstractTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, form)
        {
            this.totalIncrement = totalIncrement;
            this.duration = duration;
        }

        protected override void Initialize()
        {
            initialValue = form.Attributes.GetAttr<double>(attrName);
            finalValue = totalIncrement + initialValue;
            increasing = totalIncrement > 0;
        }

        public override void Update(ServiceLocator serviceLocator)
        {
            base.Update(serviceLocator);

            if (LocalTime >= duration)
            {
                form.Attributes.SetAttr<double>(attrName, finalValue);
                Kill();
            }
        }

    }
}
