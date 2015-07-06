#region License

// Copyright (c) 2015 FCDM
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished 
// to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region Header

/* Author: Michael Ala
 * Date of Creation: 7/4/2015
 * 
 * Description
 * ===========
 * A FunctionalAttributeEffector (FAE) is an IP-DFE which can simply be supplied a
 * Func<double, double> parameter as its ``Function`` overload. 
 * 
 * As per the definition of an AFAE, the ``Function`` returns the next value of the attribute, based 
 * on the local time and local frame of the FAE. Also, by the definition of an IP-DFE, the value 
 * returned by the ``Function`` simply overrides the previous value of the attribute (i.e. the
 * operation is O(x, y) = y).
 * 
 * An FAE always acts relative to the initial value of the attribute. For example, if the initial
 * value of the attribute is 3, and the first value returned by ``Function`` is 1, then the next
 * value of the attribute is 3 + 1 = 4, not 1. If the next value after that returned by ``Function``
 * is 2, then the next value of the attribute is 3 + 2 = 4.
 */

#endregion

#region Using Statements

using System;

#endregion

namespace Phosphaze.Framework.Forms.Effectors
{
    public class FunctionalAttributeEffector : InPlaceDoubleFunctionalEffector
    {

        /// <summary>
        /// The function that effects the attribute over time.
        /// </summary>
        public Func<double, double> func { get; private set; }

        /// <summary>
        /// The initial value of the attribute.
        /// </summary>
        public double initialValue { get; private set; }

        public FunctionalAttributeEffector(string attribute, Func<double, double> func)
            : base(attribute)
        {
            this.func = func;
        }

        public FunctionalAttributeEffector(string attribute, Func<double, double> func, Form form)
            : base(attribute, form)
        {
            this.func = func;
        }

        protected override void Initialize()
        {
            base.Initialize();
            initialValue = form.Attributes.GetAttr<double>(attributeName);
        }

        protected override double Function(double time, int frame)
        {
            return func(time) + initialValue;
        }

    }
}
