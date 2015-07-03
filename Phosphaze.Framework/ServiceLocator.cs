#region License

// Copyright (c) 2015 FCDM
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished 
// to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region Header

/* Author: Michael Ala
 * Date of Creation: 6/28/2015
 * 
 * Description
 * ===========
 * The ServiceLocator acts as a hub for all the important single instance objects in
 * the framework. It gets passed down through everything that needs to access it
 * (generally Update methods of various game related objects).
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework.Content;
using Phosphaze_V3.Framework.Display;
using Phosphaze_V3.Framework.Events;
using Phosphaze_V3.Framework.Forms;
using Phosphaze_V3.Framework.Input;

#endregion

namespace Phosphaze_V3.Framework
{
    public class ServiceLocator
    {

        // You can figure out what these mean.

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
