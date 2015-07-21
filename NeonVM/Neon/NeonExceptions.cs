using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public static class NeonExceptions
    {

        public static NeonParseException UnclosedString(int lineNum)
        {
            return new NeonParseException(String.Format("Unclosed string on line {0}.", lineNum));
        }

        public static NeonParseException UnexpectedCharacter(char c, int lineNum)
        {
            return new NeonParseException(
                String.Format("Unexpected character '{0}' encountered on line {1}.", c, lineNum)
                );
        }

        public static NeonSyntaxException UnexpectedTokenEncountered(string token, int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unexpected token \"{0}\" encountered on line {1}.", token, lineNum)
                );
        }

        public static NeonSyntaxException UnexpectedOperatorEncountered(string op, int lineNum)
        {
            return new NeonSyntaxException(
                String.Format(
                "Unexpected operator '{0}' encountered on line {1}. " +
                "Since this operator proceeds another, it is expected " +
                "to be unary, but '{0}' is not.", op, lineNum)
                );
        }

        public static NeonSyntaxException BinaryOperatorUsedAsUnary(string op, int lineNum)
        {
            return new NeonSyntaxException(
                String.Format(
                    "The operator '{0}' is binary, but was used " +
                    "like a unary operator on line {1}.", op, lineNum)
                );
        }

        public static NeonSyntaxException MismatchedClosingBracket(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing bracket on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException UnclosedOpeningBracket(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening bracket on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException MismatchedClosingMultilineCommentDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing multiline comment delimiter '*/' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException UnclosedOpeningMultilineCommentDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening multiline comment delimiter '/*' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException MismatchedClosingVectorDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing vector delimiter `>>` on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException InvalidVectorComponentCount(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Vector expression starting on line {0} does not have 2 components.", lineNum)
                );
        }

        public static NeonSyntaxException MismatchedClosingRelativeVectorDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing relative vector delimiter '|>' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException InvalidRelativeVectorComponentCount(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Relative vector expression starting on line {0} does not have 2 components.", lineNum)
                );
        }

        public static NeonSyntaxException UnclosedOpeningVectorDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening vector delimiter on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException UnclosedOpeningRelativeVectorDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Unclosed opening relative vector delimiter on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException MismatchedClosingArrayDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing array delimiter ']' on line {0}.", lineNum)
                );
        }

        public static NeonSyntaxException MismatchedClosingDictionaryDelimiter(int lineNum)
        {
            return new NeonSyntaxException(
                String.Format("Mismatched closing dict delimiter ']]' on line {0}.", lineNum)
                );
        }

    }
}
