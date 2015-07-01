using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Phosphaze_V3.Framework.Maths.Geometry
{
    public interface IGeometric
    {

        Vector2 Center { get; }

        double Perimeter { get; }

        double Area { get; }

    }
}
