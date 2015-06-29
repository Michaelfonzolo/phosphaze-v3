using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phosphaze_V3.Framework.Forms.Resources
{
    public abstract class MouseCollidableForm : Form
    {

        public bool mouseEntered { get; private set; }

        public MouseCollidableForm(ServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            mouseEntered = false;
        }

        public override void Update(ServiceLocator serviceLocator)
        {
            base.Update(serviceLocator);
        }

        public virtual void OnMouseEnter(ServiceLocator serviceLocator) { }

        public virtual void OnMouseCollide(ServiceLocator serviceLocator) { }

        public virtual void OnMouseLeave(ServiceLocator serviceLocator) { }

    }
}
