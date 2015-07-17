using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class BUILD_DICT : IInstruction
    {

        public int Elements { get; private set; }

        public BUILD_DICT(int elements) { Elements = elements; }

    }
}
