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
            var s = new ShuntingYardParser(new List<string>(new string[] { "ORIGIN", "+", "<|", "1", ",", "<<", "3", ",", "5", ">>", "|>" }));
            s.Parse();
            foreach (var i in s.GetInstructions())
                Console.WriteLine(i);
        }
    }
}
