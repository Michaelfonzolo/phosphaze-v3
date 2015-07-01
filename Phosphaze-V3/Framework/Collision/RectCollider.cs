using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Phosphaze_V3.Framework.Cache;
using Phosphaze_V3.Framework.Maths.Geometry;

namespace Phosphaze_V3.Framework.Collision
{
    public class RectCollider : Collidable, IGeometric, ITransformable
    {

        public override int Precedence { get { return CollisionPrecedences.RECT; } }

        private PropertyCache<Vector2> centerCache = new PropertyCache<Vector2>();
        private PropertyCache<Vector2[]> coordsCache = new PropertyCache<Vector2[]>();

        public Vector2 Center
        {
            get
            {
                if (centerCache.dirty)
                    centerCache.Clean = new Vector2((float)(X + W / 2.0), (float)(Y + H / 2.0));
                return centerCache.Value;
            }
        }

        public Vector2[] Coords
        {
            get
            {
                if (coordsCache.dirty)
                    coordsCache.Clean = new Vector2[]
                    {
                        new Vector2((float)X, (float)Y),
                        new Vector2((float)(X + W), (float)Y),
                        new Vector2((float)(X + W), (float)(Y + H)),
                        new Vector2((float)X, (float)(Y + H))
                    };
                return coordsCache.Value;
            }
        }

        public Vector2 TopLeft { get { return Coords[0]; } }

        public Vector2 TopRight { get { return Coords[1]; } }

        public Vector2 BottomRight { get { return Coords[2]; } }

        public Vector2 BottomLeft { get { return Coords[3]; } }

        public double Area { get { return W * H; } }

        public double Perimeter { get { return 2 * (W + H); } }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double W { get; private set; }

        public double H { get; private set; }

        public RectCollider()
            : this(0, 0, 0, 0) { }

        public RectCollider(double l)
            : this(0, 0, l, l) { }

        public RectCollider(double w, double h)
            : this(0, 0, w, h) { }

        public RectCollider(double x, double y, double l)
            : this(x, y, l, l) { }

        public RectCollider(double x, double y, double w, double h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public RectCollider(Vector2 position, double w, double h)
            : this(position.X, position.Y, w, h) { }

        public RectCollider(RectCollider rect)
            : this(rect.X, rect.Y, rect.W, rect.H) { }

        public RectCollider(Rectangle rect)
            : this(rect.X, rect.Y, rect.Width, rect.Height) { }

        public RectCollider(double[] args)
        {
            var l = args.Length;
            switch (l)
            {
                case 0:
                    X = 0;
                    Y = 0;
                    W = 0;
                    H = 0;
                    break;
                case 1:
                    X = 0;
                    Y = 0;
                    W = args[0];
                    H = args[0];
                    break;
                case 2:
                    X = 0;
                    Y = 0;
                    W = args[0];
                    H = args[1];
                    break;
                case 3:
                    X = args[0];
                    Y = args[1];
                    W = args[2];
                    H = args[2];
                    break;
                case 4:
                    X = args[0];
                    Y = args[1];
                    W = args[2];
                    H = args[3];
                    break;
                default:
                    throw new ArgumentException(
                        String.Format(
                            "Invalid argument count {0} for type \"RectCollider\"", l)
                            );
            }
        }

        public void SetPosition(double x, double y)
        {
            X = x;
            Y = y;
            centerCache.Value = new Vector2((float)(x + W / 2.0), (float)(y + H / 2.0));
            coordsCache.dirty = true;
        }

        public void SetPosition(Vector2 pos)
        {
            X = pos.X;
            Y = pos.Y;
            centerCache.Value = new Vector2((float)(X + W / 2.0), (float)(Y + H / 2.0));
            coordsCache.dirty = true;
        }

        public void Translate(double dx, double dy)
        {
            X += dx;
            Y += dy;
            var c = centerCache.Value;
            centerCache.Value += new Vector2((float)dx, (float)dy);
            coordsCache.dirty = true;
        }

        public void Translate(Vector2 delta)
        {
            X += delta.X;
            Y += delta.Y;
            centerCache.Value += delta;
            coordsCache.dirty = true;
        }

        public void Rotate(double angle, bool degrees = true)
        {
            throw new NotImplementedException("Cannot rotate RectCollider.");
        }

        public void Rotate(double angle, Vector2 origin, bool degrees = true, bool relative = true)
        {
            throw new NotImplementedException("Cannot rotate RectCollider.");
        }

        public void Scale(double amount)
        {
            var pW = W;
            var pH = H;
            W *= amount;
            Y *= amount;
            X -= (W - pW) / 2.0;
            Y -= (H - pH) / 2.0;
        }

        public void Scale(double amount, Vector2 origin, bool relative = true)
        {
            if (relative)
                origin += TopLeft;
            double dx = origin.X - X;
            double dy = origin.Y - Y;
            double alpha = 1 - amount;
            X -= alpha * dx;
            Y -= alpha * dy;
            W *= amount;
            H *= amount;
        }

        public CollisionResponse CollidingWith(PointCollider point)
        {
            bool inside = X <= point.X && point.X <= X + W &&
                          Y <= point.Y && point.Y <= Y + H;
            return new CollisionResponse(this, point, inside);
        }

        public CollisionResponse CollidingWith(SegmentCollider segment)
        {
            var intersections = new List<Vector2>();
            var corners = Coords;
            Vector2 p1, p2;
            Vector2? poi;
            for (int i = 0; i < 4; i++)
            {
                p1 = corners[(i - 1) % 4];
                p2 = corners[i];
                poi = LinearUtils.SegmentIntersectionPoint(
                    p1.X, p1.Y, p2.X, p2.Y, 
                    segment.Start.X, segment.Start.Y, 
                    segment.End.X, segment.End.Y
                    );
                if (poi.HasValue)
                    intersections.Add(poi.Value);
            }
            var c_res = new CollisionResponse(this, segment, intersections.Count > 0);
            if (!c_res.Colliding)
                return c_res;
            c_res.SetAttr<Vector2[]>("Intersections", intersections.ToArray());
            return c_res;
        }

        public CollisionResponse CollidingWith(RectCollider rect)
        {
            return new CollisionResponse(this, rect, RectUtils.Collision(
                X, Y, W, H, rect.X, rect.Y, rect.W, rect.H));
        }

    }
}
