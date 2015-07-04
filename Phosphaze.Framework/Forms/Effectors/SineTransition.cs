using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class SineTransition : AbstractTransition
    {

        protected override double Power { get { return 1.0; } set; }

        public SineTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public SineTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

    }
}
