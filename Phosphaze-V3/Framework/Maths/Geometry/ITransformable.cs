using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Phosphaze_V3.Framework.Maths.Geometry
{
    public interface ITransformable
    {

        void SetPosition(double x, double y);

        void SetPosition(Vector2 pos);

        void Translate(double dx, double dy);

        void Translate(Vector2 delta);

        void Rotate(double angle, bool degrees = true);

        void Rotate(double angle, Vector2 origin, bool degrees = true, bool relative = true);

        void Scale(double amount);

        void Scale(double amount, Vector2 origin, bool relative = true);

    }
}
