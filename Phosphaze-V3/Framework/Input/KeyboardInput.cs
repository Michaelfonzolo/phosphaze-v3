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
 * The KeyboardInput object is a singleton that responds to keyboard input. It is like
 * MouseInput in that it is preferred over the builtin Monogame inputs because these
 * track more information.
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework.Input;
using Phosphaze_V3.Framework.Events;
using System;
using System.Collections.Generic;

#endregion

namespace Phosphaze_V3.Framework.Input
{
    public sealed class KeyboardInput
    {

        /// <summary>
        /// A dictionary mapping a Keys enumeration value to its appropriate
        /// index. This index is then used by framesSinceKeyPressed, 
        /// millisecondsSinceKeyPressed, framesSinceKeyUnpressed, and 
        /// millisecondsSinceKeyUnpressed.
        /// </summary>
        private static Dictionary<Keys, int> keysToIndex = new Dictionary<Keys, int>();

        /// <summary>
        /// The number of frames each key has been pressed for.
        /// </summary>
        public int[] framesSinceKeyPressed { get; private set; }

        /// <summary>
        /// The number of milliseconds each key has been pressed for.
        /// </summary>
        public double[] millisecondsSinceKeyPressed { get; private set; }

        /// <summary>
        /// The number of frames each key has been unpressed for.
        /// </summary>
        public int[] framesSinceKeyUnpressed { get; private set; }

        /// <summary>
        /// The number of milliseconds each key has been unpressed for.
        /// </summary>
        public double[] millisecondsSinceKeyUnpressed { get; private set; }

        /// <summary>
        /// The current keyboard state.
        /// </summary>
        private KeyboardState currentKeyboardState;

        /// <summary>
        /// Prevent external instantiation, as this is a singleton.
        /// </summary>
        private KeyboardInput()
        {
            var keyList = (Keys[])(Enum.GetValues(typeof(Keys)));
            for (int i = 0; i < keyList.Length; i++)
                keysToIndex[keyList[i]] = i;

            framesSinceKeyPressed = new int[keysToIndex.Count];
            framesSinceKeyUnpressed = new int[keysToIndex.Count];

            millisecondsSinceKeyPressed = new double[keysToIndex.Count];
            millisecondsSinceKeyUnpressed = new double[keysToIndex.Count];
        }

        /// <summary>
        /// The singleton instance of this object.
        /// </summary>
        public static KeyboardInput Instance = new KeyboardInput();

        public void Update()
        {
            currentKeyboardState = Keyboard.GetState();
            foreach (var pair in keysToIndex)
            {
                if (currentKeyboardState.IsKeyDown(pair.Key))
                {
                    framesSinceKeyPressed[pair.Value]++;
                    millisecondsSinceKeyPressed[pair.Value] += Globals.deltaTime;

                    framesSinceKeyUnpressed[pair.Value] = 0;
                    millisecondsSinceKeyUnpressed[pair.Value] = 0;

                    var args = new KeyEventArgs(pair.Key);
                    if (framesSinceKeyPressed[pair.Value] == 1)
                        EventPropagator.Send(new EventTypes.OnKeyClickEvent(), args);
                    else
                        EventPropagator.Send(new EventTypes.OnKeyPressEvent(), args);
                }
                else
                {
                    framesSinceKeyPressed[pair.Value] = 0;
                    millisecondsSinceKeyPressed[pair.Value] = 0;

                    framesSinceKeyUnpressed[pair.Value]++;
                    millisecondsSinceKeyUnpressed[pair.Value] += Globals.deltaTime;

                    if (framesSinceKeyUnpressed[pair.Value] == 1)
                        EventPropagator.Send(new EventTypes.OnKeyReleaseEvent(), new KeyEventArgs(pair.Key));
                }
            }
        }

        /// <summary>
        /// Check if a given key is pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsPressed(Keys key)
        {
            return framesSinceKeyPressed[keysToIndex[key]] > 0;
        }

        /// <summary>
        /// Check if a given key is unpressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsUnpressed(Keys key)
        {
            return framesSinceKeyUnpressed[keysToIndex[key]] > 0;
        }

        /// <summary>
        /// Check if a given key has been held down for the given number of frames or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsHeld(Keys key, int frames)
        {
            return framesSinceKeyPressed[keysToIndex[key]] >= frames;
        }

        /// <summary>
        /// Check if a given key has been held down for the given number of milliseconds or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool IsHeld(Keys key, double milliseconds)
        {
            return millisecondsSinceKeyPressed[keysToIndex[key]] >= milliseconds;
        }

        /// <summary>
        /// Check if a given key has been unheld for the given number of frames or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public bool IsUnheld(Keys key, int frames)
        {
            return framesSinceKeyUnpressed[keysToIndex[key]] >= frames;
        }

        /// <summary>
        /// Check if a given key has been unheld for the given number of milliseconds or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool IsUnheld(Keys key, double milliseconds)
        {
            return millisecondsSinceKeyUnpressed[keysToIndex[key]] >= milliseconds;
        }

        /// <summary>
        /// Check if a given key has been clicked (i.e. it has been held for exactly 1 frame).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsClicked(Keys key)
        {
            return framesSinceKeyPressed[keysToIndex[key]] == 1;
        }

        /// <summary>
        /// Check if a given key has been released (i.e. it has been unheld for exactly 1 frame).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsReleased(Keys key)
        {
            return framesSinceKeyUnpressed[keysToIndex[key]] == 1;
        }

    }
}
