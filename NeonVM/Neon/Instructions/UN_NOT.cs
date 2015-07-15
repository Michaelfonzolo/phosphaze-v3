using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class UN_NOT : IInstruction
    {
        private UN_NOT() { }
        public static UN_NOT Instance = new UN_NOT();

        public override bool Equals(object obj)
        {
            return obj is UN_NOT; // Only one possible instance.
        }
    }
}
