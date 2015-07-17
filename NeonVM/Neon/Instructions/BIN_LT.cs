using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_LT : IInstruction
    {

        private BIN_LT() { }
        public static BIN_LT Instance = new BIN_LT();

    }
}
