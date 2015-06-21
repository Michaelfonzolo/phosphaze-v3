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
 * The TimingManager is a singleton that updates and manages global time-related
 * properties of the game including the current GameTime object (which itself contains
 * a number of properties), the milliseconds since the last call to Update(), and the
 * total elapsed time in milliseconds and in frames.
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using System;

#endregion

namespace Phosphaze_V3.Framework.Timing
{
    public class TimeManager
    {

        /// <summary>
        /// The global GameTime object.
        /// </summary>
        public static GameTime GameTime { get { return Instance.gameTime; } }
        private GameTime gameTime;

        /// <summary>
        /// The amount of time passed since the last call to Update (in milliseconds).
        /// </summary>
        public static double DeltaTime { get { return Instance.deltaTime; } }
        private double deltaTime;

        /// <summary>
        /// The total elapsed time since the beginning of the game in milliseconds.
        /// </summary>
        public static double ElapsedTime { get { return Instance.gameTime.ElapsedGameTime.TotalMilliseconds; } }

        /// <summary>
        /// The total number of elapsed frames since the beginning of the game.
        /// </summary>
        public static int ElapsedFrames { get { return Instance.elapsedFrames; } }
        private int elapsedFrames = 0;

        private TimeManager() { }

        private static TimeManager Instance = new TimeManager();

        public static void Update(GameTime gameTime) { Instance._Update(gameTime); }

        private void _Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            /* NOTE: gameTime.ElapsedGameTime.Milliseconds is unfortunately stored as an
             * integer, meaning at 60fps (which is the default for Monogame) deltaTime would
             * normally be 16 instead of the actual 16.66666...
             *
             * To compensate for this, since a game usually never lags enough to cause the
             * framerate to drop, we just set the deltaTime to 16.6666... whenever the elapsed
             * milliseconds is exactly 16 (which also happens to be its minimum).
             */
            this.deltaTime = Math.Max(gameTime.ElapsedGameTime.Milliseconds, Constants.MIN_DTIME);
            this.elapsedFrames++;
        }

    }
}
