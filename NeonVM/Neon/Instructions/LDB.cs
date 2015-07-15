using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon.Instructions
{
    public class LDB : IInstruction
    {
        public string name;
        public LDB(string name) { this.name = name; }

        public override bool Equals(object obj)
        {
            var o = obj as LDB;
            return (o == null) ? false : (this.name == o.name);
        }
    }
}
