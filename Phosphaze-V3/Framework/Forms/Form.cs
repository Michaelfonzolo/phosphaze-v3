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
 * Date of Creation: 6/17/2015
 * 
 * Description
 * ===========
 * A Form is anything that can be added to a Multiform, can be updated over time, rendered,
 * and can have Effectors attached to it.
 */

#endregion

#region Using Statements

using Phosphaze_V3.Framework.Events;
using Phosphaze_V3.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Phosphaze_V3.Framework.Forms
{
    public class Form : EventListener
    {

        /// <summary>
        /// The parent multiform this form is attached to.
        /// </summary>
        public Multiform parent { get; private set; }

        /// <summary>
        /// The list of all anonymous effectors currently attached to this form.
        /// </summary>
        private List<Effector> anonymousEffectors = new List<Effector>();

        /// <summary>
        /// The dictionary of all named effectors currently attached to this form.
        /// </summary>
        private Dictionary<string, Effector> namedEffectors = new Dictionary<string, Effector>();

        public Form(ServiceLocator serviceLocator) 
            : base(serviceLocator.EventPropagator) 
        {
            
        }

        /// <summary>
        /// Set this form's parent.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(Multiform parent)
        {
            if (this.parent != null)
                throw new ArgumentException("This form has already been added to a multiform.");
            this.parent = parent;
        }

        /// <summary>
        /// Add an anonymous effector to this Form.
        /// </summary>
        /// <param name="effector"></param>
        public void AddEffector(Effector effector)
        {
            if (effector.form == null)
                effector.AttachTo(this);
            anonymousEffectors.Add(effector);
        }

        /// <summary>
        /// Add a named effector to this Form.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="effector"></param>
        public void AddEffector(string name, Effector effector)
        {
            if (effector.form == null)
                effector.AttachTo(this);
            namedEffectors[name] = effector;
        }

        /// <summary>
        /// Return a named effector.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Effector GetEffector(string name)
        {
            return namedEffectors[name];
        }

        /// <summary>
        /// Get a list of all active effectors attached to this Form.
        /// </summary>
        /// <returns></returns>
        public Effector[] GetEffectors()
        {
            return anonymousEffectors.ToArray().Concat(namedEffectors.Values.ToArray());
        }

        /// <summary>
        /// Clear the effectors attached to this Form.
        /// </summary>
        public void ClearEffectors()
        {
            anonymousEffectors.Clear();
            namedEffectors.Clear();
        }

        /// <summary>
        /// Update the Form.
        /// </summary>
        public virtual void Update(ServiceLocator serviceLocator)
        {
            base.UpdateTime(serviceLocator);
            UpdateEffectors(serviceLocator);
        }

        /// <summary>
        /// Update the effectors attached to this Form.
        /// </summary>
        private void UpdateEffectors(ServiceLocator serviceLocator)
        {
            foreach (var effector in anonymousEffectors)
                effector.Update(serviceLocator);
            anonymousEffectors.RemoveAll(e => e.dead);

            var dead = new List<string>();
            foreach (var effector in namedEffectors)
            {
                effector.Value.Update(serviceLocator);
                if (effector.Value.dead)
                    dead.Add(effector.Key);
            }

            foreach (var name in dead)
                namedEffectors.Remove(name);
        }

        /// <summary>
        /// Render the Form.
        /// </summary>
        public virtual void Render(ServiceLocator serviceLocator)
        {

        }

    }
}
