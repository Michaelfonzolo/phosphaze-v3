﻿#region License

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
 * A MultiplicativeIntFunctionalEffector (M-IFE) is a IFE whose operation is multiplicative.
 * Explicitly, an M-IFE's operation is defined as O(x, y) = xy.
 */

#endregion

#region Using Statements

using System;

#endregion

namespace Phosphaze.Framework.Forms.Effectors
{
    public class MultiplicativeFunctionalAttributeEffector : MultiplicativeDoubleFunctionalEffector
    {

        public Func<double, double> func { get; private set; }

        public MultiplicativeFunctionalAttributeEffector(string attribute, Func<double, double> func)
            : base(attribute)
        {
            this.func = func;
        }

        public MultiplicativeFunctionalAttributeEffector(string attribute, Func<double, double> func, Form form)
            : base(attribute, form)
        {
            this.func = func;
        }

        protected override double Function(double time, int frame)
        {
            return func(time);
        }

    }
}
