using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NeonVM.Neon.Instructions;

namespace NeonVM.Neon
{
    public class ShuntingYardParser
    {

        /// <summary>
        /// A struct representing a token and the line number it was encountered on.
        /// </summary>
        private struct TokenWithLineNo
        {
            public string Token { get; set; }
            public int LineNumber { get; set; }
        }

        private enum ExpectantTokenType
        {

        }

        // --= STATIC OPERATOR-RELATED FIELDS =--
        // ======================================

        private static readonly string[] AMBIGUOUS_OPERATORS = new string[]
        {
            Tokens.BIN_ADD,
            Tokens.BIN_SUB
        };

        /// <summary>
        /// A list of all operator instructions, ordered by precedence (least to greatest).
        /// </summary>
        private static readonly IInstruction[] OP_INSTR_ORDERED_BY_PRECEDENCE
            = new IInstruction[]
        {
            BIN_OR.Instance,
            BIN_AND.Instance,
            BIN_EQ.Instance,
            BIN_NE.Instance,
            BIN_LT.Instance,
            BIN_GT.Instance,
            BIN_LE.Instance,
            BIN_GE.Instance,
            BIN_MOD.Instance,
            BIN_ADD.Instance,
            BIN_SUB.Instance,
            BIN_MUL.Instance,
            BIN_DIV.Instance,
            BIN_POW.Instance,
            BUILD_RANGE.Instance,
            UN_NOT.Instance,
            UN_NEG.Instance,
            UN_POS.Instance
        };

        /// <summary>
        /// A dictionary mapping binary operator tokens to their precedence, which is also their
        /// index in OP_INSTR_ORDERED_BY_PRECEDENCE.
        /// </summary>
        private static readonly Dictionary<string, int> BIN_OP_TOKEN_TO_PREC
            = new Dictionary<string, int>()
        {
            {Tokens.BIN_OR, 0},
            {Tokens.BIN_AND, 1},
            {Tokens.BOOL_EQ, 2},
            {Tokens.BOOL_NE, 3},
            {Tokens.BOOL_LT, 4},
            {Tokens.BOOL_GT, 5},
            {Tokens.BOOL_LE, 6},
            {Tokens.BOOL_GE, 7},
            {Tokens.BIN_MOD, 8},
            {Tokens.BIN_ADD, 9},
            {Tokens.BIN_SUB, 10},
            {Tokens.BIN_MUL, 11},
            {Tokens.BIN_DIV, 12},
            {Tokens.BIN_POW, 13},
            {Tokens.DISCRETE_RANGE_GEN, 14}
        };

        /// <summary>
        /// A dictionary mapping unary operator tokens to their precedence, which is also their
        /// index in OP_INSTR_ORDERED_BY_PRECEDENCE.
        /// </summary>
        private static readonly Dictionary<string, int> UN_OP_TOKEN_TO_PREC
            = new Dictionary<string, int>()
        {
            {Tokens.UN_NOT, 15},
            {Tokens.INTERNAL_UN_NEG, 16},
            {Tokens.INTERNAL_UN_POS, 17}
        };

        /// <summary>
        /// A list of the indices of operators in OP_INSTR_ORDERED_BY_PRECEDENCE that are left associative.
        /// 
        /// An operator '~' is left associative if a ~ b ~ c == (a ~ b) ~ c.
        /// </summary>
        private static readonly int[] LEFT_ASSOCIATIVE_OP_INDICES
            = new int[]
        {
            0, 1, 2, 3, 8, 9, 10, 11, 12
        };

        /// <summary>
        /// A list of the indices of operators in OP_INSTR_ORDERED_BY_PRECEDENCE that are right associative.
        /// 
        /// An operator '~' is right associative if a ~ b ~ c == a ~ (b ~ c).
        /// </summary>
        private static readonly int[] RIGHT_ASSOCIATIVE_OP_INDICES
            = new int[]
        {
            13, 14, 15, 16, 17
        };

        /// <summary>
        /// A list of the indices of operators in OP_INSTR_ORDERED_BY_PRECEDENCE that are non-associative.
        /// 
        /// An operator '~' is non-associative if the expression a ~ b ~ c is undefined.
        /// </summary>
        private static readonly int[] NON_ASSOCIATIVE_OP_INDICES
            = new int[]
        {
            4, 5, 6, 7
        };

