using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BUILD_ARRAY : IInstruction
    {

        public int Elements { get; private set; }

        private BUILD_ARRAY(int elements) { Elements = elements; }

    }
}
