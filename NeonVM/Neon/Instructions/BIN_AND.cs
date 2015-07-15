using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_AND : IInstruction
    {
        private BIN_AND() { }
        public static BIN_AND Instance = new BIN_AND();

        public override bool Equals(object obj)
        {
            return obj is BIN_AND; // Only one possible instance.
        }
    }
}
