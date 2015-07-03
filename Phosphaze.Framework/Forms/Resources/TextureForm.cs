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

        public TextureForm(
            ServiceLocator serviceLocator, string textureName, Vector2 position, bool centred = true)
            : base(serviceLocator)
        {
            Position = position;
            Texture = serviceLocator.Content.Load<Texture2D>(textureName);

            Attributes.SetAttr<bool>("Centred", centred);
            Attributes.SetAttr<double>("Alpha", 1.0);
            Attributes.SetAttr<double>("Rotation", 0.0); // Always in degrees
            Attributes.SetAttr<double>("Scale", 1.0);
            Attributes.SetAttr<Color>("Color", Color.White);
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
            if (!degrees)
                angle /= Constants.DEG_TO_RAD;
            Attributes.SetAttr<double>("Rotation", Attributes.GetAttr<double>("Rotation") + angle);
        }

        public void Rotate(double angle, Vector2 origin, bool degrees = true, bool absoluteOrigin = true)
        {
            Position = VectorUtils.Rotate(Position, angle, origin, degrees, absoluteOrigin);
            Rotate(angle, degrees);
        }

        public void Scale(double amount)
        {
            Attributes.SetAttr<double>("Scale", Attributes.GetAttr<double>("Scale") * amount);
        }

        public void Scale(double amount, Vector2 origin, bool absoluteOrigin = true)
        {
            Position = VectorUtils.Scale(Position, amount, origin, absoluteOrigin);
            Scale(amount);
        }

        public override void Render(ServiceLocator serviceLocator)
        {
            serviceLocator.DisplayManager.Draw(
                Texture, Position,
                Attributes.GetAttr<Color>("Color") * (float)Attributes.GetAttr<double>("Alpha"), 
                (float)Attributes.GetAttr<double>("Rotation"),
                (float)Attributes.GetAttr<double>("Scale"), 
                Attributes.GetAttr<bool>("Centred")
                );
        }

    }
}
