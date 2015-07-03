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

namespace Phosphaze.Framework.Events
{
    public sealed class EventPropagator
    {
        private ServiceLocator serviceLocator = null;

        public EventPropagator() { }

        public void SetServiceLocator(ServiceLocator serviceLocator)
        {
            /* We need a reference to the ServiceLocator so that we don't have to send
             * the ServiceLocator into EventPropagator.Send. 
             * 
             * An alternative solution to this problem would be to store all the events
             * sent in a list, and then in an Update(ServiceLocator) method they would
             * be activated simultaneously. This solution, however, doesn't account for
             * the fact that sometimes it is necessary for events to be activated as soon
             * as they are sent.
             */
            if (this.serviceLocator != null)
                throw new ArgumentException("The event propagator already has a reference to the service locator.");
            this.serviceLocator = serviceLocator;
        }

    	List<EventListener> tracking = new List<EventListener>();

        /// <summary>
        /// Check if the propagator is tracking the given listener.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public bool IsTracking(EventListener listener)
        {
            return tracking.Contains(listener);
        }

        /// <summary>
        /// Start tracking a listener object and propagating events to it.
        /// </summary>
        /// <param name="listener"></param>
    	public void TrackListener(EventListener listener)
	    {
		    if (listener == null)
			    throw new NullReferenceException("Supplied listener cannot be null.");
		    tracking.Add(listener);
	    }

        /// <summary>
        /// Untrack a listener.
        /// </summary>
        /// <param name="listener"></param>
	    public void UntrackListener(EventListener listener)
	    {
		    if (listener == null)
			    return;
            tracking.Remove(listener);
	    }

        /// <summary>
        /// Send an event object along with its arguments and propagate it upwards
        /// into each EventListener.
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="args"></param>
	    public void Send(IEvent evt, EventArgs args)
	    {
		    foreach (var listener in tracking)
			    evt.Activate(listener, args, serviceLocator);
	    }

    }
}
