using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class SineSquaredTransition : AbstractPowerSineTransition
    {

        protected override double Power { get { return 2.0; } set { throw new NotImplementedException(); } }

        public SineSquaredTransition(
            string attr
            , double totalIncrement
            , double duration
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) { }

        public SineSquaredTransition(
            string attr
            , double totalIncrement
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative) { }

    }
}
