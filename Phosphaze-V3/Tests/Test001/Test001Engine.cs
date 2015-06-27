using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze_V3.Framework;

namespace Phosphaze_V3.Tests.Test001
{
    public class Test001Engine : Engine
    {

        public override void SetupMultiforms()
        {
            multiformManager.RegisterMultiform("MainScreen", new MainScreen());
            multiformManager.Construct("MainScreen");
        }

    }
}
