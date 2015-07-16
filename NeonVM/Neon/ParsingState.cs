using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonVM.Neon
{
    public class ParsingState
    {

        public ParsingStateType Type { get; private set; }

        public Dictionary<string, object> Attributes { get; private set; }

        public ParsingState(ParsingStateType type)
        {
            Type = type;
            Attributes = new Dictionary<string, object>();
        }

    }

    public enum ParsingStateType
    {
        Default,
        SingleLineComment,
        MultiLineComment,
        Array,
        Dictionary,
        Vector,
        RelativeVector
    }
}
