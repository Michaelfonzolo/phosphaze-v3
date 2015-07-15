using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Phosphaze.Framework.Forms.Resources;
using Phosphaze.Framework.Maths;
using Phosphaze.Framework.Maths.Geometry;

namespace Phosphaze.Framework.Forms.Effectors.Motion
{
    public class SmoothScrollEffector : Effector
    {

        Vector2 scales, periods, projection;

        bool orthogonal;

        bool horizontal;

        public SmoothScrollEffector(
            Vector2 scales, Vector2 periods)
            : this(scales, periods, VectorUtils.Right) { }

        public SmoothScrollEffector(
            Vector2 scales, Vector2 periods, Vector2 projection)
            : base()
        {
            this.scales = scales;
            this.periods = periods;
            this.projection = projection;
            orthogonal = false;
            horizontal = false;
        }

        public SmoothScrollEffector(
            Vector2 scales, Vector2 periods, TransformableForm form)
            : this(scales, periods, VectorUtils.Right, form) { }

        public SmoothScrollEffector(
            Vector2 scales, Vector2 periods, Vector2 projection, TransformableForm form)
            : base(form)
        {
            this.scales = scales;
            this.periods = periods;
            this.projection = projection;
            orthogonal = false;
            horizontal = false;
        }

        public SmoothScrollEffector(
            double scale, double period, bool horizontal = true)
            : base()
        {
            _init(scale, period, horizontal);
        }

        public SmoothScrollEffector(
            double scale, double period, TransformableForm form, bool horizontal = true)
            : base(form)
        {
            _init(scale, period, horizontal);
        }

        private void _init(double scale, double period, bool horizontal)
        {
            if (horizontal)
            {
                this.scales = new Vector2((float)scale, 0);
                this.periods = new Vector2((float)period, 1);
            }
            else
            {
                this.scales = new Vector2(0, (float)scale);
                this.periods = new Vector2(1, (float)period);
            }
            this.projection = VectorUtils.Right;
            this.horizontal = horizontal;
            orthogonal = true;
        }

        public void AttachTo(TransformableForm form)
        {
            base.AttachTo(form);
        }

        public override void Update(ServiceLocator serviceLocator)
        {
            base.Update(serviceLocator);
            var f = form as TransformableForm;
            if (!orthogonal)
            {
                f.Translate(
                    scales.X * Math.Sin(LocalTime / periods.X),
                    scales.Y * Math.Sin(LocalTime / periods.Y));
            }
            else
            {
                if (horizontal)
                    f.Translate(scales.X * Math.Sin(LocalTime / periods.X), 0);
                else
                    f.Translate(0, scales.Y * Math.Sin(LocalTime / periods.Y));
            }
        }

    }
}
