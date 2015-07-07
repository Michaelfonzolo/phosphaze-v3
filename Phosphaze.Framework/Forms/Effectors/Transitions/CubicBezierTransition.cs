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
 * Date of Creation: 7/5/2015
 * 
 * Description
 * ===========
 * The CubicBezierTransition is an Abstract Transition whose transition function f(x)
 * is defined as
 * 
 *      f(x) = (1 + 3b - 3q) t^3 + 3(q - 2b) t^2 + 3b t
 *      
 * where t is the specific number such that
 * 
 *        x = (1 + 3a - 3p) t^3 + 3(p - 2a) t^2 + 3a t
 *        
 * Here a, b, p, and q are parameters. They represent the coordinates of the second
 * and third control points of the cubic bezier curve respectively (the control polygon
 * of the curve is <0, 0>, <a, b>, <p, q>, <1, 1>).
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Phosphaze.Framework.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class CubicBezierTransition : AbstractTransition
    {

        public Vector2 A { get; private set; }

        public Vector2 B { get; private set; }

        private double[] coeffs;

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true
            )
            : this(attr, finalValue, duration, CubicBezierPresets.EaseInOut, relative) { }

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , CubicBezierPresets preset
            , bool relative = true)
            : base(attr, finalValue, duration, relative)
        {
            // We don't have to check the positions of the preset because all of them
            // are guaranteed to have valid control points.
            A = preset.A;
            B = preset.B;
        }

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , float ax
            , float ay
            , float bx
            , float by
            , bool relative = true)
            : base(attr, finalValue, duration, relative)
        {
            CheckPositions(ax, bx);
            A = new Vector2(ax, ay);
            B = new Vector2(bx, by);
        }

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , Vector2 A
            , Vector2 B
            , bool relative = true)
            : base(attr, finalValue, duration, relative)
        {
            CheckPositions(A.X, B.X);
            this.A = A;
            this.B = B;
        }

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : this(attr, finalValue, duration, CubicBezierPresets.EaseInOut, form, relative) { }

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , CubicBezierPresets preset
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative)
        {
            A = preset.A;
            B = preset.B;
        }

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , float ax
            , float ay
            , float bx
            , float by
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative)
        {
            CheckPositions(ax, bx);
            A = new Vector2(ax, ay);
            B = new Vector2(bx, by);
        }

        public CubicBezierTransition(
            string attr
            , double finalValue
            , double duration
            , Vector2 A
            , Vector2 B
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative)
        {
            CheckPositions(A, B);
            this.A = A;
            this.B = B;
        }

        private void CheckPositions(Vector2 A, Vector2 B)
        {
            CheckPositions(A.X, B.X);
        }

        private void CheckPositions(double ax, double bx)
        {
            // Any control points outside this range will cause the function
            // to become undefined (because the Bezier curve becomes concave,
            // and is thus no longer a function of the form y = f(x)).
            if (!(0 <= ax && ax <= 1) || !(0 <= bx && bx <= 1))
                throw new ArgumentException(
                    "Invalid CubicBezier points. The x-components of the control points " +
                    "must be in the closed interval [0, 1]. The control points given had " +
                    String.Format("x-components {0} & {1}.", ax, bx)
                    );
        }

        protected override void Initialize()
        {
            base.Initialize();
            coeffs = new double[] {
                1 + 3 * A.X - 3 * B.X,
                3 * (B.X - 2 * A.X),
                3 * A.X,
                1 + 3 * A.Y - 3 * B.Y,
                3 * (B.Y - 2 * A.Y),
                3 * A.Y
            };
        }

        protected override double Function(double time, int frame)
        {
            var t = GetValid(RootSolver.Cubic(coeffs[0], coeffs[1], coeffs[2], -time / duration));
            var y = coeffs[3] * Math.Pow(t, 3.0) + coeffs[4] * Math.Pow(t, 2.0) + coeffs[5] * t;
            return deltaValue * y + initialValue;
        } 

        private double GetValid(double[] roots)
        {
            var new_roots = new List<double>();
            foreach (var r in roots)
            {
                if (r < 0 || r > 1)
                    continue;
                new_roots.Add(r);
            }
            if (new_roots.Count == 0)
                return 1;
            return new_roots.Min();
        }
    }
}
