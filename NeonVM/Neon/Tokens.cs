using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public static class Tokens
    {

        public const string BIN_ADD = "+";

        public const string BIN_SUB = "-";

        public const string BIN_MUL = "*";

        public const string BIN_DIV = "/";

        public const string BIN_POW = "^";

        public const string BIN_MOD = "%";

        public const string UN_POS = "+";

        public const string UN_NEG = "-";

        public const string EXPR_START = "(";

        public const string EXPR_END = ")";

        public const string CALL_START = "(";

        public const string CALL_END = ")";

        public const string DICT_START = "[";

        public const string DICT_END = "]";

        public const string CONT_RANGE_START = "[";

        public const string CONT_RANGE_END = "]";

        public const string BLOCK_START = "{";

        public const string BLOCK_END = "}";

        public const string ARRAY_START = "{";

        public const string ARRAY_END = "}";

        public const string ELEM_SEP = ",";

        public const string ASSIGN = "=";

        public const string EOL = ";";

        public const string KEY_VAR_CONN = ":";

        public const string BIN_AND = "&&";

        public const string BIN_OR = "||";

        public const string UN_NOT = "!";

        public const string BOOL_GT = ">";

        public const string BOOL_LT = "<";

        public const string BOOL_EQ = "==";

        public const string BOOL_GE = ">=";

        public const string BOOL_LE = "<=";

        public const string BOOL_NE = "!=";

        public const string IP_ADD = "+=";

        public const string IP_SUB = "-=";

        public const string IP_MUL = "*=";

        public const string IP_DIV = "/=";

        public const string IP_POW = "^=";

        public const string IP_MOD = "%=";

        public const string IP_AND = "&&=";

        public const string IP_OR = "||=";

        public const string VEC_START = "<<";

        public const string VEC_END = ">>";

        public const string RVEC_START = "<|";

        public const string RVEC_END = "|>";

        public const string ATTR_GET = "->";

        public const string CONT_RANGE_GEN = "->";

        public const string LAMBDA_MAP = "=>";

        public const string DISCRETE_RANGE_GEN = "...";

        public const string TIME_COMMAND = "$";

        public const string AT_INTERVALS = "@";

        public const string DURING_INTERVALS = "%";

        public const string SINGLE_LINE_COMMENT = "//";

        public const string INDEF_COMMENT_START = "/*";

        public const string INDEF_COMMENT_END = "*/";

        public const string UNUSED = "#";

        public const string WORD = @"[a-zA-Z_][a-zA-Z0-9_]*";

        public const string NUMBER = @"[0-9]+(\.[0-9]+)?";

    }
}
