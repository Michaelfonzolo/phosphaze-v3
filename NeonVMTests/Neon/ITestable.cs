using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonTests.Neon
{
    public interface ITestable
    {
        string Header { get; }
        void Prepare(string[] f_input, string[] f_expected);
        void Run();
    }
}
