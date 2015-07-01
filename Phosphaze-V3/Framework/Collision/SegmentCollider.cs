using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze_V3.Framework.Cache;
using Phosphaze_V3.Framework.Maths.Geometry;
using Microsoft.Xna.Framework;

namespace Phosphaze_V3.Framework.Collision
{
    public class SegmentCollider : Collidable, IGeometric, ITransformable
    {

        public override int Precedence { get { return CollisionPrecedences.SEGMENT; } }

        public Vector2 Center { get { return (Start + End) / 2.0f; } }

        public double Area { get { return 0; } }

        public double Perimeter { get { return Length; } }

        private PropertyCache<double> lengthCache = new PropertyCache<double>();

        public double Length 
        { 
            get 
            { 
                if (lengthCache.dirty)
                    lengthCache.Clean = Vector2.Distance(Start, End); 
                return lengthCache.Value;
            } 
        }

        public double Magnitude { get { return Length; } }

        public double LengthSquared { get { return Math.Pow(Length, 2d); } }

        public double MagnitudeSquared { get { return LengthSquared; } }

        public Vector2 Start { get; private set; }

        public Vector2 End { get; private set; }

        public SegmentCollider()
            : this(0, 0, 0, 0) { }

        public SegmentCollider(double end_x, double end_y)
            : this(0, 0, end_x, end_y) { }

        public SegmentCollider(double start_x, double start_y, double end_x, double end_y)
        {
            Start = new Vector2((float)start_x, (float)start_y);
            End = new Vector2((float)end_x, (float)end_y);
        }

        public SegmentCollider(Vector2 end)
        {
            Start = Vector2.Zero;
            End = end;
        }

        public SegmentCollider(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public SegmentCollider(SegmentCollider other)
        {
            Start = other.Start;
            End = other.End;
        }

        public SegmentCollider(double[] args)
        {
            var l = args.Length;
            float x1, y1, x2, y2;
            switch (l)
            {
                case 0:
                    x1 = 0;
                    y1 = 0;
                    x2 = 0;
                    y2 = 0;
                    break;
                case 2:
                    x1 = 0;
                    y1 = 0;
                    x2 = (float)args[0];
                    y2 = (float)args[1];
                    break;
                case 4:
                    x1 = (float)args[0];
                    y1 = (float)args[1];
                    x2 = (float)args[2];
                    y2 = (float)args[3];
                    break;
                default:
                    throw new ArgumentException(
                        String.Format(
                            "Invalid argument count {0} for type \"SegmentCollider\"", l)
                            );
            }
            Start = new Vector2(x1, y1);
            End = new Vector2(x2, y2);
        }

        public void SetStartPosition(double x, double y)
        {
            Start = new Vector2((float)x, (float)y);
            lengthCache.dirty = true;
        }

        public void SetStartPosition(Vector2 vec)
        {
            Start = vec;
            lengthCache.dirty = true;
        }

        public void SetEndPosition(double x, double y)
        {
            End = new Vector2((float)x, (float)y);
            lengthCache.dirty = true;
        }

        public void SetEndPosition(Vector2 vec)
        {
            End = vec;
            lengthCache.dirty = true;
        }

        public void SetPosition(double x, double y)
        {
            SetPosition(new Vector2((float)x, (float)y));
        }

        public void SetPosition(Vector2 pos)
        {
            // SetPosition defaults to moving an object relative to it's origin,
            // and for a Segment the origin is the center.
            var delta = pos - Center;
            Start += delta;
            End += delta;
        }

        public void TranslateStart(double dx, double dy)
        {
            Start += new Vector2((float)dx, (float)dy);
            lengthCache.dirty = true;
        }

        public void TranslateStart(Vector2 delta)
        {
            Start += delta;
            lengthCache.dirty = true;
        }

        public void TranslateEnd(double dx, double dy)
        {
            End += new Vector2((float)dx, (float)dy);
            lengthCache.dirty = true;
        }

        public void TranslateEnd(Vector2 delta)
        {
            End += delta;
            lengthCache.dirty = true;
        }

        public void Translate(double dx, double dy)
        {
            var delta = new Vector2((float)dx, (float)dy);
            Start += delta;
            End += delta;
        }

        public void Translate(Vector2 delta)
        {
            Start += delta;
            End += delta;
        }

        public void Rotate(double angle, bool degrees = true)
        {
            Rotate(angle, Center, degrees, false);
        }

        public void Rotate(double angle, Vector2 origin, bool degrees = true, bool relative = true)
        {
            Start = VectorUtils.Rotate(Start, angle, origin, degrees, relative);
            End = VectorUtils.Rotate(End, angle, origin, degrees, relative);
        }

        public void Scale(double amount)
        {
            var c = Center;
            Start = VectorUtils.Scale(Start, amount, c, false);
            End = VectorUtils.Scale(End, amount, c, false);
        }

        public void Scale(double amount, Vector2 origin, bool relative = true)
        {
            Start = VectorUtils.Scale(Start, amount, origin, relative);
            End = VectorUtils.Scale(End, amount, origin, relative);
            lengthCache.dirty = true;
        }

        public CollisionResponse CollidingWith(PointCollider point)
        {
            var result = LinearUtils.PointOnLine(point.X, point.Y, Start.X, Start.Y, End.X, End.Y);
            return new CollisionResponse(this, point, result);
        }

        public CollisionResponse CollidingWith(SegmentCollider segment)
        {
            var poi = LinearUtils.SegmentIntersectionPoint(
                Start.X, Start.Y, End.X, End.Y, segment.Start.X, 
                segment.Start.Y, segment.End.X, segment.End.Y
                );
            var c_res = new CollisionResponse(this, segment, poi.HasValue);
            if (!c_res.Colliding)
                return c_res;
            c_res.SetAttr<Vector2>("IntersectionPoint", poi.Value);
            return c_res;
        }

    }
}
