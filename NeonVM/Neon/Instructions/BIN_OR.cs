using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_OR : IInstruction
    {
        private BIN_OR() { }
        public static BIN_OR Instance = new BIN_OR();

        public override bool Equals(object obj)
        {
            return obj is BIN_OR; // Only one possible instance.
        }
    }
}
