using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class AdditiveVectorFunctionalEffector : VectorFunctionalEffector
    {
        protected override Vector2 Operate(Vector2 a, Vector2 b)
        {
            return a + b;
        }
    }
}
