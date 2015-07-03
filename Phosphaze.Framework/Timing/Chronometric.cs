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
 * Date of Creation: 6/12/2015
 * 
 * Description
 * ===========
 * A Chronometric is anything that measures time. Chronometrics are just commands that
 * take in a ChronometricEntity and return either true or false depending on certain
 * time conditions.
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze.Framework.Timing
{
    public abstract class Chronometric
    {

        /// <summary>
        /// The action to perform when this TimeCommand's ActOn method returns true.
        /// </summary>
        public Action<ServiceLocator, ChronometricEntity> action { get; private set; }

        /// <summary>
        /// Assign an action to this TimeCommand and return ``this``. 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Chronometric WithAction(Action<ServiceLocator, ChronometricEntity> action)
        {
            this.action = action;
            return this;
        }

        /// <summary>
        /// Check if this time command is active for the given entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract bool Active(ChronometricEntity entity);

    }
}
