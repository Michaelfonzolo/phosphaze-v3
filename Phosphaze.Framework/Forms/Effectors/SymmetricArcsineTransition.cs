using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class SymmetricArcsineTransition : AbstractTransition
    {

        private double alpha;

        public SymmetricArcsineTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public SymmetricArcsineTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = totalIncrement / Math.PI;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * (Math.PI / 2 - Math.Asin(1 - 2 * time / duration));
        }

    }
}
