using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Phosphaze_V3.Framework.Maths.Geometry
{
    public static class LinearUtils
    {

        public static bool IsPointInSegmentRange(
            double px, double py,
            double x1, double y1,
            double x2, double y2)
        {
            if (x1 == x2)
                return (y1 <= py) == (py <= y2);
            return (x1 <= px) == (px <= x2);
        }

        public static Vector2? LineIntersectionPoint(
            double x1, double y1, double x2, double y2,
            double x3, double y3, double x4, double y4)
        {
            if (((x1 == x2) && (x3 == x4)) || ((y1 == y2) && (y3 == y4)))
                return null;
            else if (x1 == x2)
            {
                double m1 = (y4 - y3) / (x4 - x3);
                double b1 = y3 - m1 * x3;
                double py = m1 * x1 + b1;
                return new Vector2((float)x1, (float)py);
            }
            else if (x3 == x4)
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double b1 = y1 - m1 * x1;
                double py = m1 * x3 + b1;
                return new Vector2((float)x3, (float)py);
            }
            else
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double m2 = (y4 - y3) / (x4 - x3);

                if (m1 == m2)
                    return null;

                double b1 = y1 - m1 * x1;
                double b2 = y3 - m2 * x3;

                double px = (b2 - b1) / (m1 - m2);
                double py = m1 * px + b1;

                return new Vector2((float)px, (float)py);
            }
        }

        public static Vector2? LineToSegmentIntersectionPoint(
            double x1, double y1, double x2, double y2,
            double x3, double y3, double x4, double y4)
        {
            var p = LineIntersectionPoint(x1, y1, x2, y2, x3, y3, x4, y4);
            if (p.HasValue && IsPointInSegmentRange(p.Value.X, p.Value.Y, x3, y3, x4, y4))
                return p;
            return null;
        }

        public static Vector2? SegmentIntersectionPoint(
            double x1, double y1, double x2, double y2,
            double x3, double y3, double x4, double y4)
        {
            var p = LineIntersectionPoint(x1, y1, x2, y2, x3, y3, x4, y4);
            if (!p.HasValue)
                return null;
            var v = p.Value;
            if (IsPointInSegmentRange(v.X, v.Y, x1, y1, x2, y2) &&
                IsPointInSegmentRange(v.X, v.Y, x3, y3, x4, y4))
                return p;
            return null;
        }

        public static bool PointOnLine(
            double px, double py,
            double x1, double y1, double x2, double y2)
        {
            if (x1 == x2)
                return px == x1 && ((y1 <= py) == (py <= y2));
            else if (y1 == y2)
                return py == y1 && ((x1 <= px) == (px <= y1));
            double m = (y2 - y1) / (x2 - x1);
            double b = y1 - m * x1;
            return py == m * px + b;
        }

        public static bool PointOnSegment(
            double px, double py,
            double x1, double y1, double x2, double y2)
        {
            return PointOnLine(px, py, x1, y1, x2, y2) && IsPointInSegmentRange(px, py, x1, y1, x2, y2);
        }

    }
}
