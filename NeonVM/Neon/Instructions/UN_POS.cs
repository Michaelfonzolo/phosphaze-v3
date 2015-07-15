using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class UN_POS : IInstruction
    {
        private UN_POS() { }
        public static UN_POS Instance = new UN_POS();

        public override bool Equals(object obj)
        {
            return obj is UN_POS; // Only one possible instance.
        }
    }
}
