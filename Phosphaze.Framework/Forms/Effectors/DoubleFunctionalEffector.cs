using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class DoubleFunctionalEffector : AbstractFunctionalAttributeEffector<double> 
    {
        public DoubleFunctionalEffector(string attr) : base(attr) { }

        public DoubleFunctionalEffector(string attr, Form form) : base(attr, form) { }
    }
}
