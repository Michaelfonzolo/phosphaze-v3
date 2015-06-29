using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze_V3.Framework.Cache
{
    public class PropertyCache<T>
    {

        public T Value { get; set; }

        public bool dirty { get; set; }

    }
}
