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

        // --= STATIC OPERATOR-RELATED FIELDS =--
        // ======================================

        /// <summary>
        /// A list of all operator instructions, ordered by precedence (least to greatest).
        /// </summary>
        private static IInstruction[] OP_INSTR_ORDERED_BY_PRECEDENCE
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
            BUILD_RANGE.Instance,
            BIN_MOD.Instance,
            BIN_ADD.Instance,
            BIN_SUB.Instance,
            BIN_MUL.Instance,
            BIN_DIV.Instance,
            BIN_POW.Instance,
            UN_NOT.Instance,
            UN_NEG.Instance,
            UN_POS.Instance
        };

        /// <summary>
        /// A dictionary mapping binary operator tokens to their precedence, which is also their
        /// index in OP_INSTR_ORDERED_BY_PRECEDENCE.
        /// </summary>
        private static Dictionary<string, int> BIN_OP_TOKEN_TO_PREC
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
            {Tokens.DISCRETE_RANGE_GEN, 8},
            {Tokens.BIN_MOD, 9},
            {Tokens.BIN_ADD, 10},
            {Tokens.BIN_SUB, 11},
            {Tokens.BIN_MUL, 12},
            {Tokens.BIN_DIV, 13},
            {Tokens.BIN_POW, 14}
        };

        /// <summary>
        /// A dictionary mapping unary operator tokens to their precedence, which is also their
        /// index in OP_INSTR_ORDERED_BY_PRECEDENCE.
        /// </summary>
        private static Dictionary<string, int> UN_OP_TOKEN_TO_PREC
            = new Dictionary<string, int>()
        {
            {Tokens.UN_NOT, 15},
            {Tokens.UN_NEG, 16},
            {Tokens.UN_POS, 17},
            {Tokens.INTERNAL_UN_NEG, 16},
            {Tokens.INTERNAL_UN_POS, 17}
        };

        /// <summary>
        /// A list of the indices of operators in OP_INSTR_ORDERED_BY_PRECEDENCE that are left associative.
        /// 
        /// An operator '~' is left associative if a ~ b ~ c == (a ~ b) ~ c.
        /// </summary>
        private static int[] LEFT_ASSOCIATIVE_OP_INDICES
            = new int[]
        {
            0, 1, 2, 3, 9, 10, 11, 12, 13
        };

        /// <summary>
        /// A list of the indices of operators in OP_INSTR_ORDERED_BY_PRECEDENCE that are right associative.
        /// 
        /// An operator '~' is right associative if a ~ b ~ c == a ~ (b ~ c).
        /// </summary>
        private static int[] RIGHT_ASSOCIATIVE_OP_INDICES
            = new int[]
        {
            8, 14, 15, 16, 17
        };

        /// <summary>
        /// A list of the indices of operators in OP_INSTR_ORDERED_BY_PRECEDENCE that are non-associative.
        /// 
        /// An operator '~' is non-associative if the expression a ~ b ~ c is undefined.
        /// </summary>
        private static int[] NON_ASSOCIATIVE_OP_INDICES
            = new int[]
        {
            4, 5, 6, 7
        };

        /// <summary>
        /// A mapping of operator tokens to their "internal" form. For more information on internal tokens,
        /// see the Tokens static class.
        /// </summary>
        private static Dictionary<string, string> OP_TOKEN_TO_INTERNAL_TOKEN =
            new Dictionary<string, string>()
        {
            {Tokens.UN_POS, Tokens.INTERNAL_UN_POS},
            {Tokens.UN_NEG, Tokens.INTERNAL_UN_NEG}
        };

        /// <summary>
        /// A mapping of left bracket tokens to their associated right bracket token.
        /// </summary>
        private static Dictionary<string, string> LEFT_TO_RIGHT_BRACKETS =
            new Dictionary<string, string>()
        {
            {Tokens.EXPR_START,  Tokens.EXPR_END},
            {Tokens.ARRAY_START, Tokens.ARRAY_END},
            {Tokens.DICT_START,  Tokens.DICT_END},
            {Tokens.BLOCK_START, Tokens.BLOCK_END},
            {Tokens.VEC_START,   Tokens.VEC_END},
            {Tokens.RVEC_START,  Tokens.RVEC_END}
        };

        /// <summary>
        /// A mapping of right bracket tokens to their associated left bracket token.
        /// </summary>
        private static Dictionary<string, string> RIGHT_TO_LEFT_BRACKETS =
            new Dictionary<string, string>()
        {
            {Tokens.EXPR_END,  Tokens.EXPR_START},
            {Tokens.ARRAY_END, Tokens.ARRAY_START},
            {Tokens.DICT_END,  Tokens.DICT_START},
            {Tokens.BLOCK_END, Tokens.BLOCK_START},
            {Tokens.VEC_END,   Tokens.VEC_START},
            {Tokens.RVEC_END,  Tokens.RVEC_START}
        };

        private static Dictionary<string, ParsingStateType> LEFT_BRACKET_TO_PARSING_STATE =
            new Dictionary<string, ParsingStateType>()
        {
            {Tokens.EXPR_START,  ParsingStateType.Default},
            {Tokens.ARRAY_START, ParsingStateType.Array},
            {Tokens.DICT_START,  ParsingStateType.Dictionary},
            {Tokens.BLOCK_START, ParsingStateType.Default},
            {Tokens.VEC_START,   ParsingStateType.Vector},
            {Tokens.RVEC_START,  ParsingStateType.RelativeVector}
        };

        private struct INT_FUNC_PAIR
        {
            public int count { get; set; }
            public Func<int, NeonSyntaxException> exception { get; set; }
        }

        private static Dictionary<string, INT_FUNC_PAIR> BRACKETS_THAT_REQUIRE_SPECIFIC_ELEM_COUNT =
            new Dictionary<string, INT_FUNC_PAIR>()
        {
            // Change this later
            {Tokens.VEC_START,  new INT_FUNC_PAIR() {count=2, exception=NeonExceptions.Exception0014}},
            {Tokens.RVEC_START, new INT_FUNC_PAIR() {count=2, exception=NeonExceptions.Exception0014}}
        };

        private static Dictionary<string, Func<int, IInstruction>> RIGHT_BRACKET_FINALIZATION_INSTRS =
            new Dictionary<string, Func<int, IInstruction>>()
        {
            {Tokens.ARRAY_START, (i) => new BUILD_ARRAY(i)},
            {Tokens.DICT_START,  (i) => new BUILD_DICT(i)},
            {Tokens.VEC_START,   (i) => BUILD_VEC.Instance},
            {Tokens.RVEC_START,  (i) => BUILD_RVEC.Instance}
        };

        /// <summary>
        /// Return the precedence of a token.
        /// </summary>
        private static int Prec(string op_token)
        {
            if (BIN_OP_TOKEN_TO_PREC.ContainsKey(op_token))
                return BIN_OP_TOKEN_TO_PREC[op_token];
            return UN_OP_TOKEN_TO_PREC[op_token];
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
        private static enum Arity
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
        private static enum Associativity
        {
            Left,
            Right,
            None
        }

        private static IInstruction GetOpInstr(string op_token)
        {
            if (IsUnary(op_token))
                return OP_INSTR_ORDERED_BY_PRECEDENCE[BIN_OP_TOKEN_TO_PREC[op_token]];
            return OP_INSTR_ORDERED_BY_PRECEDENCE[UN_OP_TOKEN_TO_PREC[op_token]];
        }

        /// <summary>
        /// Return the arity of an operator by it's token.
        /// </summary>
        private static Arity GetOpArity(string op_token)
        {
            bool un = UN_OP_TOKEN_TO_PREC.ContainsKey(op_token),
                 bin = BIN_OP_TOKEN_TO_PREC.ContainsKey(op_token);
            return (un && bin) ? Arity.Unknown : un ? Arity.Unary : Arity.Binary;
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

        private static bool IsLeftBracket(string token)
        {
            return LEFT_TO_RIGHT_BRACKETS.ContainsKey(token);
        }

        private static bool IsRightBracket(string token)
        {
            return RIGHT_TO_LEFT_BRACKETS.ContainsKey(token);
        }

        private static ParsingStateType GetBracketParsingState(string token)
        {
            return LEFT_BRACKET_TO_PARSING_STATE[token];
        }

        private static void ThrowIfBracketRequiresSpecificElemCount(string token, int count, int lineNumber)
        {
            if (BRACKETS_THAT_REQUIRE_SPECIFIC_ELEM_COUNT.ContainsKey(token))
            {
                var v = BRACKETS_THAT_REQUIRE_SPECIFIC_ELEM_COUNT[token];
                if (v.count != count)
                    throw v.exception(lineNumber);
            }
        }

        private static bool BracketHasFinalizationInstruction(string token)
        {
            return RIGHT_BRACKET_FINALIZATION_INSTRS.ContainsKey(token);
        }

        private static IInstruction GetBracketFinalizationInstruction(string token, int elementCount)
        {
            return RIGHT_BRACKET_FINALIZATION_INSTRS[token](elementCount);
        }

        private static Dictionary<Associativity, Func<string, string, bool>> POP_OPS_TERMINAL_PREDS_FROM_ASSOC
            = new Dictionary<Associativity,Func<string,string,bool>>()
        {
            {Associativity.Left, (op, top)  => Prec(top) >= Prec(op)},
            {Associativity.Right, (op, top) => Prec(top) > Prec(op)},
            {Associativity.None, (op, top)  => Prec(top) > Prec(op)}
        };

        private static Func<string, string, bool> GetPopOpsTerminalPredicateFromAssoc(Associativity assoc)
        {
            return POP_OPS_TERMINAL_PREDS_FROM_ASSOC[assoc];
        }

        // ======================================

        /// <summary>
        /// The current line number.
        /// </summary>
        private int lineNumber = 0;

        private Stack<string> tokenStack;

        private Stack<string> operatorStack = new Stack<string>();

        private List<IInstruction> instructions = new List<IInstruction>();

        private Stack<ParsingState> parsingStates
            = new Stack<ParsingState>(
                new ParsingState[] 
                { 
                    new ParsingState(ParsingStateType.Default) 
                });

        private ParsingState parsingState { get { return parsingStates.Peek(); } }

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
                    lineNumber++;
                    if (parsingState.Type == ParsingStateType.SingleLineComment)
                        parsingStates.Pop();
                    continue;
                }
                else if (token == Tokens.SINGLE_LINE_COMMENT)
                {
                    parsingStates.Push(new ParsingState(ParsingStateType.SingleLineComment));
                }
                else if (token == Tokens.INDEF_COMMENT_START)
                {
                    if (parsingState.Type != ParsingStateType.SingleLineComment &&
                        parsingState.Type != ParsingStateType.MultiLineComment)
                    {
                        parsingStates.Push(new ParsingState(ParsingStateType.MultiLineComment));
                        parsingState.Attributes["commentStart"] = lineNumber;
                    }
                }
                else if (token == Tokens.INDEF_COMMENT_END)
                {
                    if (parsingState.Type != ParsingStateType.SingleLineComment &&
                        parsingState.Type != ParsingStateType.MultiLineComment)
                        throw NeonExceptions.Exception0007(lineNumber);
                    else if (parsingState.Type == ParsingStateType.SingleLineComment)
                        continue;
                    parsingStates.Pop();
                }
                else if (parsingState.Type != ParsingStateType.SingleLineComment &&
                         parsingState.Type != ParsingStateType.MultiLineComment)
                {
                    if (Regex.IsMatch(token, Tokens.WORD))
                    {
                        // Change this later.
                        instructions.Add(new LDL(token));
                    }
                    else if (Regex.IsMatch(token, Tokens.NUMBER))
                    {
                        // Change this later.
                        instructions.Add(new LDC(new NeonObject()));
                    }
                    else if (token[0] == '"' && token[token.Length - 1] == '"')
                    {
                        // Change this later.
                        instructions.Add(new LDC(new NeonObject()));
                    }
                    else if (IsOp(token))
                    {
                        var arity = GetOpArity(token);
                        if (arity == Arity.Unknown)
                        {
                            arity = (prevToken == null || IsOp(prevToken)) ? Arity.Unary : Arity.Binary;
                        }
                        else if (arity == Arity.Unary)
                        {
                            if (!(prevToken == null || IsOp(prevToken)))
                                throw NeonExceptions.Exception0004(token, lineNumber);
                        }

                        if (operatorStack.Count > 0)
                        {
                            string op = ConvertToInternalToken(token, arity);

                            var assoc = GetOpAssociativity(op);
                            var non_assoc = assoc == Associativity.None;
                            var terminalPred = GetPopOpsTerminalPredicateFromAssoc(assoc);

                            var top = operatorStack.Peek();
                            while (terminalPred(op, top))
                            {
                                instructions.Add(GetOpInstr(operatorStack.Pop()));
                                if (operatorStack.Count == 0)
                                    break;
                                top = operatorStack.Peek();
                                if (non_assoc && op == top)
                                    // Change this later
                                    throw new Exception();
                            }
                            operatorStack.Push(op);
                        }
                    }
                    else if (IsLeftBracket(token))
                    {
                        operatorStack.Push(token);

                        var parsingStateType = GetBracketParsingState(token);
                        var newParsingState = new ParsingState(parsingStateType);
                        newParsingState.Attributes["elementCount"] = 0;
                        newParsingState.Attributes["encounteredElemSep"] = false;

                        parsingStates.Push(newParsingState);
                    }
                    else if (IsRightBracket(token))
                    {
                        var elementCount = (int)parsingState.Attributes["elementCount"];

                        string terminal = 
                            !(bool)parsingState.Attributes["encounteredElemSep"]
                            ? RIGHT_TO_LEFT_BRACKETS[token]
                            : Tokens.ELEM_SEP;

                        ThrowIfBracketRequiresSpecificElemCount(token, elementCount, lineNumber);

                        var top = operatorStack.Peek();
                        while (top != terminal)
                        {
                            instructions.Add(GetOpInstr(operatorStack.Pop()));
                            top = operatorStack.Peek();
                        }
                        operatorStack.Pop();
                        parsingStates.Pop();

                        if (BracketHasFinalizationInstruction(token))
                            instructions.Add(GetBracketFinalizationInstruction(token, elementCount));
                    }
                    else
                    {
                        // Change this later.
                        throw new Exception();
                    }
                    prevToken = token;
                }
            }
        }
    }
}
