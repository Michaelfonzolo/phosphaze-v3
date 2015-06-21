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
using Phosphaze_V3.Framework.Timing;
using System;
using System.Collections.Generic;

#endregion

namespace Phosphaze_V3.Framework.Input
{
    public sealed class KeyboardInput : ChronometricEntity
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
        public static int[] FramesSinceKeyPressed { get { return Instance.FSKP; } }
        private int[] FSKP;

        /// <summary>
        /// The number of milliseconds each key has been pressed for.
        /// </summary>
        public static double[] MillisecondsSinceKeyPressed { get { return Instance.MSKP; } }
        private double[] MSKP;

        /// <summary>
        /// The number of frames each key has been unpressed for.
        /// </summary>
        public static int[] FramesSinceKeyUnpressed { get { return Instance.FSKU; } }
        private int[] FSKU;

        /// <summary>
        /// The number of milliseconds each key has been unpressed for.
        /// </summary>
        public static double[] MillisecondsSinceKeyUnpressed { get { return Instance.MSKU; } }
        private double[] MSKU;

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

            FSKP = new int[keysToIndex.Count];
            FSKU = new int[keysToIndex.Count];

            MSKP = new double[keysToIndex.Count];
            MSKU = new double[keysToIndex.Count];
        }

        /// <summary>
        /// The singleton instance of this object.
        /// </summary>
        private static KeyboardInput Instance = new KeyboardInput();

        /// <summary>
        /// Convert a key to an integer value representing it's index in the lists
        /// framesSinceKeyPressed, millisecondsSinceKeyPressed, framesSinceKeyUnpressed,
        /// and millisecondsSinceKeyUnpressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int KeyToInt(Keys key)
        {
            return keysToIndex[key];
        }

        public static void Update() { Instance._Update(); }

        private void _Update()
        {
            base.UpdateTime();
            currentKeyboardState = Keyboard.GetState();
            foreach (var pair in keysToIndex)
            {
                if (currentKeyboardState.IsKeyDown(pair.Key))
                {
                    FSKP[pair.Value]++;
                    MSKP[pair.Value] += TimeManager.DeltaTime;

                    FSKU[pair.Value] = 0;
                    MSKU[pair.Value] = 0;

                    var args = new KeyEventArgs(pair.Key);
                    if (FSKP[pair.Value] == 1)
                        EventPropagator.Send(new EventTypes.OnKeyClickEvent(), args);
                    else
                        EventPropagator.Send(new EventTypes.OnKeyPressEvent(), args);
                }
                else
                {
                    FSKP[pair.Value] = 0;
                    MSKP[pair.Value] = 0;

                    FSKU[pair.Value]++;
                    MSKU[pair.Value] += TimeManager.DeltaTime;

                    if (FSKU[pair.Value] == 1)
                        EventPropagator.Send(new EventTypes.OnKeyReleaseEvent(), new KeyEventArgs(pair.Key));
                }
            }
        }

        /// <summary>
        /// Check if a given key is pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsPressed(Keys key)
        {
            return Instance.FSKP[keysToIndex[key]] > 0;
        }

        /// <summary>
        /// Check if a given key is unpressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsUnpressed(Keys key)
        {
            return Instance.FSKU[keysToIndex[key]] > 0;
        }

        /// <summary>
        /// Check if a given key has been held down for the given number of frames or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public static bool IsHeld(Keys key, int frames)
        {
            return Instance.FSKP[keysToIndex[key]] >= frames;
        }

        /// <summary>
        /// Check if a given key has been held down for the given number of milliseconds or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static bool IsHeld(Keys key, double milliseconds)
        {
            return Instance.MSKP[keysToIndex[key]] >= milliseconds;
        }

        /// <summary>
        /// Check if a given key has been unheld for the given number of frames or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="frames"></param>
        /// <returns></returns>
        public static bool IsUnheld(Keys key, int frames)
        {
            return Instance.FSKU[keysToIndex[key]] >= frames;
        }

        /// <summary>
        /// Check if a given key has been unheld for the given number of milliseconds or more.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static bool IsUnheld(Keys key, double milliseconds)
        {
            return Instance.MSKU[keysToIndex[key]] >= milliseconds;
        }

        /// <summary>
        /// Check if a given key has been clicked (i.e. it has been held for exactly 1 frame).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsClicked(Keys key)
        {
            return Instance.FSKP[keysToIndex[key]] == 1;
        }

        /// <summary>
        /// Check if a given key has been released (i.e. it has been unheld for exactly 1 frame).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsReleased(Keys key)
        {
            // Checking if LocalFrame != 1 ensures that IsReleased doesn't return true immediately
            // as soon as the game starts, which would normally occur unless the player was holding
            // down the key before the game began running.
            return Instance.FSKU[keysToIndex[key]] == 1 && Instance.LocalFrame != 1;
        }

    }
}
