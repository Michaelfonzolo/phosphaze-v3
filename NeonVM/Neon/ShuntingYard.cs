using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NeonVM.Neon.Instructions;

namespace NeonVM.Neon
{
    public class ShuntingYard
    {

        private class DUMMY_INSTR { }

        // A dummy instruction representing the start of an expression.
        private class EXPR_START_INSTR : DUMMY_INSTR, IInstruction 
        { 
            public static EXPR_START_INSTR Instance = new EXPR_START_INSTR(); 
        }

        // A dummy instruction representing the start of an array.
        private class ARRAY_START_INSTR : DUMMY_INSTR, IInstruction
        {
            public static ARRAY_START_INSTR Instance = new ARRAY_START_INSTR();
        }

        // A dummy instruction representing the start of a vector.
        private class VEC_START_INSTR : DUMMY_INSTR, IInstruction 
        { 
            public static VEC_START_INSTR Instance = new VEC_START_INSTR(); 
        }

        // A dummy instruction representing the start of a relative vector.
        private class RVEC_START_INSTR : DUMMY_INSTR, IInstruction
        {
            public static RVEC_START_INSTR Instance = new RVEC_START_INSTR();
        }

        private class ELEM_SEP_INSTR : DUMMY_INSTR, IInstruction
        {
            public static ELEM_SEP_INSTR Instance = new ELEM_SEP_INSTR();
        }

