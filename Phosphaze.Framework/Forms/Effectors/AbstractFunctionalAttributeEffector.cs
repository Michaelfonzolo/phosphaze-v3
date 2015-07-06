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
 * An AbstractFunctionalAttributeEffector (AFAE) is an Effector that represents a function which 
 * acts on an attribute of a form. Each form has an AttributeContainer object attached to it,
 * which the AFAE reads and assigns data to.
 * 
 * The AFAE represents a simple input output system. Let Vi be the current value of the attribute
 * (the input value), and Vo be the next value of the attribute. An AFAE is described by the following
 * equation.
 * 
 *    Vo = O(Vi, F(t, f))
 *    
 * Where F is a function, t & f are the local time and frames of the AFAE, and O is an operation 
 * combining the previous value of Vi and the result of f(t). The function F is defined by the 
 * abstract method ``Function(double localTime, int localFrame) : T``.
 * 
 * The definition of O is given by the abstract method ``Operator(T a, T b) : T``, with the 
 * first parameter being the previous value of the attribute, and the second parameter being 
 * the next value of the attribute (the result of calling ``Function(LocalTime)``).
 * 
 * Common examples of the operation O(x, y) are as follows:
 * 
 *  In-place       : O(x, y) = y
 *  Additive       : O(x, y) = x + y
 *  Multiplicative : O(x, y) = xy
 *  
 */

#endregion

namespace Phosphaze.Framework.Forms.Effectors
{
    public abstract class AbstractFunctionalAttributeEffector<T> : Effector
    {

        /// <summary>
        /// The name of the attribute this AFAE is effecting.
        /// </summary>
        protected string attributeName { get; private set; }

        public AbstractFunctionalAttributeEffector(string attr)
            : base()
        {
            attributeName = attr;
        }

        public AbstractFunctionalAttributeEffector(string attr, Form form)
            : base(form)
        {
            attributeName = attr;
        }

        public override void Update(ServiceLocator serviceLocator)
        {
            base.Update(serviceLocator);
            form.Attributes.SetAttr<T>(
                attributeName, Operate(
                    form.Attributes.GetAttr<T>(attributeName),
                    Function(LocalTime, LocalFrame)
                    )
                );
        }

        /// <summary>
        /// Combine the previous value of the attribute and the next value of the attribute.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract T Operate(T a, T b);

        /// <summary>
        /// Calculate the next value of the attribute based on the local time and local frame.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected abstract T Function(double time, int frame);

    }
}
