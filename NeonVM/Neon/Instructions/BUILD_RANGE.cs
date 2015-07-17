using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BUILD_RANGE : IInstruction
    {

        private BUILD_RANGE() { }
        public static BUILD_RANGE Instance = new BUILD_RANGE();

    }
}