        /// <summary>
        /// A mapping of operator tokens to their "internal" form. For more information on internal tokens,
        /// see the Tokens static class.
        /// </summary>
        private static readonly Dictionary<string, string> OP_TOKEN_TO_INTERNAL_TOKEN =
            new Dictionary<string, string>()
        {
            {Tokens.UN_POS, Tokens.INTERNAL_UN_POS},
            {Tokens.UN_NEG, Tokens.INTERNAL_UN_NEG}
        };

        /// <summary>
        /// A mapping of left bracket tokens to their associated right bracket token.
        /// </summary>
        private static readonly Dictionary<string, string> LEFT_TO_RIGHT_BRACKETS =
            new Dictionary<string, string>()
        {
            {Tokens.EXPR_LEFT,  Tokens.EXPR_RIGHT},
            {Tokens.ARRAY_LEFT, Tokens.ARRAY_RIGHT},
            {Tokens.DICT_LEFT,  Tokens.DICT_RIGHT},
            {Tokens.VEC_LEFT,   Tokens.VEC_RIGHT},
            {Tokens.RVEC_LEFT,  Tokens.RVEC_RIGHT}
        };

        /// <summary>
        /// A mapping of right bracket tokens to their associated left bracket token.
        /// </summary>
        private static readonly Dictionary<string, string> RIGHT_TO_LEFT_BRACKETS =
            new Dictionary<string, string>()
        {
            {Tokens.EXPR_RIGHT,  Tokens.EXPR_LEFT},
            {Tokens.ARRAY_RIGHT, Tokens.ARRAY_LEFT},
            {Tokens.DICT_RIGHT,  Tokens.DICT_LEFT},
            {Tokens.VEC_RIGHT,   Tokens.VEC_LEFT},
            {Tokens.RVEC_RIGHT,  Tokens.RVEC_LEFT}
        };

        // TO DO: Make a "BiDirectionalMapping" class to compress these two dicts. 

        private static readonly Dictionary<string, ParsingStateType?> LEFT_BRACKET_TO_PARSING_STATE =
            new Dictionary<string, ParsingStateType?>()
        {
            {Tokens.EXPR_LEFT,  null},
            {Tokens.ARRAY_LEFT, ParsingStateType.Array},
            {Tokens.DICT_LEFT,  ParsingStateType.Dictionary},
            {Tokens.VEC_LEFT,   ParsingStateType.Vector},
            {Tokens.RVEC_LEFT,  ParsingStateType.RelativeVector}
        };

        private static readonly Dictionary<ParsingStateType, string> PARSING_STATE_TO_LEFT_BRACKET =
            new Dictionary<ParsingStateType, string>()
        {
            {ParsingStateType.Array,          Tokens.ARRAY_LEFT},
            {ParsingStateType.Dictionary,     Tokens.DICT_LEFT},
            {ParsingStateType.Vector,         Tokens.VEC_LEFT},
            {ParsingStateType.RelativeVector, Tokens.RVEC_LEFT}
        };

        private struct INT_FUNC_PAIR
        {
            public int count { get; set; }
            public Func<int, NeonSyntaxException> exception { get; set; }
        }

        private static readonly Dictionary<string, INT_FUNC_PAIR> BRACKETS_THAT_REQUIRE_SPECIFIC_ELEM_COUNT =
            new Dictionary<string, INT_FUNC_PAIR>()
        {
            {Tokens.VEC_LEFT,  new INT_FUNC_PAIR() {count=2, exception=NeonExceptions.InvalidVectorComponentCount}},
            {Tokens.RVEC_LEFT, new INT_FUNC_PAIR() {count=2, exception=NeonExceptions.InvalidRelativeVectorComponentCount}}
        };

        private static readonly Dictionary<string, Func<int, IInstruction>> RIGHT_BRACKET_FINALIZATION_INSTRS =
            new Dictionary<string, Func<int, IInstruction>>()
        {
            {Tokens.ARRAY_RIGHT, (i) => new BUILD_ARRAY(i)},
            {Tokens.DICT_RIGHT,  (i) => new BUILD_DICT(i)},
            {Tokens.VEC_RIGHT,   (i) => BUILD_VEC.Instance},
            {Tokens.RVEC_RIGHT,  (i) => BUILD_RVEC.Instance}
        };

