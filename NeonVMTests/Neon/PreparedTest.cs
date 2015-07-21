using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTests.Neon
{
    public abstract class PreparedTest<TI, TE> : ITestable
    {
        public TI Input { get; protected set; }
        public TE Expected { get; protected set; }

        public abstract string Header { get; }
        public abstract void Prepare(string[] f_input, string[] f_expected);
        public abstract void Run();
    }
}
