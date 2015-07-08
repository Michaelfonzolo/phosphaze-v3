using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework;
using Phosphaze.Framework.Forms;
using Phosphaze.Framework.Forms.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Phosphaze.MultiformTests.Test003
{
    public class MainMultiform : Multiform
    {

        public override void Construct(ServiceLocator serviceLocator, MultiformData args)
        {
            TextureForm texture = new TextureForm(serviceLocator, "TestContent/Speaker1", new Vector2(0.5f, 0.5f));
            RegisterForm("Speaker", texture);

            SetUpdater(Update);
            SetRenderer(Render);
        }

        private void CheckInput(ServiceLocator serviceLocator)
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

        public void Update(ServiceLocator serviceLocator)
        {
            UpdateTime(serviceLocator);

            CheckInput(serviceLocator);
            UpdateForms(serviceLocator);

            var Speaker = (TextureForm)GetForm("Speaker");
            Speaker.Rotate(Math.PI/2.0 - Math.Atan(LocalTime/1000.0));
            Speaker.Scale(0.005*Math.Sin(LocalTime / 1000.0) + 1.0);
        }

        public void Render(ServiceLocator serviceLocator)
        {
            RenderForms(serviceLocator);
        }

    }
}
