#define DEBUG
#undef DEBUG
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
 * Date of Creation: 6/16/2015
 * 
 * Description
 * ===========
 * The DisplayManager maintains all properties regarding the display. It's responsibilities
 * include maintaining and appropriately updating the window properties when necessary (changes
 * in resolution, fullscreen vs windowed, borderlessness, etc.), and providing an easier interface
 * than spriteBatch for rendering.
 * 
 * DisplayManager knows when spriteBatch.Begin() and spriteBatch.End() have been called, thus making
 * it easier to draw objects. You can just call DisplayManager.Draw or DisplayManager.DrawString whenever
 * you want without worrying about whether the spriteBatch has begun or not. You can also set the
 * spriteBatch properties at will.
 */

#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze_V3.Framework.Extensions;
using Phosphaze_V3.Framework.Maths.Geometry;
using System;
using System.Linq;

#endregion

namespace Phosphaze_V3.Framework.Display
{
    public class DisplayManager
    {

        /// <summary>
        /// The valid standard resolutions.
        /// </summary>
        public static Resolution[] standardResolutions = {
            new Resolution(800, 600),  // 4:3
            new Resolution(960, 720),  // 4:3
            new Resolution(1024, 768), // 4:3
            new Resolution(1152, 864), // 4:3
            new Resolution(1280, 960), // 4:3
        };

        /// <summary>
        /// The valid widescreen resolutions.
        /// </summary>
        public static Resolution[] widescreenResolutions = {
            new Resolution(1280, 720), // 16:9
            new Resolution(1280, 768), // 5:3
            new Resolution(1280, 800), // 8:5
        };

        /// <summary>
        /// All valid resolutions.
        /// </summary>
        public static Resolution[] validResolutions =
            standardResolutions
            .Concat(widescreenResolutions)
            .Concat(new Resolution[] { Resolution.native });

        /// <summary>
        /// The index of the current resolution in validResolutions.
        /// </summary>
        public int currentResolutionIndex { get; private set; }

        public Resolution currentResolution { get { return validResolutions[currentResolutionIndex]; } }

        /// <summary>
        /// The maximum height from which all sprites are scaled relative to.
        /// </summary>
        public static readonly int maxResolutionHeight = validResolutions.Max(r => r.height);

        /// <summary>
        /// The amount by which to scale objects relative to the current resolution.
        /// </summary>
        public float resolutionScale { get { return (float)currentResolution.min / maxResolutionHeight; } }

        /* Monogame had the ingenious idea of spreading all the properties for
         * the window out over 5 different classes! So we have to keep them all
         * here if we want to be able to modify everything via one interface.
         * 
         * For example, the only way to change mouse visibility is through ``game``,
         * but the only way to change screen dimensions is through graphicsManager.
         * 
         * I don't like how coupled it's become, but it works for now.
         */

        /// <summary>
        /// The global game object.
        /// </summary>
        private Game game;

        /// <summary>
        /// The global game GraphicsDevice.
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// The global game GraphicsDeviceManager.
        /// </summary>
        private GraphicsDeviceManager graphicsManager;

        /// <summary>
        /// The global game SpriteBatch.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The main game window.
        /// </summary>
        private GameWindow window;

        /// <summary>
        /// The background colour to clear the screen with.
        /// </summary>
        private Color bgFill;

        /// <summary>
        /// A flag indicating when the screen's properties have changed.
        /// </summary>
        private bool dirty = true;

        /// <summary>
        /// Whether or not spriteBatch.Begin has been called.
        /// </summary>
        private bool begun = false;

        /// <summary>
        /// Whether or not the screen is borderless.
        /// </summary>
        public bool borderless { get; private set; }

        /// <summary>
        /// Whether or not the display is fullscreen.
        /// </summary>
        public bool fullscreen { get; private set; }

        /// <summary>
        /// Whether or not the mouse is visible.
        /// </summary>
        public bool mouseVisible { get; private set; }

