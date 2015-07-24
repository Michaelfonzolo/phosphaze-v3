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

        internal const string EXPR_LEFT = "(";

        internal const string EXPR_RIGHT = ")";

        internal const string CALL_LEFT = "(";

        internal const string CALL_RIGHT = ")";

        internal const string ARRAY_LEFT = "[";

        internal const string ARRAY_RIGHT = "]";

        internal const string DICT_LEFT = "[[";

        internal const string DICT_RIGHT = "]]";

        internal const string BLOCK_LEFT = "{";

        internal const string BLOCK_RIGHT = "}";

        internal const string ELEM_SEP = ",";

        internal const string ASSIGN = "=";

        internal const string EOL = ";";

        internal const string KVP_CONN = ":";

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

        internal const string VEC_LEFT = "<<";

        internal const string VEC_RIGHT = ">>";

        internal const string RVEC_LEFT = "<|";

        internal const string RVEC_RIGHT = "|>";

        internal const string ATTR_GET = "->";

        internal const string LAMBDA_MAP = "=>";

        internal const string DISCRETE_RANGE_GEN = "...";

        internal const string TIME_COMMAND = "$";

        internal const string AT_INTERVALS = "@";

        internal const string DURING_INTERVALS = "%";

        internal const string SINGLE_LINE_COMMENT = "//";

        internal const string INDEF_COMMENT_LEFT = "/*";

        internal const string INDEF_COMMENT_RIGHT = "*/";

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
