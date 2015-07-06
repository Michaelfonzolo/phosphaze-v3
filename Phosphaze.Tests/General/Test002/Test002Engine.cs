using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phosphaze.Framework;
using Phosphaze.Framework.Forms;

namespace Phosphaze.MultiformTests.Test002
{
    public class Test002Engine : Engine
    {

        public Test002Engine(string contentFolder)
            : base(contentFolder) { }

        public override void SetupMultiforms()
        {
            multiformManager.RegisterMultiform("Main", new MainMultiform());
            multiformManager.Construct("Main");
        }

    }
}
