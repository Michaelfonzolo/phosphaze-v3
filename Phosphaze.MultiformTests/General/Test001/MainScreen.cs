using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze.Framework;
using Phosphaze.Framework.Forms;
using Microsoft.Xna.Framework.Input;

namespace Phosphaze.MultiformTests.Test001
{
    public class MainScreen : Multiform
    {

        public override void Construct(Framework.ServiceLocator serviceLocator, MultiformData args)
        {
            SetUpdater(Update);
            SetRenderer(Render);
        }

        public void Update(ServiceLocator serviceLocator)
        {
            if (serviceLocator.Keyboard.IsReleased(Keys.Escape))
                serviceLocator.Engine.Exit();
        }

        public void Render(ServiceLocator serviceLocator)
        {

        }

    }
}
