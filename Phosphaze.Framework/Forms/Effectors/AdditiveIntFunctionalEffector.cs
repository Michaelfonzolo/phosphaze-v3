using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class AdditiveIntFunctionalEffector : IntFunctionalEffector
    {
        protected override int Operate(int a, int b)
        {
            return a + b;
        }
    }
}
