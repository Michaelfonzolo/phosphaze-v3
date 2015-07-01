using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Phosphaze_V3.Framework.Maths.Geometry;
using Phosphaze_V3.Framework.Cache;

namespace Phosphaze_V3.Framework.Collision
{
    public class EllipseCollider : Collidable, IGeometric, ITransformable
    {

        public override int Precedence { get { return CollisionPrecedences.ELLIPSE; } }

        public Vector2 Center { get { return new Vector2((float)X, (float)Y); } }

        public double Area { get { return Math.PI * A * B; } }

        private PropertyCache<double> perimeterCache = new PropertyCache<double>();

        public double Perimeter
        {
            get
            {
                if (perimeterCache.dirty)
                {
                    var h = 3 * Math.Pow((A - B) / (A + B), 2.0);
                    perimeterCache.Clean = Math.PI * (A + B) * (1 + h / (10 + Math.Sqrt(4 - h)));
                }
                return perimeterCache.Value;
            }
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double A { get; private set; }

        public double B { get; private set; }

        public EllipseCollider()
            : this(0, 0, 0, 0) { }

        public EllipseCollider(double r)
            : this(0, 0, r, r) { }

        public EllipseCollider(double a, double b)
            : this(0, 0, a, b) { }

        public EllipseCollider(double x, double y, double r)
            : this(x, y, r, r) { }

        public EllipseCollider(double x, double y, double a, double b)
        {
            X = x;
            Y = y;
            A = a;
            B = b;
        }

        public EllipseCollider(Vector2 center, double r)
            : this(center.X, center.Y, r, r) { }

        public EllipseCollider(Vector2 center, double a, double b)
            : this(center.X, center.Y, a, b) { }

        public EllipseCollider(Rectangle rect)
            : this(rect.Center.X, rect.Center.Y, rect.Width / 2.0, rect.Height / 2.0) { }

        public EllipseCollider(double[] args)
        {
            var l = args.Length;
            switch (l)
            {
                case 0:
                    X = 0;
                    Y = 0;
                    A = 0;
                    B = 0;
                    break;
                case 1:
                    X = 0;
                    Y = 0;
                    A = args[0];
                    B = args[0];
                    break;
                case 2:
                    X = 0;
                    Y = 0;
                    A = args[0];
                    B = args[1];
                    break;
                case 3:
                    X = args[0];
                    Y = args[1];
                    A = args[2];
                    B = args[2];
                    break;
                case 4:
                    X = args[0];
                    Y = args[1];
                    A = args[2];
                    B = args[3];
                    break;
                default:
                    throw new ArgumentException(
                        String.Format(
                            "Invalid argument count {0} for type \"EllipseCollider\"", l)
                            );
            }
        }

        public void SetPosition(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void SetPosition(Vector2 pos)
        {
            X = pos.X;
            Y = pos.Y;
        }

        public void Translate(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }

        public void Translate(Vector2 delta)
        {
            X += delta.X;
            Y += delta.Y;
        }

        public void Rotate(double angle, bool degrees = true)
        {
            if (A != B)
                throw new ArgumentException("Cannot rotate non-circular EllipseCollider.");
            // Literally do nothing, a circle rotated about its center is the same.
        }

        public void Rotate(double angle, Vector2 origin, bool degrees = true, bool relative = true)
        {
            if (A != B)
                throw new ArgumentException("Cannot rotate non-circular EllipseCollider.");
            SetPosition(VectorUtils.Rotate(Center, angle, origin, degrees, relative));
        }

        public void Scale(double amount)
        {
            A *= amount;
            B *= amount;
        }

        public void Scale(double amount, Vector2 origin, bool relative = true)
        {
            // Think about this. Use some calculus. I have an odd feeling that
            // scaling an ellipse off center results in a dumb shape like a
            // multifocal ellipse.
        }

        public CollisionResponse CollidingWith(PointCollider point)
        {
            return new CollisionResponse(this, point, 
                Math.Pow((point.X - X) / A, 2.0) + Math.Pow((point.Y - Y) / B, 2.0) <= 1);
        }

        public CollisionResponse CollidingWith(SegmentCollider segment)
        {
            var c_res = new CollisionResponse(this, segment, false);
            double X1 = segment.Start.X, 
                   X2 = segment.End.X, 
                   Y1 = segment.Start.Y, 
                   Y2 = segment.End.Y;
            var intersections = EllipseUtils.EllipseLineIntersections(X, Y, A, B, X1, Y1, X2, Y2);
            if (intersections.Length == 0)
                return c_res;

            var valid = new List<Vector2>();
            foreach (var poi in intersections)
            {
                if (LinearUtils.IsPointInSegmentRange(
                        poi.X, poi.Y, X1, Y1, X2, Y2))
                    valid.Add(poi);
            }

            var count = valid.Count;
            if (count == 0)
                return c_res;

            c_res.SetAttr<int>("NumIntersections", count);
            c_res.SetAttr<Vector2[]>("IntersectionPoints", valid.ToArray());

            return c_res;
        }

        public CollisionResponse CollidingWith(RectCollider rect)
        {
            bool result = false;

            // Check if this works first.
            /*
            if (A == B)
            {
                double rx = rect.X + rect.W / 2, ry = rect.Y + rect.H / 2;
                double x_offset = Math.Abs(X - rx);
                double y_offset = Math.Abs(Y - ry);
                double half_width = rect.W / 2;
                double half_height = rect.H / 2;

                if (x_offset > (half_width + A))
                    result = false;
                else if (y_offset > (half_height + A))
                    result = false;
                else 
                {
                    if (x_offset <= half_width)
                        result = true;
                    else if (y_offset <= half_height)
                        result = true;
                    else
                    {
                        double deltax = x_offset - half_width;
                        double deltay = y_offset - half_height;
                        double dist = Math.Pow(deltax, 2) + Math.Pow(deltay, 2);
                        result = dist <= A * A ? true : false;
                    }
                }
            }
            else
            {
             */
                // For the math, see:
                //   http://www.geometrictools.com/Documentation/IntersectionRectangleEllipse.pdf

                // NOTE: Look into using the minkowski sum algorithm suggested there instead,
                // it looks substantially faster.
                var c = Center;
                var corners = rect.Coords;
                for (int i = 0; i < 4; i++)
                {
                    if (EllipseUtils.EllipseOverlapSegment(c, A, B, corners[(i - 1) % 4], corners[i]))
                    {
                        result = true;
                        break;
                    }
                }
                result = rect.X <= X && X <= rect.X + rect.W &&
                         rect.Y <= Y && Y <= rect.Y + rect.H;
            /*}*/
            return new CollisionResponse(this, rect, result);
        }

        public CollisionResponse CollidingWith(EllipseCollider ellipse)
        {
            if (A == B && ellipse.A == ellipse.B)
                return new CollisionResponse(this, ellipse,
                    Vector2.Distance(Center, ellipse.Center) <= A + ellipse.A
                    );
            throw new NotImplementedException(
                "No algorithm for ellipse-to-ellipse collision detection exists yet, sorry!");
        }

    }
}
