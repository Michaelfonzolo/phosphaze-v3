using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class StaircaseTransition : AbstractTransition
    {

        public int steps { get; private set; }

        public StaircaseTransition(
            string attr
            , double totalIncrement
            , double duration
            , int steps
            , bool relative = true)
            : base(attr, totalIncrement, duration, relative) 
        {
            this.steps = steps;
        }

        public StaircaseTransition(
            string attr
            , double totalIncrement
            , double duration
            , int steps
            , Form form
            , bool relative = true)
            : base(attr, totalIncrement, duration, form, relative) 
        {
            this.steps = steps;
        }

        protected override double Function(double time, int frame)
        {
            return Math.Floor(steps * time / duration);
        }
    }
}