        private static IInstruction[] OPERATORS 
            = new IInstruction[]
        {
            BIN_OR.Instance,
            BIN_AND.Instance,
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

        private static Dictionary<string, int> BINARY_OPERATOR_TOKENS 
            = new Dictionary<string, int>()
        {
            {Tokens.BIN_OR, 0},
            {Tokens.BIN_AND, 1},
            {Tokens.BIN_ADD, 3},
            {Tokens.BIN_SUB, 4},
            {Tokens.BIN_MUL, 5},
            {Tokens.BIN_DIV, 6},
            {Tokens.BIN_POW, 7},
            {Tokens.BIN_MOD, 2}
        };

        private static Dictionary<string, int> UNARY_OPERATOR_TOKENS
            = new Dictionary<string, int>()
        {
            {Tokens.UN_NOT, 8},
            {Tokens.UN_NEG, 9},
            {Tokens.UN_POS, 10}
        };

        private static int[] LEFT_ASSOCIATIVE
            = new int[]
        {
            0, 1, 2, 3, 4, 5, 6
        };

        private static int[] RIGHT_ASSOCIATIVE
            = new int[]
        {
            7, 8, 9, 10
        };

        private static string[] BRACKET_TOKENS
            = new string[] 
        {
            Tokens.EXPR_START,
            Tokens.EXPR_END,
            Tokens.ARRAY_START,
            Tokens.ARRAY_END,
            Tokens.DICT_START,
            Tokens.DICT_END,
            Tokens.BLOCK_START,
            Tokens.BLOCK_END,
            Tokens.VEC_START,
            Tokens.VEC_END,
            Tokens.RVEC_START,
            Tokens.RVEC_END
        };

        private static Dictionary<string, Func<int, NeonSyntaxException>> mismatchedOpenBracketExceptions
            = new Dictionary<string, Func<int, NeonSyntaxException>>()
        {
            {Tokens.EXPR_START, NeonExceptions.Exception0006},
            {Tokens.VEC_START,  NeonExceptions.Exception0013},
            {Tokens.RVEC_START, NeonExceptions.Exception0014}
        };

        private static bool IsOp(string i)
        {
            return BINARY_OPERATOR_TOKENS.ContainsKey(i)
                || UNARY_OPERATOR_TOKENS.ContainsKey(i);
        }

        private static bool IsBinary(string i)
        {
            return BINARY_OPERATOR_TOKENS.ContainsKey(i);
        }

        private static bool IsUnary(string i)
        {
            return UNARY_OPERATOR_TOKENS.ContainsKey(i);
        }

        private static int Associativity(IInstruction op)
        {
            // 1 - Left Associative
            // -1 - Right Associative
            return LEFT_ASSOCIATIVE.Contains(Array.IndexOf<IInstruction>(OPERATORS, op)) ? 1 : -1;
        }

        private static int Prec(IInstruction op)
        {
            return Array.IndexOf<IInstruction>(OPERATORS, op);
        }

        private Stack<string> tokenStack;

        private Stack<IInstruction> operatorStack = new Stack<IInstruction>();

        private List<IInstruction> instructions = new List<IInstruction>();

        private int lineNumber = 0;

        private Dictionary<string, Stack<int>> bracketLineStarts
            = new Dictionary<string, Stack<int>>()
        {
            {Tokens.EXPR_START,  new Stack<int>()},
            {Tokens.ARRAY_START, new Stack<int>()},
            {Tokens.DICT_START,  new Stack<int>()},
            {Tokens.BLOCK_START, new Stack<int>()},
            {Tokens.VEC_START,   new Stack<int>()},
            {Tokens.RVEC_START,  new Stack<int>()}
        };

        private Stack<ParsingState> parsingStates = new Stack<ParsingState>();

        private ParsingState parsingState { get { return parsingStates.Peek(); } }

        private Dictionary<string, Action> tokenActions;

        public ShuntingYard(List<string> tokens)
        {
            tokens.Reverse();
            tokenStack = new Stack<string>(tokens);
            parsingStates.Push(new ParsingState(ParsingStateType.Default));

            tokenActions = new Dictionary<string, Action>()
            {
                {Tokens.EXPR_START, _Parse_EXPR_START},
                {Tokens.EXPR_END,   _Parse_EXPR_END},
                {Tokens.VEC_START,  _Parse_VEC_START},
                {Tokens.VEC_END,    _Parse_VEC_END},
                {Tokens.RVEC_START, _Parse_RVEC_START},
                {Tokens.RVEC_END,   _Parse_RVEC_END},
                {Tokens.ELEM_SEP,   _Parse_ELEM_SEP}
            };
        }

        private void PopOpsUntil(IInstruction terminal, Stack<int> lineStarts, NeonSyntaxException exception)
        {
            if (operatorStack.Count == 0 || lineStarts.Count == 0)
                throw exception;
            var top = operatorStack.Peek();
            while (top != terminal)
            {
                instructions.Add(operatorStack.Pop());
                top = operatorStack.Peek();
                // No need to check if the opStack is empty because we are guaranteed
                // to encounter a terminal character if lineStarts.Count > 0.
            }
            operatorStack.Pop(); // Get rid of the dummy operator (i.e. the left bracket)
            lineStarts.Pop();
        }

        private void BeginVec(ParsingStateType stateType, IInstruction terminal, Stack<int> lineStarts)
        {
            operatorStack.Push(terminal);
            lineStarts.Push(lineNumber);

            var vec_state = new ParsingState(stateType);
            vec_state.Attributes["componentCount"] = (int)1;
            parsingStates.Push(vec_state);
        }

        private void EndVec(
            IInstruction terminal, Stack<int> lineStart,
            NeonSyntaxException exc1, NeonSyntaxException exc2,
            IInstruction build_instr)
        {
            PopOpsUntil(terminal, lineStart, exc1);

            if ((int)parsingState.Attributes["componentCount"] != 2)
                throw exc2;

            instructions.Add(build_instr);
            parsingStates.Pop();
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
                        bool unary = IsUnary(token), binary = IsBinary(token);
                        if (unary && binary)
                        {
                            // Occurs when a token is shared for a unary and a binary operator (i.e. "+").
                            if (prevToken == null || IsOp(prevToken))
                            {
                                unary = true;
                                binary = false;
                            }
                            else
                            {
                                unary = false;
                                binary = true;
                            }
                        }
                        IInstruction op;
                        if (unary)
                        {
                            if (!(prevToken == null || IsOp(prevToken)))
                                throw NeonExceptions.Exception0004(token, lineNumber);
                            op = OPERATORS[UNARY_OPERATOR_TOKENS[token]];
                        }
                        else
                        {
                            op = OPERATORS[BINARY_OPERATOR_TOKENS[token]];
                        }
                        var l_assoc = Associativity(op) == 1;
                        if (operatorStack.Count > 0)
                        {
                            var top = operatorStack.Peek();
                            while (!(top is DUMMY_INSTR) && (l_assoc && Prec(top) >= Prec(op)) || (!l_assoc && Prec(top) > Prec(op)))
                            {
                                instructions.Add(operatorStack.Pop());
                                if (operatorStack.Count == 0)
                                    break;
                                top = operatorStack.Peek();
                            }
                        }
                        operatorStack.Push(op);
                    }
                    else if (tokenActions.ContainsKey(token))
                    {
                        tokenActions[token]();
                    }
                    else
                    {
                        // Change this later.
                        throw new Exception();
                    }
                    prevToken = token;
                }
            }

