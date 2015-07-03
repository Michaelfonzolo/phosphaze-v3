using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Phosphaze.Framework;
using Phosphaze.Framework.Forms;

namespace Phosphaze.Framework.Tests.Test002
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
            base.UpdateTime(serviceLocator);
            CheckInput(serviceLocator);
            alpha += 0.0025f;
            if (alpha >= 1f)
            {
                SetUpdater(Update);
            }
        }

        public void Update(ServiceLocator serviceLocator)
        {
            base.UpdateTime(serviceLocator);
            CheckInput(serviceLocator);
            alpha += 0.0025f;
        }

        public void CheckInput(ServiceLocator serviceLocator)
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
            serviceLocator.DisplayManager.Draw(
                texture, new Vector2(0.5f, 0.5f),
                Color.White * alpha, alpha * 2f*(float)Math.PI,
                new Vector2(0.1f * alpha, 0.1f), 1f);
        }

    }
}