        public DisplayManager(
            Game game, GraphicsDeviceManager graphicsManager, 
            SpriteBatch spriteBatch, Color backgroundFill)
        {
            this.graphicsDevice = game.GraphicsDevice;
            this.graphicsManager = graphicsManager;
            this.spriteBatch = spriteBatch;
            this.window = game.Window;
            this.bgFill = backgroundFill;
            this.game = game;

            currentResolutionIndex = validResolutions.Length - 1; // Always default to native resolution.
#if DEBUG
            // It's easier to debug in windowed mode.
            currentResolutionIndex = 0;
#endif
            borderless = true;
            fullscreen = false;
            mouseVisible = false;

            ReinitScreenProperties();
        }

        /// <summary>
        /// Apply changes to the display and prepare for rendering.
        /// </summary>
        public void BeginRender()
        {
            if (dirty)
                ReinitScreenProperties();
            graphicsDevice.Clear(bgFill);
        }

        /// <summary>
        /// Finish the render phase.
        /// </summary>
        public void EndRender()
        {
            if (begun)
            {
                spriteBatch.End();
                begun = false;
            }
        }

        /// <summary>
        /// Set new properties on the SpriteBatch.
        /// </summary>
        /// <param name="sortMode"></param>
        /// <param name="blendState"></param>
        /// <param name="samplerState"></param>
        /// <param name="depthStencilState"></param>
        /// <param name="rasterizerState"></param>
        /// <param name="effect"></param>
        /// <param name="transformMatrix"></param>
        public void SetSpriteBatchProperties(
            SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null,
            SamplerState samplerState = null, DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            if (begun)
                spriteBatch.End();

            spriteBatch.Begin(
                sortMode, blendState, samplerState, 
                depthStencilState, rasterizerState, 
                effect, transformMatrix
                );
        }

        public void ClearSpriteBatchProperties()
        {
            if (begun)
            {
                spriteBatch.End();
                begun = false;
            }
        }

        /// <summary>
        /// Initialize the properties of the screen form. These properties are whether or not the
        /// screen is borderless, whether or not it's fullscreen, its position on the screen,
        /// its width and its height.
        /// </summary>
        private void ReinitScreenProperties()
        {
            game.IsMouseVisible = mouseVisible;
            window.IsBorderless = borderless;

            graphicsManager.IsFullScreen = fullscreen;
            graphicsManager.PreferredBackBufferWidth = currentResolution.width;
            graphicsManager.PreferredBackBufferHeight = currentResolution.height;

            // Center the display based on the native resolution.
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(window.Handle);
            var position = new System.Drawing.Point(
                (Resolution.native.width - currentResolution.width) / 2,
                (Resolution.native.height - currentResolution.height) / 2
                );

            // This offset seems to width and height of the windows border,
            // so it accounts for the slight off-centering (unless the window
            // is larger than the native display).
            if (!borderless)
            {
                position.X -= Constants.WINDOW_BOUNDARY_OFFSET.X;
                position.Y -= Constants.WINDOW_BOUNDARY_OFFSET.Y;
            }
            
            graphicsManager.ApplyChanges();

            /* We have to reposition the form after we apply changes to the graphics manager
             * otherwise if the user changes the resolution while in fullscreen, then goes into
             * windowed mode, the window would be position relative to the previous resolution
             * rather than the new resolution.
             */
            form.Location = position;

            dirty = false;
        }

        /// <summary>
        /// Toggle the borderlessness of the display.
        /// </summary>
        public void ToggleBorder()
        {
            borderless ^= true;
            dirty = true;
        }

        /// <summary>
        /// Set the borderlessness of the display.
        /// </summary>
        /// <param name="state"></param>
        public void SetBorderless(bool state)
        {
            borderless = state;
            dirty = true;
        }

        /// <summary>
        /// Toggle whether or not the display is fullscreen.
        /// </summary>
        public void ToggleFullscreen()
        {
            fullscreen ^= true;
            dirty = true;
        }

        /// <summary>
        /// Set whether or not the display is fullscreen.
        /// </summary>
        /// <param name="state"></param>
        public void SetFullscreen(bool state)
        {
            fullscreen = state;
            dirty = true;
        }

        /// <summary>
        /// Toggle the mouse visibility.
        /// </summary>
        public void ToggleMouseVisibility()
        {
            mouseVisible ^= true;
            dirty = true;
        }

