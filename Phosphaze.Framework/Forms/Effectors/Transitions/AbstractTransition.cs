using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public abstract class AbstractTransition : InPlaceDoubleFunctionalEffector
    {

        public double deltaValue { get; private set; }

        public double initialValue { get; private set; }

        public double finalValue { get; private set; }

        public double duration { get; private set; }

        public bool increasing { get; private set; }

        private double y;

        private bool relative;

        public AbstractTransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr) 
        {
            y = finalValue;
            this.duration = duration;
            this.relative = relative;
        }

        public AbstractTransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, form)
        {
            y = finalValue;
            this.duration = duration;
            this.relative = relative;
        }

        protected override void Initialize()
        {
            initialValue = form.Attributes.GetAttr<double>(attrName);
            if (relative)
            {
                deltaValue = y;
                increasing = deltaValue > 0;
            }
            else
            {
                deltaValue = y - initialValue;
                increasing = y > initialValue;
            }
            finalValue = deltaValue + initialValue;
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
