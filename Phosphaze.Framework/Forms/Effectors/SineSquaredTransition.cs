using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class SineSquaredTransition : AbstractPowerSineTransition
    {

        protected override double Power { get { return 2.0; } set; }

        public SineSquaredTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public SineSquaredTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

    }
}
