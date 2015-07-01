using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze_V3.Framework.Cache
{
    public class AttributeContainer
    {

        private Dictionary<Type, object> attributes = new Dictionary<Type, object>();

        public T GetAttr<T>(string name)
        {
            return ((Dictionary<string, T>)attributes[typeof(T)])[name];
        }

        public void SetAttr<T>(string name, T val)
        {
            var t = typeof(T);
            if (attributes.ContainsKey(t))
                ((Dictionary<string, T>)attributes[t])[name] = val;
            else
                attributes[t] = new Dictionary<string, T>() { { name, val } };
        }

    }
}