            foreach (var kvp in bracketLineStarts)
            {
                if (kvp.Value.Count != 0)
                    throw mismatchedOpenBracketExceptions[kvp.Key](kvp.Value.Last());
            }

            if (parsingState.Type == ParsingStateType.MultiLineComment)
                // It doesn't matter if we end on a single line comment.
                throw NeonExceptions.Exception0008((int)parsingState.Attributes["commentStart"]);

            // Add remaining operators.
            foreach (var op in operatorStack)
            {
                instructions.Add(op);
            }
        }

        private void _Parse_EXPR_START()
        {
            operatorStack.Push(EXPR_START_INSTR.Instance);
            bracketLineStarts[Tokens.EXPR_START].Push(lineNumber);
        }

        private void _Parse_ARRAY_START()
        {
            operatorStack.Push(ARRAY_START_INSTR.Instance);

        }

        private void _Parse_VEC_START()
        {
            BeginVec(
                ParsingStateType.Vector, 
                VEC_START_INSTR.Instance, 
                bracketLineStarts[Tokens.VEC_START]
                );
        }

        private void _Parse_RVEC_START()
        {
            BeginVec(
                ParsingStateType.RelativeVector, 
                RVEC_START_INSTR.Instance, 
                bracketLineStarts[Tokens.RVEC_START]
                );
        }

        private void _Parse_EXPR_END()
        {
            PopOpsUntil(
                EXPR_START_INSTR.Instance,
                bracketLineStarts[Tokens.EXPR_START],
                NeonExceptions.Exception0005(lineNumber)
                );
        }

        private void _Parse_VEC_END()
        {
            NeonSyntaxException _exc0010;
            if (bracketLineStarts[Tokens.VEC_START].Count > 0)
                _exc0010 = NeonExceptions.Exception0010(bracketLineStarts[Tokens.VEC_START].Peek());
            else
                _exc0010 = null;

            EndVec(
                ELEM_SEP_INSTR.Instance,
                bracketLineStarts[Tokens.VEC_START],
                NeonExceptions.Exception0009(lineNumber),
                NeonExceptions.Exception0010(lineNumber),
                BUILD_VEC.Instance
                );
        }

        private void _Parse_RVEC_END()
        {
            NeonSyntaxException _exc0012;
            if (bracketLineStarts[Tokens.RVEC_START].Count > 0)
                _exc0012 = NeonExceptions.Exception0012(bracketLineStarts[Tokens.RVEC_START].Peek());
            else
                _exc0012 = null;

            EndVec(
                RVEC_START_INSTR.Instance,
                bracketLineStarts[Tokens.RVEC_START],
                NeonExceptions.Exception0011(lineNumber),
                NeonExceptions.Exception0012(lineNumber),
                BUILD_RVEC.Instance
                );
        }

        private void _Parse_ELEM_SEP()
        {
            switch (parsingState.Type)
            {
                case ParsingStateType.Vector:
                    IInstruction terminal;
                    if ((int)parsingState.Attributes["componentCount"] == 1)
                        terminal = VEC_START_INSTR.Instance;
                    else
                        terminal = ELEM_SEP_INSTR.Instance;

                    var top = operatorStack.Peek();
                    while (top != terminal)
                    {
                        instructions.Add(operatorStack.Pop());
                        top = operatorStack.Peek();
                        // No need to check if the opStack is empty because we are guaranteed
                        // to encounter a terminal character if lineStarts.Count > 0.
                    }
                    operatorStack.Pop();
                    operatorStack.Push(ELEM_SEP_INSTR.Instance);

                    parsingState.Attributes["componentCount"]
                        = (int)(parsingState.Attributes["componentCount"]) + 1;
                    break;
                case ParsingStateType.RelativeVector:
                    parsingState.Attributes["componentCount"]
                        = (int)(parsingState.Attributes["componentCount"]) + 1;
                    break;
                default:
                    // Change this later.
                    throw new Exception();
            }
        }

        public List<IInstruction> GetInstructions()
        {
            return instructions;
        }

    }
}
