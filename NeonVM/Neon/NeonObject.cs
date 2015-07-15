using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public class NeonObject
    {

        public override bool Equals(object obj)
        {
            return obj is NeonObject; // Temporary
        }

    }
}
