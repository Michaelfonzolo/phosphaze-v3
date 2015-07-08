using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework;
using Phosphaze.Framework.Forms;

namespace Phosphaze.MultiformTests.Test003
{
    public class Test003Engine : Engine
    {

        public Test003Engine(string contentFolder)
            : base(contentFolder) { }

        public override void SetupMultiforms()
        {
            multiformManager.RegisterMultiform("Main", new MainMultiform());
            multiformManager.Construct("Main");
        }

    }
}
