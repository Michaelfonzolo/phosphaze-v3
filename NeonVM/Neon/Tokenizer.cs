using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NeonVM.Neon
{
    public class Tokenizer
    {

        private static char[] SingleCharTokens = new char[]
        {
            Tokens.BIN_ADD[0],
            Tokens.BIN_SUB[0],
            Tokens.BIN_MUL[0],
            Tokens.BIN_DIV[0],
            Tokens.BIN_POW[0],
            Tokens.BIN_MOD[0],
            Tokens.EXPR_START[0],
            Tokens.EXPR_END[0],
            Tokens.CALL_START[0],
            Tokens.CALL_END[0],
            Tokens.BLOCK_START[0],
            Tokens.BLOCK_END[0],
            Tokens.ARRAY_START[0],
            Tokens.ARRAY_END[0],
            Tokens.ELEM_SEP[0],
            Tokens.EOL[0],
            Tokens.KEY_VAR_CONN[0],
            Tokens.TIME_COMMAND[0],
            Tokens.DURING_INTERVALS[0],
            Tokens.LAMBDA_INDICATOR[0],
            Tokens.UNUSED[0],
            Tokens.UN_NOT[0],
            '\\'
        };

        private class PolyglyphChecker
        {
            private string[] previousChars;

            public PolyglyphChecker(params string[] previousChars)
            {
                this.previousChars = previousChars;
            }

            public void Perform(
                char currentChar, string currentStr, StringBuilder currentToken, List<StringBuilder> tokens)
            {
                if (previousChars.Contains(currentStr))
                    currentToken.Append(currentChar);
                else
                    tokens.Add(new StringBuilder(currentChar.ToString()));
            }
        }

        private static Dictionary<char, PolyglyphChecker> polyglyphs =
            new Dictionary<char, PolyglyphChecker>()
            {
                {'*', new PolyglyphChecker("/")},
                {'&', new PolyglyphChecker("&")},
                {'<', new PolyglyphChecker("<")},
                {'[', new PolyglyphChecker("[")},
                {']', new PolyglyphChecker("]")},
                {'|', new PolyglyphChecker("<", "|")},
                {'/', new PolyglyphChecker("/", "*")},
                {'>', new PolyglyphChecker("-", "=", "|", ">")},
                {'=', new PolyglyphChecker("=", "<", ">", "!", "+", "-", "*", "/", "^", "%", "&&", "||")}
            };

        string str;

        private List<StringBuilder> tokens;

        private int lineNumber = 0;

        private StringBuilder CurrentToken { get { return tokens.Last(); } }

        private void NewToken() { tokens.Add(new StringBuilder()); }

        private void NewToken(char c) { tokens.Add(new StringBuilder(c.ToString())); }

        public Tokenizer(string str)
        {
            this.str = str;
            tokens = new List<StringBuilder>();
            // We have to initialize tokens with an empty StringBuilder otherwise
            // the first call to get_CurrentToken will throw a NullReferenceException.
            tokens.Add(new StringBuilder());
        }

        private bool IsInteger(string str)
        {
            Predicate<char> pred = c => Char.IsDigit(c);
            return Array.TrueForAll<char>(str.ToCharArray(), pred);
        }

        private bool IsTruncatedDecimal(string str)
        {
            if (str.Length > 1)
                return str.Last() == '.' && IsInteger(str.Substring(0, str.Length - 1));
            return str.Length == 1 ? Char.IsDigit(str[0]) : false;
        }

        public List<string> Tokenize()
        {
            bool parsingString = false;
            var _string = new StringBuilder();
            char c;
            string currentString;

            for (int i = 0; i < str.Length; i++)
            {
                c = str[i];
                currentString = CurrentToken.ToString();

                if (parsingString)
                {
                    if (c == '\n')
                    {
                        throw NeonExceptions.UnclosedString(lineNumber);
                    }
                    _string.Append(c);
                    if (c == '"' && str[i - 1] != '\\')
                    {
                        parsingString = false;
                        tokens.Add(_string);
                        _string = new StringBuilder();
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        parsingString = true;
                        _string.Append(c);
                    }
                    else if (Char.IsLetterOrDigit(c))
                    {
                        if (CurrentToken.Length == 0 ||
                            Regex.IsMatch(currentString, Tokens.WORD) ||
                            (Regex.IsMatch(currentString, Tokens.NUMBER) && Char.IsDigit(c)) ||
                            (IsTruncatedDecimal(currentString) && Char.IsDigit(c)))
                        {
                            CurrentToken.Append(c);
                        }
                        else
                        {
                            NewToken(c);
                        }
                    }
                    else if (c == '\n')
                    {
                        lineNumber++;
                        NewToken(c);
                    }
                    else if (Char.IsWhiteSpace(c))
                    {
                        if (CurrentToken.Length != 0)
                        {
                            NewToken();
                        }
                    }
                    else if (c == '.')
                    {
                        if (IsInteger(currentString))
                        {
                            CurrentToken.Append(c);
                        }
                        else if (IsTruncatedDecimal(currentString))
                        {
                            CurrentToken.Remove(currentString.Length - 1, 1);
                            NewToken(c);
                            NewToken(c);
                        }
                        else if (tokens.Count > 1
                            && tokens[tokens.Count - 1].ToString() == tokens[tokens.Count - 2].ToString()
                            && currentString == ".")
                        {
                            tokens.RemoveAt(tokens.Count - 1);
                            tokens.RemoveAt(tokens.Count - 1);
                            tokens.Add(new StringBuilder("..."));
                        }
                        else
                        {
                            NewToken(c);
                        }
                    }
                    else if (polyglyphs.ContainsKey(c))
                    {
                        polyglyphs[c].Perform(c, currentString, CurrentToken, tokens);
                    }
                    else if (SingleCharTokens.Contains(c))
                    {
                        NewToken(c);
                    }
                    else
                    {
                        throw NeonExceptions.UnexpectedCharacter(c, lineNumber);
                    }
                }
            }

            var cleanTokens = from token in tokens
                              where token.Length > 0
                              select token.ToString();

            return new List<string>(cleanTokens);
        }

    }
}
