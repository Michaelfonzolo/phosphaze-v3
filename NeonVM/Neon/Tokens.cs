using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    internal static class Tokens
    {

        internal const string BIN_ADD = "+";

        internal const string BIN_SUB = "-";

        internal const string BIN_MUL = "*";

        internal const string BIN_DIV = "/";

        internal const string BIN_POW = "^";

        internal const string BIN_MOD = "%";

        internal const string UN_POS = "+";

        internal const string UN_NEG = "-";

        internal const string EXPR_START = "(";

        internal const string EXPR_END = ")";

        internal const string CALL_START = "(";

        internal const string CALL_END = ")";

        internal const string ARRAY_START = "[";

        internal const string ARRAY_END = "]";

        internal const string DICT_START = "[[";

        internal const string DICT_END = "]]";

        internal const string BLOCK_START = "{";

        internal const string BLOCK_END = "}";

        internal const string ELEM_SEP = ",";

        internal const string ASSIGN = "=";

        internal const string EOL = ";";

        internal const string KEY_VAR_CONN = ":";

        internal const string BIN_AND = "&&";

        internal const string BIN_OR = "||";

        internal const string UN_NOT = "!";

        internal const string BOOL_GT = ">";

        internal const string BOOL_LT = "<";

        internal const string BOOL_EQ = "==";

        internal const string BOOL_GE = ">=";

        internal const string BOOL_LE = "<=";

        internal const string BOOL_NE = "!=";

        internal const string IP_ADD = "+=";

        internal const string IP_SUB = "-=";

        internal const string IP_MUL = "*=";

        internal const string IP_DIV = "/=";

        internal const string IP_POW = "^=";

        internal const string IP_MOD = "%=";

        internal const string IP_AND = "&&=";

        internal const string IP_OR = "||=";

        internal const string VEC_START = "<<";

        internal const string VEC_END = ">>";

        internal const string RVEC_START = "<|";

        internal const string RVEC_END = "|>";

        internal const string ATTR_GET = "->";

        internal const string LAMBDA_MAP = "=>";

        internal const string DISCRETE_RANGE_GEN = "...";

        internal const string TIME_COMMAND = "$";

        internal const string AT_INTERVALS = "@";

        internal const string DURING_INTERVALS = "%";

        internal const string SINGLE_LINE_COMMENT = "//";

        internal const string INDEF_COMMENT_START = "/*";

        internal const string INDEF_COMMENT_END = "*/";

        internal const string LAMBDA_INDICATOR = "?";

        internal const string UNUSED = "#";

        internal const string WORD = @"[a-zA-Z_][a-zA-Z0-9_]*";

        internal const string NUMBER = @"[0-9]+(\.[0-9]+)?";


        // Internal Tokens
        // ===============
        // Internal tokens are internal representations of tokens that are not actually
        // valid syntax. For example, INTERNAL_UN_POS is 'u+', which just serves to differentiate
        // between the binary addition and unary positive operators in the ShuntingYard algorithm,
        // since they are both represented as a '+'.

        internal const string INTERNAL_UN_POS = "u+";

        internal const string INTERNAL_UN_NEG = "u-";

    }
}
