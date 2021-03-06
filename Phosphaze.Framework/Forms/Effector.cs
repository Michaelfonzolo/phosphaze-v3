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
 * Date of Creation: 6/12/2015
 * 
 * Description
 * ===========
 * Effectors are objects that can be attached to Forms to update them over time externally
 * from the Form itself.
 */

#endregion

#region Using Statements

using Phosphaze.Framework.Timing;
using System;

#endregion

namespace Phosphaze.Framework.Forms
{
    public class Effector : ChronometricEntity
    {

        /// <summary>
        /// The form this effector is attached to.
        /// </summary>
        public Form form { get; private set; }

        /// <summary>
        /// Whether or not this effector is dead (finished, in which case it is deleted).
        /// </summary>
        public bool dead { get; private set; }

        private bool initialized = false;

        public Effector(Form form)
            : base()
        {
            this.form = form;
            dead = false;

            Initialize();
            initialized = true;
        }

        public Effector() 
        {
            form = null;
            dead = false;
            // We can't call Initialize from here, because no form has been attached.
        }

        /// <summary>
        /// Attach this effector to a form only if it isn't already.
        /// </summary>
        /// <param name="form"></param>
        public void AttachTo(Form form)
        {
            if (this.form != null)
                throw new ArgumentException("This effector is already attached to a form.");
            this.form = form;
            Initialize();
            initialized = true;
        }

        /// <summary>
        /// Initialization of the effector. This will only be called once as soon as
        /// the effector gets attached to a form. If you do not initialize the effector
        /// with a form, it will wait until it gets attached to call Initialize.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Update this effector.
        /// </summary>
        public virtual void Update(ServiceLocator serviceLocator)
        {
            if (!initialized)
                throw new EffectorException(
                    "Cannot begin updating an effector that hasn't been initialized. " +
                     "To initialize an effector, simply attach it to a form.");
            base.UpdateTime(serviceLocator);
        }

        public void Kill()
        {
            dead = true;
        }

    }
}
