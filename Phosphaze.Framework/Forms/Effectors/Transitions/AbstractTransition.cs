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
 * An AbstractTransition represents an arbitrary Transition effector.
 * 
 * A Transition effector is an IP-DFE whose function is one f(x) : [0, 1] -> R with
 * the property that f(0) = 0 and f(1) = 1. Transitions are used to turn one value
 * into a new value over time, then when f(1) is reached, the transition kills itself.
 * 
 * For example, a LinearTransition is an AbstractTransition whose function f(x) is
 * defined as f(x) = x.
 * 
 * Internally, a transition actually uses the "normalization" of f, which we will
 * denote [f]. The normalization of f is simply a transformed version of f such that
 * f(0) = Vi and f(d) = Vf, where Vi is the initial value of the attribute, Vf is
 * the final value of the attribute, and d is the duration in milliseconds. Explicitly,
 * [f] is defined as
 * 
 *      [f](x) = (Vf - Vi) * f(x / d) + Vi
 */

#endregion

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public abstract class AbstractTransition : InPlaceDoubleFunctionalEffector
    {

        /// <summary>
        /// The difference of the finalValue and the initialValue.
        /// </summary>
        public double deltaValue { get; private set; }

        /// <summary>
        /// The initial value of the attribute being effected.
        /// </summary>
        public double initialValue { get; private set; }

        /// <summary>
        /// The desired final value of the attribute.
        /// </summary>
        public double finalValue { get; private set; }

        /// <summary>
        /// The duration of the transition.
        /// </summary>
        public double duration { get; private set; }

        private double y;

        private bool relative;

        public AbstractTransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr) 
        {
            y = finalValue;
            this.duration = duration;
            this.relative = relative;
        }

        public AbstractTransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, form)
        {
            y = finalValue;
            this.duration = duration;
            this.relative = relative;
        }

        protected override void Initialize()
        {
            initialValue = form.Attributes.GetAttr<double>(attributeName);
            if (relative)
                deltaValue = y;
            else
                deltaValue = y - initialValue;
            finalValue = deltaValue + initialValue;
        }

        public override void Update(ServiceLocator serviceLocator)
        {
            base.Update(serviceLocator);

            if (LocalTime >= duration)
            {
                form.Attributes.SetAttr<double>(attributeName, finalValue);
                Kill();
            }
        }

    }
}
