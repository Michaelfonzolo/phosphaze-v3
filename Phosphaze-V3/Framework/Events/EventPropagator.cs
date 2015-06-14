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
 * The EventPropagator is a global singleton that responds to sent events by propagating
 * them upwards through each tracked EventListener.
 */

#endregion

#region Using Statements

using System;
using System.Collections.Generic;

#endregion

namespace Phosphaze_V3.Framework.Events
{
    public sealed class EventPropagator
    {

        private EventPropagator() { }

    	private static EventPropagator Instance = new EventPropagator();

    	List<EventListener> tracking = new List<EventListener>();

        /// <summary>
        /// Start tracking a listener object and propagating events to it.
        /// </summary>
        /// <param name="listener"></param>
    	public static void TrackListener(EventListener listener)
	    {
		    if (listener == null)
			    throw new NullReferenceException("Supplied listener cannot be null.");
		    Instance.tracking.Add(listener);
	    }

        /// <summary>
        /// Untrack a listener.
        /// </summary>
        /// <param name="listener"></param>
	    public static void UntrackListener(EventListener listener)
	    {
		    if (listener == null)
			    return;
            Instance.tracking.Remove(listener);
	    }

        /// <summary>
        /// Send an event object along with its arguments and propagate it upwards
        /// into each EventListener.
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="args"></param>
	    public static void Send(IEvent evt, EventArgs args)
	    {
		    foreach (var listener in Instance.tracking)
			    evt.Activate(listener, args);
	    }

    }
}
