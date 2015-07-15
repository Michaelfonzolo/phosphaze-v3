using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class LDL : IInstruction
    {
        public string name;
        public LDL(string name) { this.name = name; }

        public override bool Equals(object obj)
        {
            var o = obj as LDL;
            return (o == null) ? false : (this.name == o.name);
        }
    }
}
