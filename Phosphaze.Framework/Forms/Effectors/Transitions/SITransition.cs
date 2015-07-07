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
 * Date of Creation: 7/5/2015
 * 
 * Description
 * ===========
  * The SITransition is an AbstractTransition whose transition function f(x) is
 * defined as
 * 
 *      f(x) = a Si(bx)
 *      
 * where a = 0.30600413539651994, b = 10π = 31.415926, and Si is the "sine integral"
 * given by
 * 
 *               x                  x
 *              /                 /
 *      Si(x) = | sin(x)/x dx  =  | sinc(x) dx
 *              /                 /
 *              0                 0
 *              
 * where sinc(x) = sin(x) / x.
 */

#endregion

#region Using Statements

using Phosphaze.Framework.Maths;

#endregion

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class SITransition : AbstractTransition
    {

        private double alpha, beta;

        public SITransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        public SITransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = deltaValue / (2 * 1.6339648461);
            beta = 15.707963 * 2;
        } 

        protected override double Function(double time, int frame)
        {
            return alpha
                * (SpecialFunctions.SiApprox(beta * (time / duration - 0.5))
                    + 1.6339648461)
                + initialValue;
        }

    }
}
