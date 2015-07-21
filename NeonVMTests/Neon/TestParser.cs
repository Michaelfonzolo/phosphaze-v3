using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeonVM.Neon;
using NeonVM.Neon.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeonVMTests.Neon
{
    public class TestParser<T> where T : ITestable, new()
    {

        private string defaultDir = null;

        private static string _up2Dirs(string path)
        {
            var up_one = Directory.GetParent(path).FullName;
            return Directory.GetParent(up_one).FullName;
        }

        private static string _getFileName(string subdir, string file)
        {
            var top = _up2Dirs(AppDomain.CurrentDomain.BaseDirectory);
            return Path.Combine(top, Path.Combine(subdir, file));
        }

        public void SetDefaultDirectory(string dir)
        {
            defaultDir = dir;
        }

        public void Run(string testName)
        {
            if (defaultDir == null)
                throw new ArgumentException(
                    "No directory given; a directory has to be supplied as " +
                    "a parameter or set using SetDefaultDirectory."
                    );
            Run(defaultDir, testName);
        }

        public void Run(string subdir, string testName)
        {
            T testSuite = new T();

            var reader = new StreamReader(_getFileName(subdir, testName));
            var header = reader.ReadLine();
            if (!header.StartsWith("## "))
                throw new TestParserException(
                    "Invalid test file. Expected test-type header as first line."
                    );
            var testType = header.Substring(3);

            if (testSuite.Header != testType)
                throw new TestParserException(
                    String.Format("Invalid test file. Expected test type {0}.", testSuite.Header)
                    );

            var inputHeader = reader.ReadLine();
            if (inputHeader != "## INPUT")
                throw new TestParserException(
                    "Invalid test file. Expected input header as second line."
                    );

            var f_input = new List<string>();
            var line = reader.ReadLine();
            while (!line.StartsWith("## "))
            {
                f_input.Add(line);
                line = reader.ReadLine();
            }

            if (line != "## OUTPUT")
                throw new TestParserException(
                    "Invalid test file. Expected output header after input header."
                    );

            var f_expected = new List<string>();
            line = reader.ReadLine();
            while (line != null)
            {
                f_expected.Add(line);
                line = reader.ReadLine();
            }

            testSuite.Prepare(f_input.ToArray(), f_expected.ToArray());

            testSuite.Run();
        }
    }
}
