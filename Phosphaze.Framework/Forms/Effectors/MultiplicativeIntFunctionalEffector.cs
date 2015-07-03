using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class MultiplicativeIntFunctionalEffector : IntFunctionalEffector
    {
        public MultiplicativeIntFunctionalEffector(string attr) : base(attr) { }

        public MultiplicativeIntFunctionalEffector(string attr, Form form) : base(attr, form) { }

        protected override int Operate(int a, int b)
        {
            return a * b;
        }
    }
}
