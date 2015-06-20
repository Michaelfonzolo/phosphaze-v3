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
 * This file is the main Monogame Game object.
 */

#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Phosphaze_V3.Framework;
using Phosphaze_V3.Framework.Forms;
using Phosphaze_V3.Framework.Input;
using Phosphaze_V3.Framework.Extensions;
using Phosphaze_V3.Framework.Display;

#endregion

namespace Phosphaze_V3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Phosphaze : Game
    {
        public GraphicsDeviceManager graphics { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        MultiformManager multiformManager;
        DisplayManager displayManager;

        // TEMPORARY
        Texture2D texture;

        public Phosphaze()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Globals.content = Content;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //multiformManager = new MultiformManager();
            displayManager = new DisplayManager(this, graphics, spriteBatch, Constants.BG_FILLCOL);

            texture = Content.Load<Texture2D>("TestContent/Speaker1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Globals.gameTime = gameTime;

            // NOTE: gameTime.ElapsedGameTime.Milliseconds is unfortunately stored as an
            // integer, meaning at 60fps (which is the default for Monogame) deltaTime would
            // normally be 16 instead of the actual 16.66666...
            //
            // To compensate for this, since a game usually never lags enough to cause the
            // framerate to drop, we just set the deltaTime to 16.6666... whenever the elapsed
            // milliseconds is exactly 16 (which also happens to be its minimum).
            Globals.deltaTime = Math.Max(gameTime.ElapsedGameTime.Milliseconds, Constants.MIN_DTIME);

            // Update the input
            Globals.mouseInput.Update();
            Globals.keyboardInput.Update();

            // Update the multiforms.
            // multiformManager.Update();

            if (Globals.keyboardInput.IsReleased(Keys.Escape))
                Exit();
            if (Globals.keyboardInput.IsReleased(Keys.Enter))
                displayManager.SetResolution(displayManager.currentResolutionIndex + 1);
            if (Globals.keyboardInput.IsReleased(Keys.B))
                displayManager.ToggleBorder();
            if (Globals.keyboardInput.IsReleased(Keys.F))
                displayManager.ToggleFullscreen();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            displayManager.BeginUpdate();

            displayManager.Draw(texture, new Vector2(0.5f, 0.5f));
            displayManager.Draw(texture, new Vector2(0.25f, 0.5f));

            displayManager.EndUpdate();

            base.Draw(gameTime);
        }
    }
}
