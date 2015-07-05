using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class SineTransition : AbstractPowerSineTransition
    {

        protected override double Power { get { return 1.0; } set { throw new NotImplementedException(); } }

        public SineTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public SineTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

    }
}
