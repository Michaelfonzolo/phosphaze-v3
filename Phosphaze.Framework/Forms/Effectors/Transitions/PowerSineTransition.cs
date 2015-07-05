using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class PowerSineTransition : AbstractPowerSineTransition
    {

        protected override double Power { get; set; }

        public PowerSineTransition(
            string attr
            , double totalIncrement
            , double duration
            , double power
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) 
        {
            Power = power;
        }

        public PowerSineTransition(
            string attr
            , double totalIncrement
            , double duration
            , double power
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative) 
        {
            Power = power;
        }

    }
}
