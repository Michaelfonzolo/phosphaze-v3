using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze_V3.Framework.Cache
{
    public class PropertyCache<T>
    {

        public T Value { get; set; }

        public T Clean
        {
            set
            {
                Value = value;
                dirty = false;
            }
        }

        public bool dirty { get; set; }

        public PropertyCache()
        {
            dirty = true;
        }

    }
}
