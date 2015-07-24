using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BUILD_KVP : IInstruction
    {
        private BUILD_KVP() { }
        public static BUILD_KVP Instance = new BUILD_KVP();
    }
}
