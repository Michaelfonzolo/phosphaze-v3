using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_LE : IInstruction
    {

        private BIN_LE() { }
        public static BIN_LE Instance = new BIN_LE();

    }
}
