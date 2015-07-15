using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_SUB : IInstruction
    {
        private BIN_SUB() { }
        public static BIN_SUB Instance = new BIN_SUB();

        public override bool Equals(object obj)
        {
            return obj is BIN_SUB; // Only one possible instance.
        }
    }
}