        /// <summary>
        /// A mapping from right brackets to their associated exception to throw when they are mismatched
        /// (i.e. when the left bracket on the top of the bracket stack isn't the associated left bracket
        /// of the encountered right bracket). For example, if ")" was encountered, but the top of the
        /// bracket stack contained "[", then the exception for ")" in this dictionary would be thrown.
        /// </summary>
        private static readonly Dictionary<string, Func<int, NeonSyntaxException>> RIGHT_BRACKET_MISMATCH_EXCEPTIONS =
            new Dictionary<string, Func<int, NeonSyntaxException>>()
        {
            {Tokens.EXPR_RIGHT,  NeonExceptions.MismatchedClosingBracket},
            {Tokens.ARRAY_RIGHT, NeonExceptions.MismatchedClosingArrayDelimiter},
            {Tokens.DICT_RIGHT,  NeonExceptions.MismatchedClosingDictionaryDelimiter},
            {Tokens.VEC_RIGHT,   NeonExceptions.MismatchedClosingVectorDelimiter},
            {Tokens.RVEC_RIGHT,  NeonExceptions.MismatchedClosingRelativeVectorDelimiter},
        };

        /// <summary>
        /// The token that gets pushed onto the operatorStack in place of an opening bracket (any opening bracket).
        /// It doesn't have to be distinct for each bracket since if we reach one closing bracket, we will never have
        /// to parse tokens beyond its associated opening bracket (e.g. when ")" is encountered, we only have to pop
        /// ops until "(" is encountered, and if any other bracket is encountered then an appropriate error will be
        /// thrown beforehand (the bracket's associated exception in BRACKET_MISMATCH_EXCEPTIONS)).
        /// </summary>
        private static string BRACKET_TERMINAL_TOKEN = "##";

        /// <summary>
        /// Return the precedence of a token.
        /// </summary>
        private static int Prec(string op_token)
        {
            if (BIN_OP_TOKEN_TO_PREC.ContainsKey(op_token))
                return BIN_OP_TOKEN_TO_PREC[op_token];
            return UN_OP_TOKEN_TO_PREC.ContainsKey(op_token)
                   ? UN_OP_TOKEN_TO_PREC[op_token]
                   : Int32.MaxValue;
        }

        /// <summary>
        /// Determine if a token is an operator.
        /// </summary>
        private static bool IsOp(string token)
        {
            return BIN_OP_TOKEN_TO_PREC.ContainsKey(token)
                || UN_OP_TOKEN_TO_PREC.ContainsKey(token);
        }

        /// <summary>
        /// An enum representing the different possible arities of operators.
        /// </summary>
        private enum Arity
        {
            Unknown,
            Nullary,
            Unary,
            Binary
        }

        /// <summary>
        /// An enum representing the different possible associativities of operators;
        /// left, right, or none (i.e. non-associative).
        /// </summary>
        private enum Associativity
        {
            Left,
            Right,
            None
        }

        private static IInstruction GetOpInstr(string op_token)
        {
            if (IsUnary(op_token))
                return OP_INSTR_ORDERED_BY_PRECEDENCE[UN_OP_TOKEN_TO_PREC[op_token]];
            else if (IsBinary(op_token))
                return OP_INSTR_ORDERED_BY_PRECEDENCE[BIN_OP_TOKEN_TO_PREC[op_token]];
            return null;
        }

        /// <summary>
        /// Return the arity of an operator by it's token.
        /// </summary>
        private static Arity GetOpArity(string op_token)
        {
            if (AMBIGUOUS_OPERATORS.Contains(op_token))
                return Arity.Unknown;
            else if (BIN_OP_TOKEN_TO_PREC.ContainsKey(op_token))
                return Arity.Binary;
            return Arity.Unary;
        }

        /// <summary>
        /// Determine if an operator token is unary or not.
        /// </summary>
        private static bool IsUnary(string op_token)
        {
            return UN_OP_TOKEN_TO_PREC.ContainsKey(op_token);
        }

        /// <summary>
        /// Determine if an operator token is binary or not.
        /// </summary>
        private static bool IsBinary(string op_token)
        {
            return BIN_OP_TOKEN_TO_PREC.ContainsKey(op_token);
        }

