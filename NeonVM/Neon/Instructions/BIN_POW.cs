using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_POW : IInstruction
    {
        private BIN_POW() { }
        public static BIN_POW Instance = new BIN_POW();

        public override bool Equals(object obj)
        {
            return obj is BIN_POW; // Only one possible instance.
        }
    }
}
