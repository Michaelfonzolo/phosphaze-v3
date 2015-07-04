using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class PowerSineTransition : AbstractPowerSineTransition
    {

        protected override double Power { get; set; }

        public PowerSineTransition(string attr, double totalIncrement, double duration, double power)
            : base(attr, totalIncrement, duration) 
        {
            Power = power;
        }

        public PowerSineTransition(string attr, double totalIncrement, double duration, double power, Form form)
            : base(attr, totalIncrement, duration, form) 
        {
            Power = power;
        }

    }
}
