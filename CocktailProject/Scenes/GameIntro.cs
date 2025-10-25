using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;

using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;



namespace CocktailProject.Scenes
{
    class GameIntro : Scene
    {

        private Texture2D introAtlas;
        private List<Rectangle> frames = new List<Rectangle>();
        private List<double> frameDurations = new List<double>();
        private int currentFrame = 0;
        private double timer = 0;

        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Check if we've reached the end before doing anything
            if (currentFrame >= frames.Count)
            {
                Core.ChangeScene(new MainMenu()); //Change scene here
                return;
            }

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            timer += gameTime.ElapsedGameTime.TotalSeconds;

            bool spacePressed = keyboard.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space);
            bool enterPressed = keyboard.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter);
            bool mousePressed = mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;

            if (spacePressed || enterPressed || mousePressed)
            {
                timer = 0;
                currentFrame++;
            }
            else if (timer >= frameDurations[currentFrame])
            {
                timer = 0;
                currentFrame++;
            }

            previousKeyboardState = keyboard;
            previousMouseState = mouse;

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            introAtlas = Content.Load<Texture2D>("images/Intro_Atlas");

            // Frames Configs
            frames.Add(new Rectangle(0, 0, 1000, 200));     // Unjob Title
            frameDurations.Add(3.5);
            frames.Add(new Rectangle(0, 200, 1000, 200));   // MNG Title
            frameDurations.Add(3.5);
            frames.Add(new Rectangle(0, 400, 1000, 400));   // Caution Title
            frameDurations.Add(5);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.Black);
            if (currentFrame < frames.Count)
            {
                Core.SpriteBatch.Begin();
                Rectangle src = frames[currentFrame];
                Rectangle dest = new Rectangle(
                    Core.Graphics.PreferredBackBufferWidth / 2 - src.Width / 2,
                    Core.Graphics.PreferredBackBufferHeight / 2 - src.Height / 2,
                    src.Width,
                    src.Height
                );
                Core.SpriteBatch.Draw(introAtlas, dest, src, Color.White);
                Core.SpriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
