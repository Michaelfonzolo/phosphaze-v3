using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class LDC : IInstruction
    {
        public NeonObject Value;
        public LDC(NeonObject val) { Value = val; }

        public override bool Equals(object obj)
        {
            return obj is LDC; // Temporary
            // var o = obj as LDC;
            // return (o == null) ? false : (this.Value == o.Value);
        }
    }
}
