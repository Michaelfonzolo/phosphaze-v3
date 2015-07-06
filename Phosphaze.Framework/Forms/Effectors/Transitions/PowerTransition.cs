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
  * The PowerTransition is an AbstractTransition whose transition function f(x) is
 * defined as
 * 
 *      f(x) = x^p
 */

#endregion

#region Using Statements

using System;

#endregion

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class PowerTransition : AbstractTransition
    {

        private double power;

        private double alpha;

        public PowerTransition(
            string attr
            , double finalValue
            , double duration
            , double power
            , bool relative = true)
            : base(attr, finalValue, duration, relative) 
        {
            this.power = power;
        }

        public PowerTransition(
            string attr
            , double finalValue
            , double duration
            , double power
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative)
        {
            this.power = power;
        }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = Math.Pow(deltaValue, 1.0 / power) / duration;
        }

        protected override double Function(double time, int frame)
        {
            return Math.Pow(alpha * time, power) + initialValue;
        }

    }
}
