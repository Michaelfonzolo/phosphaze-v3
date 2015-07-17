using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BIN_GT : IInstruction
    {

        private BIN_GT() { }
        public static BIN_GT Instance = new BIN_GT();

    }
}
