using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze_V3.Framework.Collision
{
    public static class CollisionPrecedences
    {

        internal const int POINT = 0;
        internal const int SEGMENT = 1;
        internal const int RECT = 2;
        internal const int ELLIPSE = 3;
        /*
        internal const int CONVEX_POLYGON = 4;
        internal const int POLYGON = 5;
         */

        public static readonly int MAX = ELLIPSE;

    }
}
