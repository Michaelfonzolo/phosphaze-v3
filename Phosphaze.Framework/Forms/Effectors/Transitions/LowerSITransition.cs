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
 * The LowerSITransition is an AbstractTransition whose transition function f(x) is
 * defined as
 * 
 *      f(x) = a Si(bx) + 1
 *      
 * where a = 0.6120082707930399, b = 17.707963, and Si is the "sine integral" given by
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
    public class LowerSITransition : AbstractTransition
    {

        private double alpha, beta;

        public LowerSITransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        public LowerSITransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            // 15.707963 is the 6th root of Si'(x).
            // Si(15.707963) = 1.6339648461028329
            alpha = deltaValue / 1.6339648461028329;
            beta = 15.707963 / duration;
        }

        protected override double Function(double time, int frame)
        {
            return alpha
                * (SpecialFunctions.Si(
                    beta * (time - 1))
                    + 1.6339648461028329)
                + initialValue;
        }

    }
}
