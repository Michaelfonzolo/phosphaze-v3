using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class VectorFunctionalEffector : FunctionalAttributeEffector<Vector2> 
    {
        public VectorFunctionalEffector(string attr) : base(attr) { }

        public VectorFunctionalEffector(string attr, Form form) : base(attr, form) { }
    }
}
