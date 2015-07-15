using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_ADD : IInstruction
    {
        private BIN_ADD() { }
        public static BIN_ADD Instance = new BIN_ADD();

        public override bool Equals(object obj)
        {
            return obj is BIN_ADD; // Only one possible instance.
        }
    }
}
