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
    public class TextureForm : TransformableForm
    {

        public const string CENTRED_ATTR = "CENTRED";

        public const string ALPHA_ATTR = "Alpha";

        public const string ROTATION_ATTR = "Rotation";

        public const string SCALE_ATTR = "Scale";

        public const string COLOR_ATTR = "Color";

        public Vector2 Position { get; protected set; }

        public Texture2D Texture { get; private set; }

        public double Alpha 
        { 
            get 
            { 
                return Attributes.GetAttr<double>(ALPHA_ATTR); 
            } 
            set 
            { 
                Attributes.SetAttr<double>(ALPHA_ATTR, value); 
            } 
        }

        public Color Color
        {
            get
            {
                return Attributes.GetAttr<Color>(COLOR_ATTR);
            }
            set
            {
                Attributes.SetAttr<Color>(COLOR_ATTR, value);
            }
        }

        public TextureForm(
            ServiceLocator serviceLocator, string textureName, Vector2 position, bool centred = true)
            : base(serviceLocator)
        {
            Position = position;
            Texture = serviceLocator.Content.Load<Texture2D>(textureName);

            Attributes.SetAttr<bool>(CENTRED_ATTR, centred);
            Attributes.SetAttr<double>(ALPHA_ATTR, 1.0);
            Attributes.SetAttr<double>(ROTATION_ATTR, 0.0); // Always in degrees
            Attributes.SetAttr<double>(SCALE_ATTR, 1.0);
            Attributes.SetAttr<Color>(COLOR_ATTR, Color.White);
        }

        public void ToggleVisibility() 
        { 
            Attributes.SetAttr<double>(ALPHA_ATTR, 1.0 - Attributes.GetAttr<double>(ALPHA_ATTR)); 
        }

        public override void SetPosition(double x, double y)
        {
            Position = new Vector2((float)x, (float)y);
        }

        public override void SetPosition(Vector2 pos)
        {
            Position = pos;
        }

        public override void SetPositionX(double x)
        {
            Position = new Vector2((float)x, Position.Y);
        }

        public override void SetPositionY(double y)
        {
            Position = new Vector2(Position.X, (float)y);
        }

        public override void Translate(double dx, double dy)
        {
            Position += new Vector2((float)dx, (float)dy);
        }

        public override void Translate(Vector2 delta)
        {
            Position += delta;
        }

        public override void Rotate(double angle, bool degrees = true)
        {
            if (!degrees)
                angle /= Constants.DEG_TO_RAD;
            Attributes.SetAttr<double>(ROTATION_ATTR, Attributes.GetAttr<double>(ROTATION_ATTR) + angle);
        }

        public override void Rotate(double angle, Vector2 origin, bool degrees = true, bool absoluteOrigin = true)
        {
            Position = VectorUtils.Rotate(Position, angle, origin, degrees, absoluteOrigin);
            Rotate(angle, degrees);
        }

        public override void Scale(double amount)
        {
            Attributes.SetAttr<double>(SCALE_ATTR, Attributes.GetAttr<double>(SCALE_ATTR) * amount);
        }

        public override void Scale(double amount, Vector2 origin, bool absoluteOrigin = true)
        {
            Position = VectorUtils.Scale(Position, amount, origin, absoluteOrigin);
            Scale(amount);
        }

        public override void Render(ServiceLocator serviceLocator)
        {
            serviceLocator.DisplayManager.Draw(
                Texture, Position,
                Attributes.GetAttr<Color>(COLOR_ATTR) * (float)Attributes.GetAttr<double>(ALPHA_ATTR), 
                (float)Attributes.GetAttr<double>(ROTATION_ATTR),
                (float)Attributes.GetAttr<double>(SCALE_ATTR), 
                Attributes.GetAttr<bool>(CENTRED_ATTR)
                );
        }

    }
}