        /// <summary>
        /// Set whether or not the mouse is visible.
        /// </summary>
        /// <param name="state"></param>
        public void SetMouseVisibility(bool state)
        {
            mouseVisible = state;
            dirty = true;
        }

        /// <summary>
        /// Get the resolution with the given index in the list of valid resolutions.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Resolution GetResolution(int index)
        {
            return validResolutions[index];
        }

        /// <summary>
        /// Set the current resolution to that with the given index in the list of valid resolutions.
        /// </summary>
        /// <param name="index"></param>
        public void SetResolution(int index)
        {
            currentResolutionIndex = index % validResolutions.Length;
            dirty = true;
        }

        public void NextResolution()
        {
            currentResolutionIndex++;
            currentResolutionIndex %= validResolutions.Length;
            dirty = true;
        }

        public void PrevResolution()
        {
            currentResolutionIndex--;
            currentResolutionIndex %= validResolutions.Length;
            dirty = true;
        }

        /// <summary>
        /// Set the current resolution to the given one.
        /// </summary>
        /// <param name="resolution"></param>
        public void SetResolution(Resolution resolution)
        {
            if (!validResolutions.Contains(resolution))
                throw new ArgumentException(
                    String.Format("Invalid resolution {0}x{1}; not contained in DisplayManager.validResolutions.",
                                  resolution.width, resolution.height)
                                  );
            currentResolutionIndex = Array.IndexOf<Resolution>(validResolutions, resolution);
            dirty = true;
        }

        private Vector2 ScaleVector(Vector2 vec)
        {
            return new Vector2(vec.X * currentResolution.width, vec.Y * currentResolution.height);
        }

        private void DrawInit(
            ref float scale, ref Vector2 position, Vector2? offset, 
            Vector2 center, float rotation, bool centred)
        {
            if (!begun)
            {
                spriteBatch.Begin();
                begun = true;
            }
            scale *= resolutionScale;
            position = ScaleVector(position);

            if (centred)
            {
                position -= center * scale;
            }

            /*
            if (offset.HasValue)
                position -= ScaleVector(offset.Value);
             */
        }

        // The following region just contains a bunch of overloads for the Draw
        // method, each of which being a partition of the total arguments (i.e. instead of all
        // 10 arguments, only texture, position, and color, etc.)
        //
        // For the sake of brevity, keep it closed.
        #region Draw Overloads

