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

        public ArcsineTransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        public ArcsineTransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = 2.0 / Math.PI * deltaValue;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * Math.Asin(time / duration) + initialValue;
        }
    }
}
