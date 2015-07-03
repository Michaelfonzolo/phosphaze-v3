using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class FunctionalAttributeEffector<T> : Effector
    {

        protected string attrName { get; private set; }

        public FunctionalAttributeEffector(string attr)
            : base()
        {
            attrName = attr;
        }

        public FunctionalAttributeEffector(string attr, Form form)
            : base(form)
        {
            attrName = attr;
        }

        public override void Update(ServiceLocator serviceLocator)
        {
            base.Update(serviceLocator);
            form.Attributes.SetAttr<T>(
                attrName, Operate(
                    form.Attributes.GetAttr<T>(attrName),
                    Function(LocalTime, LocalFrame)
                    )
                );
        }

        protected abstract T Operate(T a, T b);

        protected abstract T Function(double time, int frame);

    }
}
