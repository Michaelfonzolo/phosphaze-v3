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
 * The LowerSISquaredTransition is an AbstractTransition whose transition function f(x) is
 * defined as
 * 
 *      f(x) = a SiS(bx) + 1
 *      
 * where a = 0.6120082707930399, b = 3.9633272, and the function SiS(x) is given by
 * 
 *      SiS(x) = Si(sgn(x)*x^2)
 *      
 * where Si is the "sine integral" defined by
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
using System;

#endregion

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class LowerSISquaredTransition : AbstractTransition
    {

        private Func<double, double> _func = 
            x => SpecialFunctions.Si(Math.Sign(x) * x * x);

        private double alpha, beta;

        public LowerSISquaredTransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        public LowerSISquaredTransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = deltaValue / 1.633964846102816;
            beta = 3.9633272 / duration;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * (_func(beta * (time - 1)) + 1.633964846102816) + initialValue;
        }

    }
}
