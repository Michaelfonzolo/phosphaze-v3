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
 * This file contains all constants used in the Phosphaze game. Constants
 * are different from Globals as Globals change depending on the game's
 * state, and they are different from Options because Options are user
 * specified.
 */

#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Phosphaze_V3.Framework
{
    /// <summary>
    /// The constants used throughout Phosphaze.
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// The background fill colour.
        /// </summary>
        public static Color BG_FILLCOL = Color.Black;

        /// <summary>
        /// The minimum possible value of Globals.deltaTime.
        /// </summary>
        public const double MIN_DTIME = 16.6666666666666;

        /// <summary>
        /// The multiplicative constant to convert degrees into radians.
        /// </summary>
        public const double DEG_TO_RAD = Math.PI / 180;

        /// <summary>
        /// The amount of pixels the border of a window offsets the display by.
        /// </summary>
        public static System.Drawing.Point WINDOW_BOUNDARY_OFFSET = new System.Drawing.Point(8, 30);

        /// <summary>
        /// The global random number generator.
        /// </summary>
        public static Random random = new Random();

    }
}
