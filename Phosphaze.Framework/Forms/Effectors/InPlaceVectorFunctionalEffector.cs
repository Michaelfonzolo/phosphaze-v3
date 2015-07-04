using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class InPlaceVectorFunctionalEffector : VectorFunctionalEffector
    {

        public InPlaceVectorFunctionalEffector(string attr) : base(attr) { }

        public InPlaceVectorFunctionalEffector(string attr, Form form) : base(attr, form) { }

        protected override Vector2 Operate(Vector2 a, Vector2 b)
        {
            return b; // b is the result of calling Function, so just returning it overrides the
            // previous value.
        }

    }
}
