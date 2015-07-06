using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework;
using Phosphaze.Framework.Forms;
using Phosphaze.Framework.Forms.Resources;
using Phosphaze.Framework.Forms.Effectors;
using Phosphaze.Framework.Forms.Effectors.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Phosphaze.MultiformTests.Test004
{
    public class MainMultiform : Multiform
    {

        public override void Construct(ServiceLocator serviceLocator, MultiformData args)
        {
            TextureForm texture = new TextureForm(serviceLocator, "TestContent/Speaker1", new Vector2(0.5f, 0.5f));
            texture.ToggleVisibility();
            RegisterForm("Speaker", texture);

            SetUpdater(Update);
            SetRenderer(Render);

            // texture.AddEffector(new LinearTransition(TextureForm.ALPHA_ATTR, 1.0, 1000));  -- OK
            // texture.AddEffector(new ArcsineTransition(TextureForm.ALPHA_ATTR, 1.0, 1000, relative: false)); -- OK
            texture.AddEffector(new CubicBezierTransition("Alpha", 1.0, 3000));
        }

        private void CheckInput(ServiceLocator serviceLocator)
        {
            Console.WriteLine(((TextureForm)GetForm("Speaker")).Alpha);
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
            Speaker.Rotate(Math.PI / 2.0 - Math.Atan(LocalTime / 1000.0));
            Speaker.Scale(0.002 * Math.Sin(LocalTime / 1000.0) + 1.0);
        }

        public void Render(ServiceLocator serviceLocator)
        {
            RenderForms(serviceLocator);
        }

    }
}
