using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze_V3.Framework.Display;
using Phosphaze_V3.Framework.Input;
using Phosphaze_V3.Framework.Events;
using Phosphaze_V3.Framework.Forms;

namespace Phosphaze_V3.Framework
{
    public abstract class Engine
    {

        /// <summary>
        /// The global GameTime object.
        /// </summary>
        public GameTime gameTime { get; private set; }

        /// <summary>
        /// The amount of time passed since the last call to Update (in milliseconds).
        /// </summary>
        public double deltaTime { get; private set; }

        /// <summary>
        /// The total elapsed time since the beginning of the game in milliseconds.
        /// </summary>
        public double elapsedTime { get { return gameTime.ElapsedGameTime.TotalMilliseconds; } }

        /// <summary>
        /// The total number of elapsed frames since the beginning of the game.
        /// </summary>
        public int elapsedFrames { get; private set; }

        internal bool exited = false;

        // We don't really need these references to the different single instances,
        // as we have the serviceLocator for that, but it just makes it cleaner in
        // Update and Render, as we don't have to say serviceLocator.multiformManager,
        // we can just say multiformManager instead.
        public MultiformManager multiformManager { get; private set; }
        private DisplayManager displayManager;
        private MouseInput mouseInput;
        private KeyboardInput keyboardInput;
        private EventPropagator eventPropagator;

        private ServiceLocator serviceLocator;

        // So apparently the fuckin' Monogame Game object checks if a GraphicsDeviceManager
        // was made in it's constructor, but then it constructs the fuckin' GraphicsDevice
        // in Initialize. <sarcasm> Ohhhh! That makes sense! </sarcasm> So now we have to
        // fucking send in this stupid bullshit.
        private GraphicsDeviceManager graphics;

        public Engine() { elapsedFrames = 0; }

        public void Initialize(Game game)
        {
            this.multiformManager = new MultiformManager();
            var spriteBatch = new SpriteBatch(game.GraphicsDevice);

            this.displayManager = new DisplayManager(game, graphics, spriteBatch, Constants.BG_FILLCOL);
            this.mouseInput = new MouseInput();
            this.keyboardInput = new KeyboardInput();
            this.eventPropagator = new EventPropagator();

            this.serviceLocator = new ServiceLocator(
                this, game.Content, displayManager, multiformManager, 
                mouseInput, keyboardInput, eventPropagator);

            this.eventPropagator.SetServiceLocator(this.serviceLocator);
        }

        public abstract void SetupMultiforms();

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            /* NOTE: gameTime.ElapsedGameTime.Milliseconds is unfortunately stored as an
             * integer, meaning at 60fps (which is the default for Monogame) deltaTime would
             * normally be 16 instead of the actual 16.66666...
             *
             * To compensate for this, since a game usually never lags enough to cause the
             * framerate to drop, we just set the deltaTime to 16.6666... whenever the elapsed
             * milliseconds is exactly 16 (which also happens to be its minimum).
             */
            this.deltaTime = Math.Max(gameTime.ElapsedGameTime.Milliseconds, Constants.MIN_DTIME);

            mouseInput.Update(serviceLocator);
            keyboardInput.Update(serviceLocator);
            multiformManager.Update(serviceLocator);
        }

        public void Render()
        {
            displayManager.BeginRender();

            multiformManager.Render(serviceLocator);

            displayManager.EndRender();
        }

        public void Exit()
        {
            exited = true;
        }

        /// <summary>
        /// Deal with the bullshit.
        /// </summary>
        /// <param name="graphics"></param>
        internal void SendGraphicsDeviceManager(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }
    }
}