        /// <summary>
        /// Return the associativity of an operator by it's token.
        /// </summary>
        private static Associativity GetOpAssociativity(string op_token)
        {
            var index = Prec(op_token);
            if (LEFT_ASSOCIATIVE_OP_INDICES.Contains(index))
                return Associativity.Left;
            else if (RIGHT_ASSOCIATIVE_OP_INDICES.Contains(index))
                return Associativity.Right;
            return Associativity.None;
        }

        /// <summary>
        /// Convert a token to it's appropriate "internal" token.
        /// </summary>
        private static string ConvertToInternalToken(string op_token, Arity arity)
        {
            if (arity == Arity.Binary)
                return op_token; // There are no internal binary tokens.
            if (OP_TOKEN_TO_INTERNAL_TOKEN.ContainsKey(op_token))
                return OP_TOKEN_TO_INTERNAL_TOKEN[op_token];
            return op_token;
        }

        /// <summary>
        /// Determine if a token is a valid left-bracket type token (i.e. "(").
        /// </summary>
        private static bool IsLeftBracket(string token)
        {
            return LEFT_TO_RIGHT_BRACKETS.ContainsKey(token);
        }

        /// <summary>
        /// Determine if a token is a valid right-bracket type token (i.e. ")").
        /// </summary>
        private static bool IsRightBracket(string token)
        {
            return RIGHT_TO_LEFT_BRACKETS.ContainsKey(token);
        }

        /// <summary>
        /// Determine the parsing state type to enter when entering brackets (or null).
        /// For example, when entering a vector (whose bracket tokens are << and >>), the
        /// parsing state type gets changed to ParsingStateType.Vector.
        /// </summary>
        private static ParsingStateType? GetBracketParsingState(string token)
        {
            return LEFT_BRACKET_TO_PARSING_STATE.ContainsKey(token)
                   ? LEFT_BRACKET_TO_PARSING_STATE[token]
                   : null;
        }

        /// <summary>
        /// Throw an error if a bracket type requires a certain number of elements to have
        /// been encountered before reaching the closing bracket. For example, in a vector,
        /// 2 elements must be encountered before reaching the closing bracket (<< 1, 2 >>
        /// is valid, but << 1, 2, 3>> is not).
        /// 
        /// The error thrown must take a the line number of the opening bracket as its only
        /// argument.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="count"></param>
        /// <param name="lineNumber"></param>
        private static void ThrowIfBracketRequiresSpecificElemCount(string token, int count, int lineNumber)
        {
            if (BRACKETS_THAT_REQUIRE_SPECIFIC_ELEM_COUNT.ContainsKey(token))
            {
                var v = BRACKETS_THAT_REQUIRE_SPECIFIC_ELEM_COUNT[token];
                if (v.count != count)
                    throw v.exception(lineNumber);
            }
        }

        /// <summary>
        /// Check if a bracket type has a finalization instruction. For example, when a
        /// vector closing bracket >> has been encountered, the instruction BUILD_VEC must
        /// be added.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool BracketHasFinalizationInstruction(string token)
        {
            return RIGHT_BRACKET_FINALIZATION_INSTRS.ContainsKey(token);
        }

        /// <summary>
        /// Return the finalization instruction associated with a bracket.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="elementCount"></param>
        /// <returns></returns>
        private static IInstruction GetBracketFinalizationInstruction(string token, int elementCount)
        {
            return RIGHT_BRACKET_FINALIZATION_INSTRS[token](elementCount);
        }

        /// <summary>
        /// The shunting-yard algorithm requires that when an operator '~' has been encountered,
        /// some of the operators in the operatorStack must be popped off first and added to
        /// the instruction list. Depending on the associativity of '~', the operators popped
        /// must satisfy different predicates. This dictionary maps associativity types to
        /// appropriate predicates for the operators to be popped off the operatorStack.
        /// </summary>
        private static Dictionary<Associativity, Func<string, string, bool>> POP_OPS_TERMINAL_PREDS_FROM_ASSOC
            = new Dictionary<Associativity,Func<string,string,bool>>()
        {
            {Associativity.Left, (op, top)  => Prec(top) >= Prec(op)},
            {Associativity.Right, (op, top) => Prec(top) > Prec(op)},
            {Associativity.None, (op, top)  => Prec(top) >= Prec(op)}
        };

