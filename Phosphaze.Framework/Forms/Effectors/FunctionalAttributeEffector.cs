using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class FunctionalAttributeEffector : InPlaceDoubleFunctionalEffector
    {

        public Func<double, double> func { get; private set; }

        public double initialValue { get; private set; }

        public FunctionalAttributeEffector(string attribute, Func<double, double> func)
            : base(attribute)
        {
            this.func = func;
        }

        public FunctionalAttributeEffector(string attribute, Func<double, double> func, Form form)
            : base(attribute, form)
        {
            this.func = func;
        }

        protected override void Initialize()
        {
            base.Initialize();
            initialValue = form.Attributes.GetAttr<double>(attrName);
        }

        protected override double Function(double time, int frame)
        {
            return func(time) + initialValue;
        }

    }
}
