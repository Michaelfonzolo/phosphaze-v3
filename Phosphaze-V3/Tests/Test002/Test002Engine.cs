using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze_V3.Framework;
using Phosphaze_V3.Framework.Forms;

namespace Phosphaze_V3.Tests.Test002
{
    public class Test002Engine : Engine
    {

        public override void SetupMultiforms()
        {
            multiformManager.RegisterMultiform("Main", new MainMultiform());
            multiformManager.Construct("Main");
        }

    }
}
