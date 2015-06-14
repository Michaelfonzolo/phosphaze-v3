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

using Phosphaze_V3.Framework.Timing;
using System;

#endregion

namespace Phosphaze_V3.Framework.Events
{
    public class EventListener : ChronometricEntity, IDisposable
    {

        public EventListener()
		    : base()
	    {
    		EventPropagator.TrackListener(this);
	    }

	    /// <summary>
	    /// Activated when the mouse has been pressed for exactly one frame.
	    /// </summary>
	    public virtual void OnMouseClick(MouseEventArgs args) { }

	    /// <summary>
	    /// Activated when the mouse has been pressed for two or more frames.
	    /// </summary>
	    public virtual void OnMousePress(MouseEventArgs args) { }

	    /// <summary>
	    /// Activated when the mouse has been unpressed for exactly one frame.
	    /// </summary>
	    public virtual void OnMouseRelease(MouseEventArgs args) { }

	    /// <summary>
	    /// Activated when the mouse has remained still for two or more frames.
	    /// </summary>
	    public virtual void OnMouseStill() { }

        /// <summary>
        /// Activated when the mouse scroll wheel has been changed.
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnScrollWheelChanged(MouseEventArgs args) { }

        // Tentative
        /*
	    /// <summary>
	    /// Activated when the mouse has entered this object's collision mask.
	    /// </summary>
	    public virtual void OnMouseEnter() { }

	    /// <summary>
	    /// Activated when the mouse is still inside this object's collision mask.
	    /// </summary>
	    public virtual void OnMouseHover() { }

	    /// <summary>
	    /// Activated when the mouse has left this object's collision mask.
	    /// </summary>
	    public virtual void OnMouseLeave() { }
         */

	    /// <summary>
	    /// Activated when a key has been pressed for exactly one frame.
	    /// </summary>
	    public virtual void OnKeyClick(KeyEventArgs args) { }

	    /// <summary>
	    /// Activated when a key has been pressed for two or more frames.
	    /// </summary>
	    public virtual void OnKeyPress(KeyEventArgs args) { }

	    /// <summary>
	    /// Activated when a key has been unpressed for exactly one frame.
	    /// </summary>
	    public virtual void OnKeyRelease(KeyEventArgs args) { }

	    public void Dispose()
	    {
		    EventPropagator.UntrackListener(this);
	    }

    }
}
