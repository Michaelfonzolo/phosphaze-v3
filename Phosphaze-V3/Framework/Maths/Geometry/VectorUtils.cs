using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Phosphaze_V3.Framework.Maths.Geometry
{
    public static class VectorUtils
    {

        public static Vector2 Polar(double angle, bool degrees = true)
        {
            if (degrees)
                angle *= Constants.DEG_TO_RAD;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static Vector2 Polar(double angle, Vector2 origin, bool degrees = true)
        {
            if (degrees)
                angle *= Constants.DEG_TO_RAD;
            return new Vector2((float)Math.Cos(angle) + origin.X, (float)Math.Sin(angle) + origin.Y);
        }

        public static Vector2 Rotate(
            Vector2 vec, double angle, Vector2 origin, 
            bool degrees = true, bool relative = true)
        {
            if (degrees)
                angle *= Constants.DEG_TO_RAD;

            double x, y, nx, ny;

            if (relative)
            {
                x = -origin.X;
                y = -origin.Y;
            }
            else
            {
                x = vec.X - origin.X;
                y = vec.Y - origin.Y;
            }
            double cos_theta = Math.Cos(angle);
            double sin_theta = Math.Sin(angle);

            nx = x * cos_theta - y * sin_theta;
            ny = y * sin_theta + x * cos_theta;

            if (relative)
            {
                nx += vec.X;
                ny += vec.Y;
            }
            else
            {
                nx += origin.X;
                ny += origin.Y;
            }
            return new Vector2((float)(nx), (float)(ny));
        }

        public static Vector2 ToNormalized(Vector2 vec)
        {
            return vec / vec.Length();
        }

        public static Vector2 Scale(
            Vector2 vec, double amount, Vector2 origin, bool relative = true)
        {
            if (relative)
                return vec + origin * (1f - (float)amount);
            return (vec - origin) * (float)amount + origin;
        }

    }
}
