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

using Phosphaze_V3.Framework.Timing;
using System;

#endregion

namespace Phosphaze_V3.Framework.Forms
{
    public abstract class Multiform : ChronometricEntity
    {

        /// <summary>
        /// The manager governing this Multiform.
        /// </summary>
        public MultiformManager manager { get; private set; }

        public MultiformState state { get; private set; }

        public Multiform() 
            : base() 
        { 
            LoadContent();
            manager = null;
            state = MultiformState.Update;
        }

        /// <summary>
        /// Assign a manager to this multiform (only if one has not already been assigned).
        /// </summary>
        /// <param name="manager"></param>
        public void SetManager(MultiformManager manager)
        {
            if (this.manager != null)
                throw new ArgumentException("This multiform already has a manager.");
            this.manager = manager;
        }

        public void SetState(MultiformState state)
        {
            this.state = state;
        }

        /// <summary>
        /// Load all the content for the Multiform. This is only called once. Not that as of
        /// calling this function, the Multiform's manager is null.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Construct the Multiform. This gets called every time the MultiformManager switches
        /// to using this Multiform. TransitionArguments from the previous Multiform are passed in.
        /// </summary>
        /// <param name="args"></param>
        public abstract void Construct(TransitionArguments args);

        /// <summary>
        /// Update the multiform.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Render the multiform.
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Render the multiform transitioning in from the previous multiform. By default this
        /// just defers to Render().
        /// </summary>
        /// <param name="previousMultiform"></param>
        public virtual void RenderTransitionIn(string previousMultiform)
        {
            Render();
        }

        /// <summary>
        /// Render the multiform transitioning out to the next multiform. By default this
        /// just defers to Render();
        /// </summary>
        /// <param name="nextMultiform"></param>
        public virtual void RenderTransitionOut(string nextMultiform)
        {
            Render();
        }

        /// <summary>
        /// Prepare and return the TransitionArguments to the next multiform.
        /// </summary>
        /// <param name="nextMultiform"></param>
        /// <returns></returns>
        public virtual TransitionArguments PrepareTransitionArgs(string nextMultiform)
        {
            return null;
        }

        /// <summary>
        /// Close the multiform.
        /// </summary>
        /// <returns></returns>
        public virtual void Close(string nextMultiform) { }

        public virtual TransitionType GetTransitionType(string nextMultiform)
        {
            return TransitionType.Independent;
        }

        /// <summary>
        /// Transition in from another multiform.
        /// </summary>
        public virtual void TransitionIn(string previousMultiform)
        {
            base.UpdateTime();
            // The default transition in is to just skip the transition entirely and go
            // straight to updating.
            SetState(MultiformState.Update);
        }

        /// <summary>
        /// Transition out to another multiform.
        /// </summary>
        public virtual void TransitionOut(string nextMultiform)
        {
            base.UpdateTime();
            // The default transition out is to just skip the transition entirely.
            SetState(MultiformState.Closed);
        }
    }
}