        /// <summary>
        /// Return the appropriate operator predicate to use when popping operators off
        /// the operatorStack, according to the associativity of the encountered operator.
        /// For more info, see POP_OPS_TERMINAL_PREDS_FROM_ASSOC.
        /// </summary>
        private static Func<string, string, bool> GetPopOpsTerminalPredicateFromAssoc(Associativity assoc)
        {
            return POP_OPS_TERMINAL_PREDS_FROM_ASSOC.ContainsKey(assoc)
                   ? POP_OPS_TERMINAL_PREDS_FROM_ASSOC[assoc]
                   : null;
        }

        // ======================================

        /// <summary>
        /// The current line number.
        /// </summary>
        private int lineNumber = 0;

        /// <summary>
        /// The stack of tokens to parse.
        /// </summary>
        private Stack<string> tokenStack;

        /// <summary>
        /// The stack of operators (part of the shunting-yard algorithm).
        /// </summary>
        private Stack<string> operatorStack = new Stack<string>();

        /// <summary>
        /// The instructions created by parsing the tokens in tokenStack.
        /// </summary>
        private List<IInstruction> instructions = new List<IInstruction>();

        /// <summary>
        /// A stack of parsing states. A parsing state is a way of letting the shunting-yard
        /// parser do different things depending on the current parsing scenario. The reason
        /// the parsing states are contained in a stack is so that parsing states can be nested.
        /// 
        /// For example, in the program [1, [[ "1" : << 1, 1 >> ]] ], at the second "," token,
        /// the parsing states stack would contain Vector, Dictionary, Array in order from top
        /// to bottom of the stack (since we are inside a Vector, which is inside a Dictionary,
        /// which is inside an Array).
        /// </summary>
        private Stack<ParsingState> parsingStates
            = new Stack<ParsingState>(
                new ParsingState[] 
                { 
                    new ParsingState(ParsingStateType.Default) 
                });

        /// <summary>
        /// The current parsing state (the top of the parsingStates stack).
        /// </summary>
        private ParsingState parsingState { get { return parsingStates.Peek(); } }

        /// <summary>
        /// A stack of brackets encountered so far and their associated line numbers.
        /// </summary>
        private Stack<TokenWithLineNo> bracketStack = new Stack<TokenWithLineNo>();

        private Stack<ExpectantTokenType> expectant = new Stack<ExpectantTokenType>();

        public ShuntingYardParser(List<string> tokens)
        {
            tokens.Reverse();
            tokenStack = new Stack<string>(tokens);
        }

        public void Parse()
        {
            string token, prevToken = null;

            while (tokenStack.Count != 0)
            {
                token = tokenStack.Pop();
                if (token == "\n")
                {
                    ParseNewline(token, prevToken);
                    continue;
                }
                else if (token == Tokens.SINGLE_LINE_COMMENT)
                {
                    ParseSingleLineComment(token, prevToken);
                }
                else if (token == Tokens.INDEF_COMMENT_LEFT)
                {
                    ParseMultilineCommentStart(token, prevToken);
                }
                else if (token == Tokens.INDEF_COMMENT_RIGHT)
                {
                    ParseMultilineCommentEnd(token, prevToken);
                    continue;
                }
                
                if (parsingState.Type == ParsingStateType.SingleLineComment ||
                         parsingState.Type == ParsingStateType.MultiLineComment)
                    continue;

                // The reason this is here is because if ANY token that isn't a comment
                // is encountered directly after parsing a left-bracket token (which will
                // create a parsing state with an "elementCount" attribute) indicates that
                // there will be at least one element in whatever bracketed thing we are
                // parsing.
                if (parsingState.Attributes.ContainsKey("elementCount") &&
                    (int)parsingState.Attributes["elementCount"] == 0)
                    parsingState.Attributes["elementCount"] = 1;

                if (Regex.IsMatch(token, Tokens.WORD))
                {
                    ParseWord(token, prevToken);
                }
                else if (Regex.IsMatch(token, Tokens.NUMBER))
                {
                    ParseNumber(token, prevToken);
                }
                else if (token[0] == '"' && token[token.Length - 1] == '"')
                {
                    ParseString(token, prevToken);
                }
                else if (IsOp(token))
                {
                    ParseOp(token, prevToken);
                }
                else if (IsLeftBracket(token))
                {
                    ParseLeftBracket(token, prevToken);
                }
                else if (IsRightBracket(token))
                {
                    ParseRightBracket(token, prevToken);
                }
                else if (token == Tokens.ELEM_SEP)
                {
                    ParseElemSep(token, prevToken);
                }
                else if (token == Tokens.KEY_VAR_CONN)
                {
                    ParseKVPConn(token, prevToken);
                }
                else
                {
                    // Change this later.
                    throw new Exception();
                }
                prevToken = token;
            }

            PostParse();
        }

