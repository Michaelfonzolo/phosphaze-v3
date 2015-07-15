using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BUILD_RVEC : IInstruction
    {

        private BUILD_RVEC() { }
        public static BUILD_RVEC Instance = new BUILD_RVEC();

    }
}
