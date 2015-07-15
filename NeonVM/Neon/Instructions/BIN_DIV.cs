using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_DIV : IInstruction
    {
        private BIN_DIV() { }
        public static BIN_DIV Instance = new BIN_DIV();

        public override bool Equals(object obj)
        {
            return obj is BIN_DIV; // Only one possible instance.
        }
    }
}
