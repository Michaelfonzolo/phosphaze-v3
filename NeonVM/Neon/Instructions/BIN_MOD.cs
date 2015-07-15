using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_MOD : IInstruction
    {
        private BIN_MOD() { }
        public static BIN_MOD Instance = new BIN_MOD();

        public override bool Equals(object obj)
        {
            return obj is BIN_MOD; // Only one possible instance.
        }
    }
}
