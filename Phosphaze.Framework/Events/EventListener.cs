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
 * An EventListener is any object that responds to Events received by the global EventPropagator.
 * It contains a number of virtual methods that can be override that respond to different events.
 */

#endregion

#region Using Statements

using Phosphaze.Framework.Timing;
using System;

#endregion

namespace Phosphaze.Framework.Events
{
    public class EventListener : ChronometricEntity
    {

        private EventPropagator propagator;

        public EventListener(EventPropagator propagator)
		    : base()
	    {
            this.propagator = propagator;
    		this.propagator.TrackListener(this);
	    }

	    /// <summary>
	    /// Activated when the mouse has been pressed for exactly one frame.
	    /// </summary>
	    public virtual void OnMouseClick(ServiceLocator serviceLocator, MouseEventArgs args) { }

	    /// <summary>
	    /// Activated when the mouse has been pressed for two or more frames.
	    /// </summary>
        public virtual void OnMousePress(ServiceLocator serviceLocator, MouseEventArgs args) { }

	    /// <summary>
	    /// Activated when the mouse has been unpressed for exactly one frame.
	    /// </summary>
        public virtual void OnMouseRelease(ServiceLocator serviceLocator, MouseEventArgs args) { }

	    /// <summary>
	    /// Activated when the mouse has remained still for two or more frames.
	    /// </summary>
	    public virtual void OnMouseStill(ServiceLocator serviceLocator) { }

        /// <summary>
        /// Activated when the mouse scroll wheel has been changed.
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnScrollWheelChanged(ServiceLocator serviceLocator, MouseEventArgs args) { }

        // Tentative
        /*
	    /// <summary>
	    /// Activated when the mouse has entered this object's collision mask.
	    /// </summary>
	    public virtual void OnMouseEnter(ServiceLocator serviceLocator) { }

	    /// <summary>
	    /// Activated when the mouse is still inside this object's collision mask.
	    /// </summary>
	    public virtual void OnMouseHover(ServiceLocator serviceLocator) { }

	    /// <summary>
	    /// Activated when the mouse has left this object's collision mask.
	    /// </summary>
	    public virtual void OnMouseLeave(ServiceLocator serviceLocator) { }
         */

        /// <summary>
	    /// Activated when a key has been pressed for exactly one frame.
	    /// </summary>
        public virtual void OnKeyClick(ServiceLocator serviceLocator, KeyEventArgs args) { }

	    /// <summary>
	    /// Activated when a key has been pressed for two or more frames.
	    /// </summary>
        public virtual void OnKeyPress(ServiceLocator serviceLocator, KeyEventArgs args) { }

	    /// <summary>
	    /// Activated when a key has been unpressed for exactly one frame.
	    /// </summary>
        public virtual void OnKeyRelease(ServiceLocator serviceLocator, KeyEventArgs args) { }

        /// <summary>
        /// Indicate to this listener that it should stop responding to events from the event propagator.
        /// </summary>
	    public void StopListening()
	    {
            if (propagator.IsTracking(this))
		        this.propagator.UntrackListener(this);
	    }

        /// <summary>
        /// Indicate to this listener that it should start responding to events from the event propagator.
        /// </summary>
        public void RestartListening()
        {
            if (!propagator.IsTracking(this))
                propagator.TrackListener(this);
        }

    }
}
