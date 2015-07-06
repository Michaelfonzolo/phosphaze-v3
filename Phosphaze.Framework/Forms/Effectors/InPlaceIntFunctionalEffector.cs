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
 * An InPlaceIntFunctionalEffector (IP-IFE) is a IFE whose operation is in-place.
 * Explicitly, an IP-IFE's operation is defined as O(x, y) = y.
 */

#endregion

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class InPlaceIntFunctionalEffector : IntFunctionalEffector
    {

        public InPlaceIntFunctionalEffector(string attr) : base(attr) { }

        public InPlaceIntFunctionalEffector(string attr, Form form) : base(attr, form) { }

        protected override int Operate(int a, int b)
        {
            return b; // b is the result of calling Function, so just returning it overrides the
            // previous value.
        }

    }
}
