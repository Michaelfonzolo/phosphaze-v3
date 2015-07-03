using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class AdditiveIntFunctionalEffector : IntFunctionalEffector
    {
        public AdditiveIntFunctionalEffector(string attr) : base(attr) { }

        public AdditiveIntFunctionalEffector(string attr, Form form) : base(attr, form) { }

        protected override int Operate(int a, int b)
        {
            return a + b;
        }
    }
}
