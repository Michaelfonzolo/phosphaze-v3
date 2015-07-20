using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public static class NeonExceptions
    {

        public static NeonParseException Exception0001(int lineNum)
        {
            return new NeonParseException(String.Format("Unclosed string on line {0}.", lineNum));
        }

        public static NeonParseException Exception0002(char c, int lineNum)
        {
            return new NeonParseException(
                String.Format("Unexpected character '{0}' encountered on line {1}.", c, lineNum)
                );
        }

        public static NeonSyntaxException Exception0003(string token, int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unexpected token \"{0}\" encountered on line {1}.", token, lineNum)
                );
        }

        public static NeonSyntaxException Exception0004(string op, int lineNum)
        {
            return new NeonSyntaxException(
                String.Format(
                "Unexpected operator '{0}' encountered on line {1}. " +
                "Since this operator proceeds another, it is expected " +
                "to be unary, but '{0}' is not.", op, lineNum)
                );
        }

        public static NeonSyntaxException Exception0005(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing bracket on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0006(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening bracket on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0007(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing multiline comment delimiter '*/' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0008(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening multiline comment delimiter '/*' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0009(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing vector delimiter `>>` on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0010(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Vector expression starting on line {0} does not have 2 components.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0011(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing relative vector delimiter '|>' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0012(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Relative vector expression starting on line {0} does not have 2 components.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0013(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening vector delimiter on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0014(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening relative vector delimiter on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0015(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing array delimiter ']' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException Exception0016(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing dict delimiter ']]' on line {0}.", lineNum)
                );
        }

    }
}
