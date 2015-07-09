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
 * A Multiform is a section of a game that defines its own behaviour and visual appearance. 
 * For example, a Menu and a Level would be separate Multiforms. They are called Multiforms 
 * because they are composed of multiple Forms. Each Form is a self-contained component that
 * serves to make their encompassing Multiform functional.
 */

#endregion

#region Using Statements

using Phosphaze.Framework.Extensions;
using Phosphaze.Framework.Timing;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Phosphaze.Framework.Forms
{
    public abstract class Multiform : ChronometricEntity
    {

        private List<Form> anonymousForms = new List<Form>();

        private Dictionary<string, Form> namedForms = new Dictionary<string, Form>();

        /// <summary>
        /// The manager governing this Multiform.
        /// </summary>
        public MultiformManager manager { get; private set; }

        public Action<ServiceLocator> Updater { get; private set; }

        public Action<ServiceLocator> Renderer { get; private set; }

        public Multiform() 
            : base()
        { 
            manager = null;
        }

        /// <summary>
        /// Assign a manager to this multiform (only if one has not already been assigned).
        /// </summary>
        /// <param name="manager"></param>
        internal void SetManager(MultiformManager manager)
        {
            // We are only allowed to reset the manager if either the current manager is null or
            // the incoming manager is null.
            if (this.manager != null && manager != null)
                throw new ArgumentException("A manager has already been assigned.");
            this.manager = manager;
        }

        /// <summary>
        /// Set the current updater the multiform is using.
        /// </summary>
        /// <param name="updater"></param>
        protected void SetUpdater(Action<ServiceLocator> updater)
        {
            Updater = updater;
        }

        /// <summary>
        /// Set the current renderer the multiform is using.
        /// </summary>
        /// <param name="renderer"></param>
        protected void SetRenderer(Action<ServiceLocator> renderer)
        {
            Renderer = renderer;
        }

        /// <summary>
        /// Register a form.
        /// </summary>
        /// <param name="form"></param>
        protected void RegisterForm(Form form)
        {
            anonymousForms.Add(form);
            form.SetParent(this);
        }

        /// <summary>
        /// Register a form with a given name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="form"></param>
        /// <param name="serviceLocator"></param>
        protected void RegisterForm(string name, Form form)
        {
            namedForms[name] = form;
            form.SetParent(this);
        }

        /// <summary>
        /// Return a form by it's name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Form GetForm(string name)
        {
            return namedForms[name];
        }

        /// <summary>
        /// Clear all forms.
        /// </summary>
        protected void ClearForms()
        {
            namedForms.Clear();
            anonymousForms.Clear();
        }

        /// <summary>
        /// Remove a form with the given name.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveForm(string name)
        {
            namedForms.Remove(name);
        }

        /// <summary>
        /// Attempt to remove a form object.
        /// </summary>
        /// <param name="form"></param>
        public void RemoveForm(Form form)
        {
            var removed = anonymousForms.Remove(form);
            if (!removed)
            {
                var item = namedForms.First(i => i.Value == form);
                namedForms.Remove(item.Key);
            }
        }

        /// <summary>
        /// Return a list of all the forms in this multiform.
        /// </summary>
        /// <returns></returns>
        public Form[] AllForms()
        {
            return anonymousForms.ToArray().Concat(namedForms.Values.ToArray());
        }

        /// <summary>
        /// Update all the forms attached to this multiform.
        /// </summary>
        /// <param name="serviceLocator"></param>
        protected void UpdateForms(ServiceLocator serviceLocator)
        {
            foreach (var form in anonymousForms)
                form.Update(serviceLocator);
            foreach (var form in namedForms)
                form.Value.Update(serviceLocator);
        }

        /// <summary>
        /// Render all the forms attached to this multiform.
        /// </summary>
        /// <param name="serviceLocator"></param>
        protected void RenderForms(ServiceLocator serviceLocator)
        {
            foreach (var form in anonymousForms)
                form.Render(serviceLocator);
            foreach (var form in namedForms)
                form.Value.Render(serviceLocator);
        }

        protected void RenderForm(string name, ServiceLocator serviceLocator)
        {
            namedForms[name].Render(serviceLocator);
        }

        /// <summary>
        /// Tell all the active forms to stop listening to the event propagator.
        /// </summary>
        protected void StopListening()
        {
            foreach (var form in anonymousForms)
                form.StopListening();
            foreach (var form in namedForms)
                form.Value.StopListening();
        }

        /// <summary>
        /// Tell all the active forms to start listening to the event propagator.
        /// </summary>
        protected void StartListening()
        {
            foreach (var form in anonymousForms)
                form.RestartListening();
            foreach (var form in namedForms)
                form.Value.RestartListening();
        }

        /// <summary>
        /// Construct the Multiform. This gets called every time the MultiformManager switches
        /// to using this Multiform. TransitionArguments from the previous Multiform are passed in.
        /// </summary>
        /// <param name="args"></param>
        public abstract void Construct(ServiceLocator serviceLocator, MultiformData args);

        /// <summary>
        /// Close the multiform.
        /// </summary>
        /// <returns></returns>
        public virtual void Close(ServiceLocator serviceLocator) 
        {
            StopListening();
        }
    }
}
