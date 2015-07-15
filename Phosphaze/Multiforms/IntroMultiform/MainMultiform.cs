using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework;
using Phosphaze.Framework.Cache;
using Phosphaze.Framework.Collision;
using Phosphaze.Framework.Display;
using Phosphaze.Framework.Events;
using Phosphaze.Framework.Extensions;
using Phosphaze.Framework.Forms;
using Phosphaze.Framework.Forms.Resources;
using Phosphaze.Framework.Forms.Effectors;
using Phosphaze.Framework.Forms.Effectors.Motion;
using Phosphaze.Framework.Forms.Effectors.Transitions;
using Phosphaze.Framework.Input;
using Phosphaze.Framework.Maths;
using Phosphaze.Framework.Maths.Geometry;
using Phosphaze.Framework.Timing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Phosphaze.Multiforms.IntroMultiform
{
    public class MainMultiform : Multiform
    {

        public static string Background = "Background";

        public override void Construct(Framework.ServiceLocator serviceLocator, MultiformData args)
        {
            var background = new TextureForm(
                serviceLocator, "IntroContent/Backgrounds/MainBackground", new Vector2(0.5f, 0.5f));

            background.Scale(0.9);
            background.ToggleVisibility();

            background.AddEffector(new InverseArcsineTransition(TextureForm.ALPHA_ATTR, 0.93, 4000));
            background.AddEffector(new SmoothScrollEffector(0.09, 8000));

            RegisterForm(Background, background);

            SetUpdater(Update);
            SetRenderer(Render);
        }

        private void Update(ServiceLocator serviceLocator)
        {
            if (serviceLocator.Keyboard.IsReleased(Keys.Escape))
                serviceLocator.Engine.Exit();

            if (serviceLocator.Keyboard.IsReleased(Keys.Enter))
                serviceLocator.DisplayManager.NextResolution();

            var bg = (TextureForm)GetForm(Background);
            
            if (At(4000))
                bg.AddEffector(new FunctionalAttributeEffector(
                    TextureForm.ALPHA_ATTR, x => - 0.1 * Math.Pow(Math.Sin(x / 4000), 2.0)));

            var mpos = serviceLocator.Mouse.mousePosAsVec - Resolution.native.center;
            var offset = new Vector2(0.000035f * mpos.X, 0.00002f * mpos.Y)
                * (float)SpecialFunctions.CircularNormalDistribution(
                    1.5 * mpos.X / serviceLocator.DisplayManager.currentResolution.width,
                    1.5 * mpos.Y / serviceLocator.DisplayManager.currentResolution.height);
            offset += VectorUtils.Ones / 2.0f;
            bg.SetPosition(offset);

            UpdateTime(serviceLocator);
            UpdateForms(serviceLocator);
        }

        private void Render(ServiceLocator serviceLocator)
        {
            serviceLocator.DisplayManager.SetSpriteBatchProperties(blendState: BlendState.Additive);
            RenderForm(Background, serviceLocator);
        }

    }
}
