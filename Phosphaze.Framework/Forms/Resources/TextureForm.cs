using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Framework.Cache;
using Phosphaze.Framework.Maths.Geometry;

namespace Phosphaze.Framework.Forms.Resources
{
    public class TextureForm : Form, ITransformable
    {

        public Vector2 Position { get; protected set; }

        public Texture2D Texture { get; private set; }

        public AttributeContainer VisualKernel { get; private set; }

        public TextureForm(
            ServiceLocator serviceLocator, string textureName, Vector2 position, bool centred = true)
            : base(serviceLocator)
        {
            Position = position;
            Texture = serviceLocator.Content.Load<Texture2D>(textureName);

            VisualKernel = new AttributeContainer();
            VisualKernel.SetAttr<bool>("Centred", centred);
            VisualKernel.SetAttr<double>("Alpha", 1.0);
            VisualKernel.SetAttr<double>("Rotation", 0.0); // Always in degrees
            VisualKernel.SetAttr<double>("Scale", 1.0);
            VisualKernel.SetAttr<Color>("Color", Color.White);
        }

        public void SetPosition(double x, double y)
        {
            Position = new Vector2((float)x, (float)y);
        }

        public void SetPosition(Vector2 pos)
        {
            Position = pos;
        }

        public void Translate(double dx, double dy)
        {
            Position += new Vector2((float)dx, (float)dy);
        }

        public void Translate(Vector2 delta)
        {
            Position += delta;
        }

        public void Rotate(double angle, bool degrees = true)
        {
            throw new NotImplementedException();
        }

        public void Rotate(double angle, Vector2 origin, bool degrees = true, bool relative = true)
        {
            throw new NotImplementedException();
        }

        public void Scale(double amount)
        {
            VisualKernel.SetAttr<double>("Scale", VisualKernel.GetAttr<double>("Scale") * amount);
        }

        public void Scale(double amount, Vector2 origin, bool relative = true)
        {
            throw new NotImplementedException();
        }



    }
}