        public void Draw(
            Texture2D texture, Vector2 position, bool centred = true)
        {
            Draw(texture, position, null, Color.White, 0f, null, 1f, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, bool centred = true)
        {
            Draw(texture, position, null, color, 0f, null, 1f, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Vector2 offset,
            bool centred = true)
        {
            Draw(texture, position, null, Color.White, 0f, offset, 1f, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, Vector2 offset,
            bool centred = true)
        {
            Draw(texture, position, null, color, 0f, offset, 1f, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, float scale,
            bool centred = true)
        {
            Draw(texture, position, null, Color.White, 0f, null, scale, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, float scale,
            bool centred = true)
        {
            Draw(texture, position, null, color, 0f, null, scale, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, float rotation,
            float scale, bool centred = true)
        {
            Draw(texture, position, null, Color.White, rotation, null, scale, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, float rotation,
            float scale, bool centred = true)
        {
            Draw(texture, position, null, color, rotation, null, scale, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, float rotation,
            Vector2 offset, float scale, bool centred = true)
        {
            Draw(texture, position, null, Color.White, rotation, offset, scale, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, float rotation,
            Vector2 offset, float scale, bool centred = true)
        {
            Draw(texture, position, null, color, rotation, offset, scale, SpriteEffects.None, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, SpriteEffects fx,
            bool centred = true)
        {
            Draw(texture, position, null, Color.White, 0f, null, 0f, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, SpriteEffects fx,
            bool centred = true)
        {
            Draw(texture, position, null, color, 0f, null, 0f, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Vector2 offset,
            SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, Color.White, 0f, offset, 0f, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, Vector2 offset,
            SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, color, 0f, offset, 0f, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, float scale,
            SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, Color.White, 0f, null, scale, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, float scale,
            SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, color, 0f, null, scale, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, float rotation,
            float scale, SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, Color.White, rotation, null, scale, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, float rotation,
            float scale, SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, color, rotation, null, scale, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, float rotation,
            Vector2 offset, float scale, SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, Color.White, rotation, offset, scale, fx, 0f, centred);
        }

        public void Draw(
            Texture2D texture, Vector2 position, Color color, float rotation,
            Vector2 offset, float scale, SpriteEffects fx, bool centred = true)
        {
            Draw(texture, position, null, color, rotation, offset, scale, fx, 0f, centred);
        }

        #endregion

        /// <summary>
        /// Draw a texture. This will properly scale the object so that it matches the current
        /// resolution. The position and offset vectors must be values in the unit square (i.e.
        /// |x| <= 1 && |y| <= 1). These represent the percentage from the left of the screen
        /// and from the top of the screen respectively, meaning (0.5, 0.5) would be the center
        /// of the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="source"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="offset"></param>
        /// <param name="scale"></param>
        /// <param name="fx"></param>
        /// <param name="layer"></param>
        /// <param name="centred"></param>
        public void Draw(
            Texture2D texture, Vector2 position, Rectangle? source, Color color,
            float rotation, Vector2? offset, float scale, SpriteEffects fx, float layer,
            bool centred = true)
        {
            if (!begun)
            {
                spriteBatch.Begin();
                begun = true;
            }
            // DrawInit(ref scale, ref position, offset, new Vector2(texture.Width, texture.Height) / 2f, rotation, centred);
            scale *= resolutionScale;
            var P = ScaleVector(position);
            var D = new Vector2(texture.Width, texture.Height) / 2f;
            var A = P - D;
            var Pp = VectorUtils.Rotate(D, -rotation, Vector2.Zero, degrees: false);
            var r = P - Pp;
            spriteBatch.Draw(texture, A, source, color, rotation, Vector2.Zero, scale, fx, layer);
            // Fuck Monogame's offset parameter, it makes no fucking sense.
        }

        // Same as above, but even worse.
        #region DrawString Overloads

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, 0f, null, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, 0f, null, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation,
            bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, null, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
            bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, null, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation, float scale,
            bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, null, scale, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
            float scale, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, null, scale, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Vector2 offset, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, 0f, offset, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, Vector2 offset, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, 0f, offset, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation, Vector2 offset, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, offset, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, 
            Vector2 offset, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, offset, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation, Vector2 offset,
            float scale, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, offset, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, 
            Vector2 offset, float scale, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, offset, 0f, SpriteEffects.None, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, 0f, null, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, SpriteEffects fx,
            bool centred = true)
        {
            DrawString(spriteFont, text, position, color, 0f, null, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation,
            SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, null, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
            SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, null, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation, float scale,
            SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, null, scale, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
            float scale, SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, null, scale, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Vector2 offset, SpriteEffects fx,
            bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, 0f, offset, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, Vector2 offset, 
            SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, 0f, offset, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation, Vector2 offset, 
            SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, offset, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
            SpriteEffects fx, Vector2 offset, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, offset, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, float rotation, Vector2 offset,
            float scale, SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, Color.White, rotation, offset, 0f, fx, 0f, centred);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation,
            Vector2 offset, float scale, SpriteEffects fx, bool centred = true)
        {
            DrawString(spriteFont, text, position, color, rotation, offset, 0f, fx, 0f, centred);
        }

        #endregion

        /// <summary>
        /// Draw a string. This will properly scale the text so that it matches the current
        /// resolution. The position and offset vectors must be values in the unit square (i.e.
        /// |X| <= 1 $$ |y| <= 1). These represent the percentage from the left of the screen
        /// and from the top of the screen respectively, meaning (0.5, 0.5) would be the center
        /// of the screen.
        /// </summary>
        /// <param name="spriteFont"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="offset"></param>
        /// <param name="scale"></param>
        /// <param name="fx"></param>
        /// <param name="layerDepth"></param>
        /// <param name="centred"></param>
        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color,
            float rotation, Vector2? offset, float scale, SpriteEffects fx,
            float layerDepth, bool centred = true)
        {
            DrawInit(ref scale, ref position, offset, spriteFont.MeasureString(text) / 2f, rotation, centred);
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, Vector2.Zero, scale, fx, layerDepth);
        }
    }
}
