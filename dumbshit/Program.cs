using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            var syp = new ShuntingYardParser(new List<string>() { "<<", "1", ",", "2", ",", "3", ">>" });
            syp.Parse();
            var things = syp.GetInstructions();
            foreach (var thing in things)
                Console.WriteLine(thing);
        }
    }
}
