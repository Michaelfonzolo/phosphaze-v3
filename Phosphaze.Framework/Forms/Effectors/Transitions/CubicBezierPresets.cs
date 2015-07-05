using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public sealed class CubicBezierPresets
    {
        public static CubicBezierPresets Linear 
            = new CubicBezierPresets(0f, 0f, 1f, 1f);

        public static CubicBezierPresets Equivariant 
            = new CubicBezierPresets(1f/3f, 1f, 2f/3f, 0f);

        public static CubicBezierPresets EaseIn 
            = new CubicBezierPresets(0.4f, 0f, 1f, 1f);

        public static CubicBezierPresets EaseOut 
            = new CubicBezierPresets(0.1f, 0.2f, 0.52f, 1f); // These values just feel right.

        public static CubicBezierPresets EaseInOut
            = new CubicBezierPresets(0.5f, 0f, 0.5f, 1f);

        public static CubicBezierPresets Canyon
            = new CubicBezierPresets(1f, 1f, 0f, 1f);

        public static CubicBezierPresets QuasiSinusoidal
            = new CubicBezierPresets(0.5f, -0.25f, 0.5f, 1.25f);

        public Vector2 A { get; private set; }

        public Vector2 B { get; private set; }
        
        private CubicBezierPresets(float ax, float ay, float bx, float by)
        {
            A = new Vector2(ax, ay);
            B = new Vector2(bx, by);
        }
    }
}
