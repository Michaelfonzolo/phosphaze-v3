using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze.Framework;

namespace Phosphaze.Framework.Tests.Test001
{
    public class Test001Engine : Engine
    {

        public Test001Engine(string contentFolder)
            : base(contentFolder) { }

        public override void SetupMultiforms()
        {
            multiformManager.RegisterMultiform("MainScreen", new MainScreen());
            multiformManager.Construct("MainScreen");
        }

    }
}
