using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public class EllipticTransition : AbstractTransition
    {

        public EllipticTransition(string attr, double totalIncrement, double duration)
            : base(attr, totalIncrement, duration) { }

        public EllipticTransition(string attr, double totalIncrement, double duration, Form form)
            : base(attr, totalIncrement, duration, form) { }

    }
}
