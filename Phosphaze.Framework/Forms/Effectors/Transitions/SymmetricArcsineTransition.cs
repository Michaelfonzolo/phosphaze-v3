using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class SymmetricArcsineTransition : AbstractTransition
    {

        private double alpha;

        public SymmetricArcsineTransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        public SymmetricArcsineTransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = deltaValue / Math.PI;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * (Math.PI / 2 - Math.Asin(1 - 2 * time / duration)) + initialValue;
        }

    }
}
