using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_GE : IInstruction
    {

        private BIN_GE() { }
        public static BIN_GE Instance = new BIN_GE();

    }
}
