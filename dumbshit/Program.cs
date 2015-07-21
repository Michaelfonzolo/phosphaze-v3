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
            var things = new Tokenizer("0...5...alpha").Tokenize();
            foreach (var thing in things)
                Console.WriteLine(thing);
        }
    }
}
