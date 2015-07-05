using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class PowerTransition : AbstractTransition
    {

        private double power;

        private double alpha;

        public PowerTransition(
            string attr
            , double totalIncrement
            , double duration
            , double power
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) 
        {
            this.power = power;
        }

        public PowerTransition(
            string attr
            , double totalIncrement
            , double duration
            , double power
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative)
        {
            this.power = power;
        }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = Math.Pow(deltaValue, 1.0 / power) / duration;
        }

        protected override double Function(double time, int frame)
        {
            return Math.Pow(alpha * time, power) + initialValue;
        }

    }
}
