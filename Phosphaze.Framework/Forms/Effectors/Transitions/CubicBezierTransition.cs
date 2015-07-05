using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Phosphaze.Framework.Maths;

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
            var t = RootSolver.Cubic(coeffs[0], coeffs[1], coeffs[2], time / duration)[0];
            var y = coeffs[3] * Math.Pow(t, 3.0) + coeffs[4] * Math.Pow(t, 2.0) + coeffs[5] * t;
            return deltaValue * y + initialValue;
        } 

    }
}
