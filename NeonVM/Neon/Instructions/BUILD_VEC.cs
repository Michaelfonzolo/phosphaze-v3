using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BUILD_VEC : IInstruction
    {

        private BUILD_VEC() { }
        public static BUILD_VEC Instance = new BUILD_VEC();

    }
}
