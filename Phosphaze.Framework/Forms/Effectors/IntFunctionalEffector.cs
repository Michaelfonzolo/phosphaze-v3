using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class IntFunctionalEffector : AbstractFunctionalAttributeEffector<int> 
    {
        public IntFunctionalEffector(string attr) : base(attr) { }

        public IntFunctionalEffector(string attr, Form form) : base(attr, form) { }
    }
}