        private void ParseNewline(string token, string prevToken)
        {
            lineNumber++;
            if (parsingState.Type == ParsingStateType.SingleLineComment)
                parsingStates.Pop();
        }

        private void ParseSingleLineComment(string token, string prevToken)
        {
            parsingStates.Push(new ParsingState(ParsingStateType.SingleLineComment));
        }

        private void ParseMultilineCommentStart(string token, string prevToken)
        {
            if (parsingState.Type != ParsingStateType.SingleLineComment &&
                        parsingState.Type != ParsingStateType.MultiLineComment)
            {
                parsingStates.Push(new ParsingState(ParsingStateType.MultiLineComment));
                parsingState.Attributes["commentStart"] = lineNumber;
            }
        }

        private void ParseMultilineCommentEnd(string token, string prevToken)
        {
            if (parsingState.Type != ParsingStateType.SingleLineComment &&
                parsingState.Type != ParsingStateType.MultiLineComment)
                throw NeonExceptions.MismatchedClosingMultilineCommentDelimiter(lineNumber);

            if (parsingState.Type != ParsingStateType.SingleLineComment)
                parsingStates.Pop();
        }

        private void ParseWord(string token, string prevToken)
        {
            // Change this later.
            instructions.Add(new LDL(token));
        }

        private void ParseNumber(string token, string prevToken)
        {
            // Change this later.
            instructions.Add(new LDC(new NeonObject()));
        }

        private void ParseString(string token, string prevToken)
        {
            // Change this later.
            instructions.Add(new LDC(new NeonObject()));
        }

        private void ParseOp(string token, string prevToken)
        {
            var arity = GetOpArity(token);
            if (arity == Arity.Unknown)
            {
                arity = (prevToken == null || IsOp(prevToken) || IsLeftBracket(prevToken))
                        ? Arity.Unary
                        : Arity.Binary;
            }
            else if (arity == Arity.Unary)
            {
                if (!(prevToken == null || IsOp(prevToken)))
                    throw NeonExceptions.UnexpectedOperatorEncountered(token, lineNumber);
            }
            else if (arity == Arity.Binary)
            {
                if (prevToken == null || IsOp(prevToken))
                    throw NeonExceptions.BinaryOperatorUsedAsUnary(token, lineNumber);
            }

            string op = ConvertToInternalToken(token, arity);
            if (operatorStack.Count > 0)
            {

                var assoc = GetOpAssociativity(op);
                var non_assoc = assoc == Associativity.None;
                var terminalPred = GetPopOpsTerminalPredicateFromAssoc(assoc);

                var top = operatorStack.Peek();
                while (IsOp(top) && terminalPred(op, top))
                {
                    instructions.Add(GetOpInstr(operatorStack.Pop()));
                    if (operatorStack.Count == 0)
                        break;
                    top = operatorStack.Peek();
                    if (non_assoc && op == top)
                        // Change this later
                        throw new Exception();
                }
            }
            operatorStack.Push(op);
        }

        private void ParseLeftBracket(string token, string prevToken)
        {
            operatorStack.Push(BRACKET_TERMINAL_TOKEN);

            var parsingStateType = GetBracketParsingState(token);

            if (parsingStateType.HasValue)
            {
                var pst_val = parsingStateType.Value;
                var newParsingState = new ParsingState(pst_val);
                newParsingState.Attributes["elementCount"] = 0;

                if (pst_val == ParsingStateType.Dictionary)
                    newParsingState.Attributes["encounteredKVPConnector"] = false;

                parsingStates.Push(newParsingState);
            }

            bracketStack.Push(new TokenWithLineNo() { Token = token, LineNumber = lineNumber });
        }

