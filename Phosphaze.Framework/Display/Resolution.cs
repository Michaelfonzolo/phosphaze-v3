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
 * Date of Creation: 6/15/2015
 * 
 * Description
 * ===========
 * A basic struct representing a valid screen resolution.
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using System.Windows.Forms;

#endregion

namespace Phosphaze_V3.Framework.Display
{
    /// <summary>
    /// A valid screen resolution.
    /// </summary>
    public struct Resolution
    {
        public int width, height;

        /// <summary>
        /// The aspect ratio of the screen.
        /// </summary>
        public double aspectRatio { get { return width / height; } }

        /// <summary>
        /// The center of the screen.
        /// </summary>
        public Vector2 center { get { return new Vector2(width / 2, height / 2); } }

        /// <summary>
        /// The minimum value of the width and height. This is used to properly scale
        /// sprites relative to the maximum valid resolution height.
        /// </summary>
        public int min { get { return width < height ? width : height; } }

        public Resolution(int w, int h)
        {
            width = w;
            height = h;
        }

        /// <summary>
        /// The native screen resolution.
        /// </summary>
        public static Resolution native = new Resolution(Screen.PrimaryScreen.Bounds.Width,
                                                         Screen.PrimaryScreen.Bounds.Height);
    }
}
