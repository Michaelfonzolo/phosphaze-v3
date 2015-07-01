using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze_V3.Framework.Maths.Geometry;
using Microsoft.Xna.Framework;

namespace Phosphaze_V3.Framework.Collision
{
    public class PointCollider : Collidable, IGeometric, ITransformable
    {

        public override int Precedence { get { return CollisionPrecedences.POINT; } }

        public Vector2 Center { get { return new Vector2((float)X, (float)Y); } }

        public double Area { get { return 0; } }

        public double Perimeter { get { return 0; } }

        public double X { get; set; }

        public double Y { get; set; }

        public PointCollider()
            : this(0, 0) { }

        public PointCollider(double c)
            : this(c, c) { }

        public PointCollider(Vector2 vec)
            : this(vec.X, vec.Y) { }

        public PointCollider(Point point)
            : this(point.X, point.Y) { }

        public PointCollider(PointCollider point)
            : this(point.X, point.Y) { }

        public PointCollider(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointCollider(double[] args)
        {
            var l = args.Length;
            switch (l)
            {
                case 0:
                    X = 0;
                    Y = 0;
                    break;
                case 1:
                    X = args[0];
                    Y = args[0];
                    break;
                case 2:
                    X = args[0];
                    Y = args[1];
                    break;
                default:
                    throw new ArgumentException(
                        String.Format(
                            "Invalid argument count {0} for type \"PointCollider\"", l)
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

        /* The Rotation and Scaling transformations are linear maps whose kernel is the origin. That 
         * means if we apply the transformation relative to the origin of the point, nothing happens, 
         * because there is nothing to be transformed other than the point which coincides with it's 
         * own origin.
         */
        
        public void Rotate(double angle, bool degrees = true) { }

        public void Scale(double amount) { }

        public void Rotate(double angle, Vector2 origin, bool degrees = true, bool relative = true)
        {
            SetPosition(VectorUtils.Rotate(Center, angle, origin, degrees, relative));
        }

        public void Scale(double amount, Vector2 origin, bool relative = true)
        {
            SetPosition(VectorUtils.Scale(Center, amount, origin, relative));
        }

        private bool _eq(double x, double y)
        {
            return X == x && Y == y;
        }

        public CollisionResponse CollidingWith(PointCollider other)
        {
            return new CollisionResponse(this, other, _eq(other.X, other.Y));
        }

        public CollisionResponse CollidingWith(Vector2 point)
        {
            return CollidingWith(new PointCollider(point));
        }

        public CollisionResponse CollidingWith(Point point)
        {
            return CollidingWith(new PointCollider(point));
        }

    }
}
