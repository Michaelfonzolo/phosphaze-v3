using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class UN_NEG : IInstruction
    {
        private UN_NEG() { }
        public static UN_NEG Instance = new UN_NEG();

        public override bool Equals(object obj)
        {
            return obj is UN_NEG; // Only one possible instance.
        }
    }
}
