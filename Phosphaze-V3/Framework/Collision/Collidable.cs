using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace Phosphaze_V3.Framework.Collision
{
    public abstract class Collidable
    {

        private class InternalChecker
        {
            public static CollisionResponse CollisionBetween(Collidable a, Collidable b)
            {
                try
                {
                    if (a.Precedence >= b.Precedence)
                        return DynamicCollisionBetween(a, b);
                    return DynamicCollisionBetween(b, a);
                }
                catch (RuntimeBinderException)
                {
                    return null;
                }
            }

            public static CollisionResponse CollisionBetween(Collidable a, object b)
            {
                try
                {
                    return DynamicCollisionBetween(a, b);
                }
                catch (RuntimeBinderException)
                {
                    return null;
                }
            }

            private static CollisionResponse DynamicCollisionBetween(dynamic a, dynamic b)
            {
                // This is to prevent infinite recursion in the rare case that
                // CollisionBetween(a, new object()) is called.
                if (b is object)
                    return null;
                return a.CollidingWith(b);
            }
        }

        public abstract int Precedence { get; }

        public CollisionResponse CollidingWith(Collidable other) 
        { 
            return InternalChecker.CollisionBetween(this, other); 
        }

        public CollisionResponse CollidingWith(object other) 
        { 
            return InternalChecker.CollisionBetween(this, other); 
        }

    }
}
