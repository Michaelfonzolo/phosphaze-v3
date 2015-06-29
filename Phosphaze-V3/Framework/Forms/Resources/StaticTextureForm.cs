using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Phosphaze_V3.Framework.Forms.Resources
{
    public class StaticTextureForm : MouseCollidableForm
    {

        public readonly string textureName;

        public Texture2D texture { get; private set; }

        public StaticTextureForm(string textureName, ServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            texture = serviceLocator.Content.Load<Texture2D>(textureName);
            this.textureName = textureName;
        }

        public override void Render(ServiceLocator serviceLocator)
        {
            
        }

    }
}
