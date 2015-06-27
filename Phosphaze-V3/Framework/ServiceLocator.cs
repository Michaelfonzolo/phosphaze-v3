using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Phosphaze_V3.Framework.Display;
using Phosphaze_V3.Framework.Timing;
using Phosphaze_V3.Framework.Events;
using Phosphaze_V3.Framework.Forms;
using Phosphaze_V3.Framework.Input;

namespace Phosphaze_V3.Framework
{
    public class ServiceLocator
    {

        public Engine Engine { get; private set; }
        public ContentManager Content { get; private set; }
        public DisplayManager DisplayManager { get; private set; }
        public MultiformManager MultiformManager { get; private set; }
        public MouseInput Mouse { get; private set; }
        public KeyboardInput Keyboard { get; private set; }
        public EventPropagator EventPropagator { get; private set; }

        public ServiceLocator(
            Engine engine, ContentManager content, DisplayManager displayManager, MultiformManager multiformManager, 
            MouseInput mouseInput, KeyboardInput keyboardInput, EventPropagator eventPropagator)
        {
            this.Engine = engine;
            this.Content = content;
            this.DisplayManager = displayManager;
            this.MultiformManager = multiformManager;
            this.Mouse = mouseInput;
            this.Keyboard = keyboardInput;
            this.EventPropagator = eventPropagator;
        }

    }
}
