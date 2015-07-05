﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phosphaze.Framework.Forms.Effectors.Transitions
{
    public abstract class AbstractPowerSineTransition : AbstractTransition
    {

        protected virtual double Power { get; set; }

        private double alpha;

        public AbstractPowerSineTransition(
            string attr
            , double finalValue
            , double duration
            , bool relative = true)
            : base(attr, finalValue, duration, relative) { }

        public AbstractPowerSineTransition(
            string attr
            , double finalValue
            , double duration
            , Form form
            , bool relative = true)
            : base(attr, finalValue, duration, form, relative) { }

        protected override void Initialize()
        {
            base.Initialize();
            alpha = Math.PI / (2.0 * duration);
        }

        protected override double Function(double time, int frame)
        {
            return deltaValue * Math.Sin(alpha * time) + initialValue;
        }

    }
}
