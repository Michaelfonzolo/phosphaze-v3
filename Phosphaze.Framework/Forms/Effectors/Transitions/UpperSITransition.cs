﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phosphaze.Framework.Maths;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public class UpperSITransition : AbstractTransition
    {

        private double alpha, beta;

        public UpperSITransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        public UpperSITransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = deltaValue / 1.6339648461028329;
            beta = 15.707963 / duration;
        }

        protected override double Function(double time, int frame)
        {
            return alpha * SpecialFunctions.Si(time * beta) + initialValue;
        }

    }
}