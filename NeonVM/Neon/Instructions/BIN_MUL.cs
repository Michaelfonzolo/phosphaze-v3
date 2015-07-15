using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_MUL : IInstruction
    {
        private BIN_MUL() { }
        public static BIN_MUL Instance = new BIN_MUL();

        public override bool Equals(object obj)
        {
            return obj is BIN_MUL; // Only one possible instance.
        }
    }
}
