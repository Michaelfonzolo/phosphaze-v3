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
 * Date of Creation: 6/24/2015
 * 
 * Description
 * ===========
 * A set of geometric utility methods regarding vectors.
 */

#region Using Statements

using Microsoft.Xna.Framework;
using System;

#endregion

namespace Phosphaze_V3.Framework.Maths.Geometry
{
    public static class VectorUtils
    {

        /// <summary>
        /// Return the unit vector pointing at the given angle relative to the positive x-axis.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 Polar(double angle, bool degrees = true)
        {
            if (degrees)
                angle *= Constants.DEG_TO_RAD;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        /// <summary>
        /// Return the unit vector pointing at the given angle relative to the positive x-axis,
        /// centered at the given origin.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="origin"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 Polar(double angle, Vector2 origin, bool degrees = true)
        {
            if (degrees)
                angle *= Constants.DEG_TO_RAD;
            return new Vector2((float)Math.Cos(angle) + origin.X, (float)Math.Sin(angle) + origin.Y);
        }

        /// <summary>
        /// Rotate a vector by the given amount relative to the given point, and return the result.
        /// </summary>
        public static Vector2 Rotate(
            Vector2 vec, double angle, Vector2 origin, 
            bool degrees = true, bool relative = true)
        {
            if (degrees)
                angle *= Constants.DEG_TO_RAD;

            double x, y, nx, ny;

            if (relative)
            {
                x = -origin.X;
                y = -origin.Y;
            }
            else
            {
                x = vec.X - origin.X;
                y = vec.Y - origin.Y;
            }
            double cos_theta = Math.Cos(angle);
            double sin_theta = Math.Sin(angle);

            nx = x * cos_theta - y * sin_theta;
            ny = y * sin_theta + x * cos_theta;

            if (relative)
            {
                nx += vec.X;
                ny += vec.Y;
            }
            else
            {
                nx += origin.X;
                ny += origin.Y;
            }
            return new Vector2((float)(nx), (float)(ny));
        }

        /// <summary>
        /// Return the normalization of a given vector.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector2 ToNormalized(Vector2 vec)
        {
            return vec / vec.Length();
        }

        /// <summary>
        /// Scale a vector by the given amount relative to the given point, and return the result.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="amount"></param>
        /// <param name="origin"></param>
        /// <param name="relative"></param>
        /// <returns></returns>
        public static Vector2 Scale(
            Vector2 vec, double amount, Vector2 origin, bool relative = true)
        {
            if (relative)
                return vec + origin * (1f - (float)amount);
            return (vec - origin) * (float)amount + origin;
        }

    }
}
