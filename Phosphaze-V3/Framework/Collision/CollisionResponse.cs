using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze_V3.Framework.Cache;

namespace Phosphaze_V3.Framework.Collision
{
    public class CollisionResponse : AttributeContainer
    {

        public Collidable ColliderA { get; private set; }

        public Collidable ColliderB { get; private set; }

        public bool Colliding { get; set; }

        public CollisionResponse(Collidable a, Collidable b, bool colliding)
        {
            ColliderA = a;
            ColliderB = b;
            Colliding = colliding;
        }

    }
}
