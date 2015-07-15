using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NeonVM.Neon;

namespace dumbshit
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {

            var s = new ShuntingYard(new List<string>(new string[] {  }));
            s.Parse();
            foreach (var i in s.GetInstructions())
                Console.WriteLine(i);
        }
    }
}
