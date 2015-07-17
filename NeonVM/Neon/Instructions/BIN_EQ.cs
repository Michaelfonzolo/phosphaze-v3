using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_EQ : IInstruction
    {

        private BIN_EQ() { }
        public static BIN_EQ Instance = new BIN_EQ();

    }
}
