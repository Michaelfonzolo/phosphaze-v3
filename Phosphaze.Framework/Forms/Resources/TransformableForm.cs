using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths.Geometry;
using Microsoft.Xna.Framework;

namespace Phosphaze.Framework.Forms.Resources
{
    public abstract class TransformableForm : Form, ITransformable
    {

        public TransformableForm(ServiceLocator serviceLocator)
            : base(serviceLocator) { }

        public abstract void SetPosition(double x, double y);

        public abstract void SetPosition(Vector2 pos);

        public abstract void SetPositionX(double x);

        public abstract void SetPositionY(double y);

        public abstract void Translate(double dx, double dy);

        public abstract void Translate(Vector2 delta);

        public abstract void Rotate(double angle, bool degrees = true);

        public abstract void Rotate(double angle, Vector2 origin, bool degrees = true, bool absoluteOrigin = true);

        public abstract void Scale(double amount);

        public abstract void Scale(double amount, Vector2 origin, bool absoluteOrigin = true);

    }
}
