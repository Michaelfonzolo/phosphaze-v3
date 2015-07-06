using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework;

namespace Phosphaze.MultiformTests.Test004
{
    public class Test004Engine : Engine
    {

        public Test004Engine(string contentFolder)
            : base(contentFolder) { }

        public override void SetupMultiforms()
        {
            multiformManager.RegisterMultiform("Main", new MainMultiform());
            multiformManager.Construct("Main");
        }

    }
}
