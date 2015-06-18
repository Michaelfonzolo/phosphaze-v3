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
 * The MultiformManager is a manager object that stores all multiforms in the game,
 * and initializes them, constructs, them, updates them, transitions between them,
 * transfers data between them, and destroys them accordingly. There can only be 
 * one active multiform at a time.
 * 
 * The MultiformManager also stores a dictionary of global forms not attached to
 * any multiform. 
 */

#endregion

#region Using Statements

using Phosphaze_V3.Framework.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;

#endregion

namespace Phosphaze_V3.Framework.Forms
{
    public class MultiformManager
    {

        /// <summary>
        /// The mapping of names to registered multiforms.
        /// </summary>
        Dictionary<string, Multiform> multiformMap = new Dictionary<string, Multiform>();
        
        /// <summary>
        /// The previous multiform's name (or null).
        /// </summary>
        string previousMultiform;

        /// <summary>
        /// The current multiform's name.
        /// </summary>
        string currentMultiform;

        /// <summary>
        /// The next multiform's name (or null).
        /// </summary>
        string nextMultiform;

        // We don't allow for multiple multiforms to be active at the same time, but most
        // of the time multiple multiforms are used to share information from multiform
        // to multiform (i.e. a background that is the same in one multiform as in another).
        // Because forms are already used to represent functionality in individual 
        // multiforms, allowing for global multiforms solves this problem of shared 
        // functionality.

        /// <summary>
        /// The manager of all global forms not attached to any multiform. The supplied
        /// GlobalFormManager must be a derived class of GlobalFormManager so as to implement
        /// its own functionality regarding the added scenes.
        /// </summary>
        GlobalFormManager globalForms;

        public MultiformManager(string initialMultiform) 
        {
            currentMultiform = initialMultiform;
        }

        /// <summary>
        /// Set the GlobalFormManager for this MultiformManager.
        /// </summary>
        /// <param name="manager"></param>
        public void SetGlobalFormManager(GlobalFormManager manager)
        {
            if (globalForms != null)
                throw new ArgumentException("This MultiformManager already has an assigned GlobalFormManager.");
            globalForms = manager;
        }

        /// <summary>
        /// Register a multiform to this manager.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="multiform"></param>
        public void RegisterMultiform(string name, Multiform multiform)
        {
            multiformMap[name] = multiform;
            multiform.SetManager(this);
        }

        /// <summary>
        /// Set the next multiform.
        /// </summary>
        /// <param name="name"></param>
        public void SetNextMultifom(string name)
        {
            nextMultiform = name;
        }

        /// <summary>
        /// Return the current multiform.
        /// </summary>
        /// <returns></returns>
        public Multiform GetCurrentMultiform()
        {
            return multiformMap[currentMultiform];
        }

        /// <summary>
        /// Return the previous multiform or null.
        /// </summary>
        /// <returns></returns>
        public Multiform GetPreviousMultiform()
        {
            return previousMultiform == null ? null : multiformMap[previousMultiform];
        }

        /// <summary>
        /// Return the next multiform or null.
        /// </summary>
        /// <returns></returns>
        public Multiform GetNextMultiform()
        {
            return nextMultiform == null ? null : multiformMap[nextMultiform];
        }

        /// <summary>
        /// Return a registered multiform by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Multiform GetMultiform(string name)
        {
            return multiformMap[name];
        }

        /// <summary>
        /// Return a global form by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Form GetGlobalForm(string name)
        {
            return globalForms.Get(name);
        }

        /// <summary>
        /// Return an array of all global forms.
        /// </summary>
        /// <returns></returns>
        public Form[] GetGlobalForms()
        {
            return globalForms.All();
        }

        /// <summary>
        /// Clear the list of global forms.
        /// </summary>
        public void Clear()
        {
            globalForms.Clear();
        }

        /// <summary>
        /// Remove a global form by its name.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            globalForms.Remove(name);
        }

        /// <summary>
        /// Add an anonymous form to the list of global forms.
        /// </summary>
        /// <param name="form"></param>
        public void AddGlobalForm(Form form)
        {
            globalForms.Add(form);
        }

        /// <summary>
        /// Add a named form to the list of global forms.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="form"></param>
        public void AddGlobalForm(string name, Form form)
        {
            globalForms.Add(name, form);
        }

        /// <summary>
        /// Update the current multiform and handle transitions.
        /// </summary>
        public void Update()
        {
            var multiform = multiformMap[currentMultiform];
            var state = multiform.state;
            Update(multiform, state);
            
            // During an intertwined transition, there's a chance the destination multiform changes state
            // from TransitionIn to Update before the source multiform changes state from TransitionOut to
            // Closed. In this case, the currentMultiform gets set to the destination, but we still have to
            // transition out the previous multiform.
            if (previousMultiform != null && multiformMap[previousMultiform].state == MultiformState.TransitionOut)
                multiformMap[previousMultiform].TransitionOut(currentMultiform);

            globalForms.Update();
        }