        private void ParseRightBracket(string token, string prevToken)
        {
            var l_brac = RIGHT_TO_LEFT_BRACKETS[token];
            if (bracketStack.Count == 0)
                throw RIGHT_BRACKET_MISMATCH_EXCEPTIONS[token](lineNumber);

            var last = bracketStack.Pop();
            if (last.Token != l_brac)
                throw RIGHT_BRACKET_MISMATCH_EXCEPTIONS[token](last.LineNumber);

            var top = operatorStack.Peek();
            while (top != BRACKET_TERMINAL_TOKEN)
            {
                instructions.Add(GetOpInstr(operatorStack.Pop()));
                top = operatorStack.Peek();
            }
            operatorStack.Pop();

            if (parsingState.Attributes.ContainsKey("elementCount"))
            {
                var elementCount = (int)parsingState.Attributes["elementCount"];
                ThrowIfBracketRequiresSpecificElemCount(l_brac, elementCount, lineNumber);

                if (BracketHasFinalizationInstruction(token))
                    instructions.Add(GetBracketFinalizationInstruction(token, elementCount));
            }

            // If the token doesn't have an associated parsing state (i.e. ")"), then
            // the top of the parsingStates stack doesn't change and we don't have to
            if (GetBracketParsingState(l_brac).HasValue)
                parsingStates.Pop();
        }

        private void ParseElemSep(string token, string prevToken)
        {
            string top;
            switch (parsingState.Type)
            {
                case ParsingStateType.Default: 
                    break;
                case ParsingStateType.Array:
                case ParsingStateType.Vector:
                case ParsingStateType.RelativeVector:
                    if (prevToken == PARSING_STATE_TO_LEFT_BRACKET[parsingState.Type])
                        // Change this later
                        throw new Exception();
                    else if (prevToken == Tokens.ELEM_SEP)
                        // Change this later
                        throw new Exception();

                    parsingState.Attributes["elementCount"]
                        = (int)parsingState.Attributes["elementCount"] + 1; // no easy ++ :(

                    top = operatorStack.Peek();
                    while (top != BRACKET_TERMINAL_TOKEN)
                    {
                        instructions.Add(GetOpInstr(operatorStack.Pop()));
                        top = operatorStack.Peek();
                    }
                    // We leave the BRACKET_TERMINAL_TOKEN on the operatorStack instead of popping it 
                    // because the next time we run into an ELEM_SEP or a right bracket the operators 
                    // will only be popped up until the BRACKET_TERMINAL_TOKEN anyways.
                    break;
                case ParsingStateType.Dictionary:
                    if (prevToken == Tokens.DICT_LEFT)
                        // Change this later
                        throw new Exception();
                    else if (prevToken == Tokens.ELEM_SEP)
                        // Change this later
                        throw new Exception();
                    else if (prevToken == Tokens.KEY_VAR_CONN)
                        // Change this later
                        throw new Exception();

                    parsingState.Attributes["elementCount"]
                        = (int)parsingState.Attributes["elementCount"] + 1;

                    top = operatorStack.Peek();
                    while (top != Tokens.KEY_VAR_CONN)
                    {
                        instructions.Add(GetOpInstr(operatorStack.Pop()));
                        top = operatorStack.Peek();
                    }
                    operatorStack.Pop();
                    operatorStack.Push(BRACKET_TERMINAL_TOKEN);
                    break;
            }
        }

        private void ParseKVPConn(string token, string prevToken)
        {

        }

        private void PostParse()
        {
            if (parsingState.Type == ParsingStateType.MultiLineComment)
                throw NeonExceptions.UnclosedOpeningMultilineCommentDelimiter(
                    (int)parsingState.Attributes["commentStart"]);

            if (bracketStack.Count != 0)
            {
                var top = bracketStack.Pop();
                throw RIGHT_BRACKET_MISMATCH_EXCEPTIONS[LEFT_TO_RIGHT_BRACKETS[top.Token]](top.LineNumber);
            }

            while (operatorStack.Count > 0)
            {
                instructions.Add(GetOpInstr(operatorStack.Peek()));
                operatorStack.Pop();
            }
        }

        public List<IInstruction> GetInstructions()
        {
            return instructions;
        }
    }
}
