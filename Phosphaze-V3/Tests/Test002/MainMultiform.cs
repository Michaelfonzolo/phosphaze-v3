using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Phosphaze_V3.Framework;
using Phosphaze_V3.Framework.Forms;

namespace Phosphaze_V3.Tests.Test002
{
    public class MainMultiform : Multiform
    {

        float alpha = 0;
        Texture2D texture;

        public override void Construct(ServiceLocator serviceLocator, MultiformData args)
        {
            SetUpdater(TransitionIn);
            SetRenderer(Render);
            texture = serviceLocator.Content.Load<Texture2D>("TestContent/Speaker1");
        }

        public void TransitionIn(ServiceLocator serviceLocator)
        {
            alpha += 0.01f;
            if (alpha >= 1f)
            {
                alpha = 1f;
                SetUpdater(Update);
            }
        }

        public void Update(ServiceLocator serviceLocator)
        {
            if (serviceLocator.Keyboard.IsReleased(Keys.Enter))
                serviceLocator.DisplayManager.NextResolution();
            
            if (serviceLocator.Keyboard.IsReleased(Keys.B))
                serviceLocator.DisplayManager.ToggleBorder();

            if (serviceLocator.Keyboard.IsReleased(Keys.F))
                serviceLocator.DisplayManager.ToggleFullscreen();

            if (serviceLocator.Keyboard.IsReleased(Keys.M))
                serviceLocator.DisplayManager.ToggleMouseVisibility();

            if (serviceLocator.Keyboard.IsReleased(Keys.Escape))
                serviceLocator.Engine.Exit();
        }

        public void Render(ServiceLocator serviceLocator)
        {
            serviceLocator.DisplayManager.Draw(texture, new Vector2(0.5f, 0.5f), Color.White*alpha);
        }

    }
}