        /// <summary>
        /// Update the current multiform based on it's state.
        /// </summary>
        /// <param name="multiform"></param>
        /// <param name="state"></param>
        private void Update(Multiform multiform, MultiformState state)
        {
            switch (state)
            {
                case MultiformState.Update:
                    multiform.Update();
                    break;

                /* There are two different types of transitions, Independent and Intertwined.
                 * Independent means first the current Multiform's TransitionOut method is
                 * called, then the current Multiform is set to the next Multiform and its
                 * TransitionIn method is called.
                 * 
                 * Intertwined means both the current Multiform's TransitionOut method and
                 * the next Multiform's TransitionIn method are called simultaneously.
                 * 
                 * During an Intertwined transition, if the destination's TransitionIn method
                 * finishes before the source's TransitionOut method does (i.e. it's state gets
                 * changed to Update before the source's state gets changed to Closed), then
                 * currentMultiform is set to the destination and the previous multiform becomes
                 * the currentMultiform.
                 */

                case MultiformState.TransitionIn:
                    multiform.TransitionIn(previousMultiform);
                    break;
                case MultiformState.TransitionOut:
                    UpdateTransitionOut(multiform);
                    break;
                case MultiformState.Closed:
                    CloseCurrentScene(multiform);
                    break;
            }
        }

        /// <summary>
        /// Upate the current (and possibly next) multiforms transitioning out.
        /// </summary>
        /// <param name="multiform"></param>
        private void UpdateTransitionOut(Multiform multiform)
        {
            // Prevent transitioning if we don't have a nextMultiform.
            if (nextMultiform == null)
                throw new ArgumentException("Cannot transition without setting a destination multiform.");

            var transitionType = multiform.GetTransitionType(nextMultiform);
            if (transitionType == TransitionType.Intertwined)
            {
                var nextState = multiformMap[nextMultiform].state;
                if (nextState == MultiformState.Closed)
                    // This only occurs if we haven't actually begun transitioning the
                    // next multiform.
                    ConstructNextScene(multiform);
                else if (nextState == MultiformState.TransitionIn)
                    multiformMap[nextMultiform].TransitionIn(currentMultiform);
                else if (nextState == MultiformState.Update)
                    // If the next multiform finishes transitioning in before the current
                    // multiform finishes transitioning out, close the current multiform and
                    // shift the multiforms around so current becomes previous and next
                    // becomes current.
                    CloseCurrentScene(multiform);
            }
        }

        /// <summary>
        /// Close the current multiform and set up the next one.
        /// </summary>
        /// <param name="multiform"></param>
        private void CloseCurrentScene(Multiform multiform)
        {
            var transitionType = multiform.GetTransitionType(nextMultiform);
            if (transitionType == TransitionType.Independent)
                ConstructNextScene(multiform);

            // Shift the previous, current, and next scenes.
            previousMultiform = currentMultiform;
            currentMultiform = nextMultiform;
            nextMultiform = null;
            
        }

        /// <summary>
        /// Construct the next multiform.
        /// </summary>
        /// <param name="multiform"></param>
        private void ConstructNextScene(Multiform multiform)
        {
            var transitionArgs = multiform.PrepareTransitionArgs(nextMultiform);
            multiformMap[nextMultiform].SetState(MultiformState.TransitionIn);
            multiformMap[nextMultiform].Construct(transitionArgs);
        }

        /// <summary>
        /// Render the current (and possibly next) multiform(s).
        /// </summary>
        public void Render()
        {
            // RENDER ORDER: Global Forms, Next Multiform (if transitioning in and 
            // intertwined), Current Multiform.
            globalForms.Render();

            // We only render the next scene if the current scene's transition type
            // is Intertwined and the next scene is transitioning in.
            if (nextMultiform != null && multiformMap[nextMultiform].state == MultiformState.TransitionIn)
                multiformMap[nextMultiform].RenderTransitionIn(currentMultiform);

            Render(multiformMap[currentMultiform], multiformMap[currentMultiform].state);
        }

        /// <summary>
        /// Render the current multiform appropriately relative to its current state.
        /// </summary>
        /// <param name="multiform"></param>
        /// <param name="state"></param>
        private void Render(Multiform multiform, MultiformState state)
        {
            switch (state)
            {
                case MultiformState.Update:
                    multiform.Render();
                    break;
                case MultiformState.TransitionIn:
                    multiform.RenderTransitionIn(previousMultiform);
                    break;
                case MultiformState.TransitionOut:
                    multiform.RenderTransitionOut(nextMultiform);
                    break;
            }
        }
    }
}
