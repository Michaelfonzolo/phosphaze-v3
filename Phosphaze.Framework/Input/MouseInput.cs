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
 * The MouseInput object is a singleton that tracks input from the mouse. It is
 * preferred over the builtin Monogame MouseInput because it records more than simply
 * the current state of the mouse (for example, it records the previous state of the mouse,
 * and the amount of time for which a button has been held down).
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Phosphaze.Framework.Events;
using Phosphaze.Framework.Timing;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Phosphaze.Framework.Input
{
    public sealed class MouseInput : ChronometricEntity
    {

        /// <summary>
        /// The valid mouse buttons from which input can be retrieved.
        /// </summary>
        public enum MouseButton { Left, Middle, Right }

        /// <summary>
        /// The number of frames since a mouse button was pressed. The
        /// first, second, and third elements of the array correspond to
        /// the left, middle, and right mouse buttons respectively.
        /// </summary>
        public int[] framesSinceMousePressed { get; private set; }

        /// <summary>
        /// The number of milliseconds since a mouse button was pressed. The
        /// first, second, and third elements of the array correspond to the
        /// left, middle, and right mouse buttons respectively.
        /// </summary>
        public double[] millisecondsSinceMousePressed { get; private set; }

        /// <summary>
        /// The number of frames since a mouse button was pressed. The
        /// first, second, and third elements of the array correspond to
        /// the left, middle, and right mouse buttons respectively.
        /// </summary>
        public int[] framesSinceMouseUnpressed { get; private set; }

        /// <summary>
        /// The number of milliseconds since a mouse button was upressed. The
        /// first, second, and third elements of the array correspond to the
        /// left, middle, and right mouse buttons respectively.
        /// </summary>
        public double[] millisecondsSinceMouseUnpressed { get; private set; }

        /// <summary>
        /// The current mouse state (type Microsoft.Xna.Framework.Input.MouseState).
        /// </summary>
        private MouseState currentMouseState;

        /// <summary>
        /// The stack of previous mouse positions. This stores the last second of
        /// mouse positions.
        /// </summary>
        public Stack<Point> prevMousePositions { get; private set; }

        /// <summary>
        /// The number of frames since the last time the mouse moved.
        /// </summary>
        public int framesSinceMouseMovement { get; private set; }

        /// <summary>
        /// The number of milliseconds since the last time the mouse moved.
        /// </summary>
        public double millisecondsSinceMouseMovement { get; private set; }

        /// <summary>
        /// The previous number of frames since mouse movement.
        /// </summary>
        public int prevFramesSinceMouseMovement { get; private set; }

        /// <summary>
        /// The previous number of milliseconds since mouse movement.
        /// </summary>
        public double prevMillisecondsSinceMouseMovement { get; private set; }

        /// <summary>
        /// The position of the mouse.
        /// </summary>
        public Point mousePosition { get; private set; }

        public Vector2 mousePosAsVec
        {
            get
            {
                return new Vector2(mousePosition.X, mousePosition.Y);
            }
        }

        /// <summary>
        /// The previous cumulative scroll wheel value.
        /// </summary>
        public int prevScrollWheelVal { get; private set; }

        /// <summary>
        /// The cumulative scroll wheel value since the game began.
        /// </summary>
        public int scrollWheelValue { get; private set; }

        /// <summary>
        /// Return the change in the scroll wheel value since the last call to Update.
        /// </summary>
        public int deltaScrollWheelValue { get { return scrollWheelValue - prevScrollWheelVal; } }

        public double mouseSpeed
        {
            get
            {
                var _v = prevMousePositions.Peek();
                var v1 = new Vector2(_v.X, _v.Y);
                var v2 = new Vector2(mousePosition.X, mousePosition.Y);
                return Vector2.Distance(v1, v2);
            }
        }

        /// <summary>
        /// Return the velocity of the mouse movement in
        /// </summary>
        public Vector2 mouseVelocity
        {
            get
            {
                var _v = prevMousePositions.Peek();
                var v1 = new Vector2(_v.X, _v.Y);
                var v2 = new Vector2(mousePosition.X, mousePosition.Y);
                return v2 - v1;
            }
        }

        /// <summary>
        /// Prevent external instantiation, as this is a singleton.
        /// </summary>
        public MouseInput()
        {
            framesSinceMousePressed = new int[] { 0, 0, 0 };
            framesSinceMouseUnpressed = new int[] { 0, 0, 0 };
            millisecondsSinceMousePressed = new double[] { 0, 0, 0 };
            millisecondsSinceMouseUnpressed = new double[] { 0, 0, 0 };

            framesSinceMouseMovement = 0;
            millisecondsSinceMouseMovement = 0;
            prevFramesSinceMouseMovement = 0;
            prevMillisecondsSinceMouseMovement = 0;

            prevMousePositions = new Stack<Point>(60);

            prevScrollWheelVal = 0;
            scrollWheelValue = 0;
        }

        /// <summary>
        /// Convert a MouseButton into it's appropriate index in the list
        /// framesSinceMousePressed.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public int ButtonToIndex(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left: return 0;
                case MouseButton.Middle: return 1;
                case MouseButton.Right: return 2;
                default: return 0; // Should never occur.
            }
        }

        /// <summary>
        /// Update the mouse state to retrieve new input.
        /// </summary>
        public void Update(ServiceLocator serviceLocator)
        {
            base.UpdateTime(serviceLocator);
            currentMouseState = Mouse.GetState();

            prevMousePositions.Push(mousePosition);
            mousePosition = currentMouseState.Position;

            if (mousePosition == prevMousePositions.First())
            {
                framesSinceMouseMovement++;
                millisecondsSinceMouseMovement += serviceLocator.Engine.deltaTime;
                serviceLocator.EventPropagator.Send(new EventTypes.OnMouseStillEvent(), MouseEventArgs.Empty);
            }
            else
            {
                framesSinceMouseMovement = 0;
                millisecondsSinceMouseMovement = 0;
            }

            prevScrollWheelVal = scrollWheelValue;
            scrollWheelValue = currentMouseState.ScrollWheelValue;

            if (deltaScrollWheelValue != 0)
                serviceLocator.EventPropagator.Send(new EventTypes.OnScrollWheelChangedEvent(), MouseEventArgs.Empty);

            UpdateButton(MouseButton.Left, serviceLocator);
            UpdateButton(MouseButton.Middle, serviceLocator);
            UpdateButton(MouseButton.Right, serviceLocator);
        }

        /// <summary>
        /// Update an individual mouse button.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="position"></param>
        private void UpdateButton(MouseButton button, ServiceLocator serviceLocator)
        {
            int pos = ButtonToIndex(button);
            if (IsPressed(button))
            {
                framesSinceMousePressed[pos]++;
                millisecondsSinceMousePressed[pos] += serviceLocator.Engine.deltaTime;

                framesSinceMouseUnpressed[pos] = 0;
                millisecondsSinceMouseUnpressed[pos] = 0;

                var args = new MouseEventArgs(button);
                if (framesSinceMousePressed[pos] == 1)
                    serviceLocator.EventPropagator.Send(new EventTypes.OnMouseClickEvent(), args);
                else
                    serviceLocator.EventPropagator.Send(new EventTypes.OnMousePressEvent(), args);
            }
            else
            {
                framesSinceMousePressed[pos] = 0;
                millisecondsSinceMousePressed[pos] = 0;

                framesSinceMouseUnpressed[pos]++;
                millisecondsSinceMouseUnpressed[pos] += serviceLocator.Engine.deltaTime;

                if (framesSinceMouseUnpressed[pos] == 1)
                    serviceLocator.EventPropagator.Send(new EventTypes.OnMouseReleaseEvent(), new MouseEventArgs(button));
            }
        }

        /// <summary>
        /// Check if a given mouse button is pressed or not. A button is pressed
        /// iff it has been held for greater than or equal to 1 frame.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsPressed(MouseButton button)
        {
            // We can't just check if framesSinceMousePressed[ButtonToPosition(button)] > 0
            // because that would break UpdateButton.
            switch (button)
            {
                case MouseButton.Left:   return currentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Middle: return currentMouseState.MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:  return currentMouseState.RightButton == ButtonState.Pressed;
                default: return false;
            }
        }

        /// <summary>
        /// Check if a mouse button has been held for a given number of frames or more.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsHeld(MouseButton button, int frames)
        {
            return framesSinceMousePressed[ButtonToIndex(button)] >= frames;
        }

        /// <summary>
        /// Check if a mouse button has been held for a given number of milliseconds or more.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool IsHeld(MouseButton button, double milliseconds)
        {
            return millisecondsSinceMousePressed[ButtonToIndex(button)] >= milliseconds;
        }

        /// <summary>
        /// Check if a mouse button has been unheld for a given number of frames or more.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsUnheld(MouseButton button, int frames)
        {
            return framesSinceMouseUnpressed[ButtonToIndex(button)] >= frames;
        }

        /// <summary>
        /// Check if a mouse button has been unheld for a given number of milliseconds or more.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool IsUnheld(MouseButton button, double milliseconds)
        {
            return millisecondsSinceMouseUnpressed[ButtonToIndex(button)] >= milliseconds;
        }

        /// <summary>
        /// Check if a mouse button has been clicked (i.e. it has been held for exactly one frame).
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsClicked(MouseButton button)
        {
            return framesSinceMousePressed[ButtonToIndex(button)] == 1;
        }

        /// <summary>
        /// Check if a mouse button has been released (i.e. it has been unpressed for exactly one frame).
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsReleased(MouseButton button)
        {
            // Checking if LocalFrame != 1 ensures that this doesn't return true immediately as
            // soon as the game begins, which would normally occur unless the player was pressing
            // the button before the game began.
            return framesSinceMouseUnpressed[ButtonToIndex(button)] == 1 && LocalFrame != 1;
        }

        /// <summary>
        /// Check if the mouse has not moved since the last call to Update.
        /// </summary>
        /// <returns></returns>
        public bool IsMouseStill()
        {
            return framesSinceMouseMovement > 0;
        }

        /// <summary>
        /// Check if the mouse has not moved for the given number of frames.
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsMouseStill(int frames)
        {
            return framesSinceMouseMovement >= frames;
        }

        /// <summary>
        /// Check if the mouse has not moved for the given number of milliseconds.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool IsMouseStill(double milliseconds)
        {
            return millisecondsSinceMouseMovement >= milliseconds;
        }

        /// <summary>
        /// Check if the mouse just began moving after being still for any variable period of time.
        /// </summary>
        /// <returns></returns>
        public bool MouseJustMoved()
        {
            return framesSinceMouseMovement == 1 && 
                   prevFramesSinceMouseMovement > 0;
        }

        /// <summary>
        /// Check if the mouse began moving after being still for the given number of frames or more.
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool MouseMovedAfter(int frames)
        {
            return framesSinceMouseMovement == 0 && 
                   prevFramesSinceMouseMovement >= frames;
        }

        /// <summary>
        /// Check if the mouse began moving after being still for the given number of milliseconds or more.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool MouseMovedAfter(double milliseconds)
        {
            return millisecondsSinceMouseMovement == 0 && 
                   prevMillisecondsSinceMouseMovement >= milliseconds;
        }
    }
}
