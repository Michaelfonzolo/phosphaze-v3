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

        // A dummy instruction representing the start of a vector.
        private class VEC_START_INSTR : DUMMY_INSTR, IInstruction 
        { 
            public static VEC_START_INSTR Instance = new VEC_START_INSTR(); 
        }

        private class RVEC_START_INSTR : DUMMY_INSTR, IInstruction
        {
            public static RVEC_START_INSTR Instance = new RVEC_START_INSTR();
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

        private Stack<string> tokenStack;

        private List<IInstruction> instructions = new List<IInstruction>();

        private int lineNumber = 0;
        private Stack<IInstruction> operatorStack = new Stack<IInstruction>();
        private Stack<int> exprStartLineNums = new Stack<int>();
        private Stack<int> vecStartLineNums = new Stack<int>();
        private Stack<int> rvecStartLineNums = new Stack<int>();
        private Stack<ParsingState> parsingStates = new Stack<ParsingState>();
        private ParsingState parsingState { get { return parsingStates.Peek(); } }

        public ShuntingYard(List<string> tokens)
        {
            tokens.Reverse();
            tokenStack = new Stack<string>(tokens);
            parsingStates.Push(new ParsingState(ParsingStateType.Default));
        }

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
                    else
                    {
                        ParseFurther(token);
                    }
                    prevToken = token;
                }
            }

            if (exprStartLineNums.Count != 0)
                // Throw an exception for the first mismatched opening bracket discovered.
                throw NeonExceptions.Exception0006(exprStartLineNums.Last());

            if (parsingState.Type == ParsingStateType.MultiLineComment)
                // It doesn't matter if we end on a single line comment.
                throw NeonExceptions.Exception0008((int)parsingState.Attributes["commentStart"]);

            // Add remaining operators.
            foreach (var op in operatorStack)
            {
                instructions.Add(op);
            }
        }

        private void ParseFurther(string token)
        {
            switch (token)
            {
                case Tokens.EXPR_START:
                    operatorStack.Push(EXPR_START_INSTR.Instance);
                    exprStartLineNums.Push(lineNumber);
                    break;
                case Tokens.VEC_START:
                    BeginVec(ParsingStateType.Vector, VEC_START_INSTR.Instance, vecStartLineNums);
                    break;
                case Tokens.RVEC_START:
                    BeginVec(ParsingStateType.RelativeVector, RVEC_START_INSTR.Instance, rvecStartLineNums);
                    break;
                case Tokens.EXPR_END:
                    PopOpsUntil(
                        EXPR_START_INSTR.Instance,
                        exprStartLineNums,
                        NeonExceptions.Exception0005(lineNumber));
                    break;
                case Tokens.VEC_END:
                    EndVec(
                        VEC_START_INSTR.Instance,
                        vecStartLineNums,
                        NeonExceptions.Exception0009(lineNumber),
                        NeonExceptions.Exception0010(vecStartLineNums.Peek()),
                        BUILD_VEC.Instance);
                    break;
                case Tokens.RVEC_END:
                    EndVec(
                        RVEC_START_INSTR.Instance,
                        rvecStartLineNums,
                        NeonExceptions.Exception0011(lineNumber),
                        NeonExceptions.Exception0012(lineNumber),
                        BUILD_RVEC.Instance);
                    break;
                case Tokens.ELEM_SEP:
                    switch (parsingState.Type)
                    {
                        case ParsingStateType.Vector:
                        case ParsingStateType.RelativeVector:
                            parsingState.Attributes["componentCount"]
                                = (int)(parsingState.Attributes["componentCount"]) + 1;
                            break;
                        default:
                            // Change this later.
                            throw new Exception();
                    }
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
