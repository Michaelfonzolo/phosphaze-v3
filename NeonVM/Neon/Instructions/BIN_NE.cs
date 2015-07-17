using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_NE : IInstruction
    {

        private BIN_NE() { }
        public static BIN_NE Instance = new BIN_NE();

    }
}
