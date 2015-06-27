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
 * Date of Creation: 6/18/2015
 * 
 * Description
 * ===========
 * A WordTracker is a type of input tracker that detects when a word has been typed,
 * uninterruptedly. 
 * 
 * TERMINOLOGY: When we talk about a tracker being "refreshed", we mean it starts over.
 * A word tracker refreshes specifically when a key other then the next expected key
 * is pressed or a key has been held down for longer than the given keyPressLimit value
 * (which defaults to a sixth of a second).
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework.Input;
using Phosphaze_V3.Framework.Events;
using System;

#endregion

namespace Phosphaze_V3.Framework.Input
{
    public class WordTracker : EventListener
    {

        /// <summary>
        /// The word to track.
        /// </summary>
        public Keys[] word { get; private set; }

        /// <summary>
        /// The maximum time interval (in milliseconds) between pressing individual keys.
        /// </summary>
        private double maxInterval;

        /// <summary>
        /// The maximum time a key can be left pressed (in milliseconds) before the tracker is refreshed.
        /// </summary>
        private double keyPressLimit;

        /// <summary>
        /// The current character to check if pressed.
        /// </summary>
        private int currentChar = 0;

        /// <summary>
        /// The time when the last character was pressed.
        /// </summary>
        private double timeOfLastKey = 0;

        public WordTracker(
            EventPropagator eventPropagator, Keys[] word, 
            double maxInterval = Double.PositiveInfinity, 
            double keyPressLimit = 600)
            : base(eventPropagator)
        {
            this.word = word;
            this.maxInterval = maxInterval;
            this.keyPressLimit = keyPressLimit;
        }

        public void Update(ServiceLocator serviceLocator)
        {
            base.UpdateTime(serviceLocator);
        }

        public override void OnKeyClick(ServiceLocator serviceLocator, KeyEventArgs args)
        {
            if (args.key == word[currentChar] && LocalTime - timeOfLastKey < maxInterval)
            {
                currentChar++;
                timeOfLastKey = LocalTime;
            }
        }

        public override void OnKeyPress(ServiceLocator serviceLocator, KeyEventArgs args)
        {
            var index = serviceLocator.Keyboard.KeyToInt(args.key);
            // If any key has been pressed for longer than the keyPressLimit then
            // refresh the tracker (go back to the first character).
            if (serviceLocator.Keyboard.millisecondsSinceKeyPressed[index] > keyPressLimit)
                currentChar = 0;
        }

        /// <summary>
        /// Check if the given word has been typed.
        /// </summary>
        /// <returns></returns>
        public bool Typed()
        {
            if (currentChar == word.Length)
            {
                currentChar = 0;
                return true;
            }
            return false;
        }

    }
}
