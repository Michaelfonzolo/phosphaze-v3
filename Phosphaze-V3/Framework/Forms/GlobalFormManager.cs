using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze_V3.Framework.Events;
using Phosphaze_V3.Framework.Extensions;

namespace Phosphaze_V3.Framework.Forms
{
    public abstract class GlobalFormManager : EventListener 
    {

        /// <summary>
        /// The list of anonymous global forms.
        /// </summary>
        List<Form> anonymousGlobalForms = new List<Form>();

        /// <summary>
        /// The mapping of named global forms.
        /// </summary>
        Dictionary<string, Form> namedGlobalForms = new Dictionary<string, Form>();

        public GlobalFormManager() : base() { }

        /// <summary>
        /// Retrieve a form by it's name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Form Get(string name)
        {
            return namedGlobalForms[name];
        }

        /// <summary>
        /// Return an array of all global forms.
        /// </summary>
        /// <returns></returns>
        public Form[] All()
        {
            return anonymousGlobalForms.ToArray().Concat(namedGlobalForms.Values.ToArray());
        }

        /// <summary>
        /// Add an anonymous global form.
        /// </summary>
        /// <param name="form"></param>
        public void Add(Form form)
        {
            anonymousGlobalForms.Add(form);
        }

        /// <summary>
        /// Add a named global form.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="form"></param>
        public void Add(string name, Form form)
        {
            namedGlobalForms[name] = form;
        }

        /// <summary>
        /// Clear the list of global forms.
        /// </summary>
        public void Clear()
        {
            anonymousGlobalForms.Clear();
            namedGlobalForms.Clear();
        }

        /// <summary>
        /// Remove a global form by its name.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            namedGlobalForms.Remove(name);
        }

        /// <summary>
        /// Update the global forms. This is implementation specific.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Render the global forms. This is not by default implementation
        /// specific, but can be used to implement is own special rendering
        /// capabilities.
        /// </summary>
        public virtual void Render()
        {
            foreach (var form in anonymousGlobalForms)
                form.Render();
            foreach (var form in namedGlobalForms.Values)
                form.Render();
        }
    }
}
