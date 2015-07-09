using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework;

namespace Phosphaze
{
    public class PhosphazeEngine : Engine
    {

        public PhosphazeEngine(string contentFolder)
            : base(contentFolder) { }

        public override void SetupMultiforms()
        {
            multiformManager.RegisterMultiform("Intro", new Multiforms.IntroMultiform.MainMultiform());
            multiformManager.Construct("Intro");
        }

    }
}
