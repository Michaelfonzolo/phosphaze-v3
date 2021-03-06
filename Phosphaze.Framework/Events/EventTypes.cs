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
 * This file contains all IEvent objects that can be passed to the EventPropagator.
 */

#endregion

#region Using Statements

using System;

#endregion

namespace Phosphaze.Framework.Events
{
    public static class EventTypes
    {
        /// <summary>
        /// An event that gets sent when a mouse button has been clicked.
        /// </summary>
        public class OnMouseClickEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnMouseClick(serviceLocator, (MouseEventArgs)args);
            }
        }

        /// <summary>
        /// An event that gets sent when a mouse button has been pressed.
        /// </summary>
        public class OnMousePressEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnMousePress(serviceLocator, (MouseEventArgs)args);
            }
        }

        /// <summary>
        /// An event that gets sent when a mouse button has been released.
        /// </summary>
        public class OnMouseReleaseEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnMouseRelease(serviceLocator, (MouseEventArgs)args);
            }
        }

        /// <summary>
        /// An event that gets sent when the mouse stays still.
        /// </summary>
        public class OnMouseStillEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnMouseStill(serviceLocator);
            }
        }

        /// <summary>
        /// An event that gets sent when the mouse scroll wheel value changes.
        /// </summary>
        public class OnScrollWheelChangedEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnScrollWheelChanged(serviceLocator, (MouseEventArgs)args);
            }
        }

        // Tentative
        /*
        public class OnMouseEnterEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnMouseEnter(serviceLocator);
            }
        }

        public class OnMouseHoverEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnMouseHover(serviceLocator);
            }
        }

        public class OnMouseLeaveEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnMouseLeave(serviceLocator);
            }
        }
         */

        /// <summary>
        /// An event that gets sent when a key is clicked.
        /// </summary>
        public class OnKeyClickEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnKeyClick(serviceLocator, (KeyEventArgs)args);
            }
        }

        /// <summary>
        /// An event that gets sent when a key is pressed.
        /// </summary>
        public class OnKeyPressEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnKeyPress(serviceLocator, (KeyEventArgs)args);
            }
        }

        /// <summary>
        /// An event that gets sent when a key is released.
        /// </summary>
        public class OnKeyReleaseEvent : IEvent
        {
            public void Activate(EventListener listener, EventArgs args, ServiceLocator serviceLocator)
            {
                listener.OnKeyRelease(serviceLocator, (KeyEventArgs)args);
            }
        }

    }
}
