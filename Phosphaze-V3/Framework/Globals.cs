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
 * Date of Creation: 6/12/2015
 * 
 * Description
 * ===========
 * This file contains references to all globally retrievable objects
 * in Phosphaze.
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Phosphaze_V3.Framework.Input;

#endregion

namespace Phosphaze_V3.Framework
{
    public static class Globals
    {

        /// <summary>
        /// The current GameTime object.
        /// </summary>
        public static GameTime gameTime;

        /// <summary>
        /// The global game ContentManager.
        /// </summary>
        public static ContentManager content;

        /// <summary>
        /// The time since the last call to the game's update method.
        /// This is short for GlobalProperties.Instance.gameTime.ElapsedGameTime.Milliseconds.
        /// </summary>
        public static double deltaTime;

    }
}
