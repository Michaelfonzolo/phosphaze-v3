using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace Phosphaze_V3.Framework.Collision
{

    public abstract class Collidable
    {
        public abstract int Precedence { get; }
        public bool CollidingWith(Collidable other) { return Checker.CollisionBetween(this, other); }
        public bool CollidingWith(object other) { return Checker.CollisionBetween(this, other); }
    }

    public class Point : Collidable
    {
        public override int Precedence { get { return 0; } }
        public bool CollidingWith(Point other) { return false; }
    }

    public class Rect : Collidable
    {
        public override int Precedence { get { return 1; } }
        public bool CollidingWith(Point other) { return false; }
        public bool CollidingWith(Rect other) { return false; }
    }

    public static class Checker
    {

        public static bool CollisionBetween(Collidable a, Collidable b)
        {
            try
            {
                if (a.Precedence >= b.Precedence)
                    return DynamicCollisionBetween(a, b);
                return DynamicCollisionBetween(b, a);
            }
            catch (RuntimeBinderException)
            {
                return false;
            }
        }

        public static bool CollisionBetween(Collidable a, object b)
        {
            try
            {
                return DynamicCollisionBetween(a, b);
            }
            catch (RuntimeBinderException)
            {
                return false;
            }
        }

        private static bool DynamicCollisionBetween(dynamic a, dynamic b)
        {
            // This is to prevent infinite recursion in the rare case that
            // CollisionBetween(a, new object()) is called.
            if (b is object)
                return false;
            return a.CollidingWith(b);
        }

    }
}
