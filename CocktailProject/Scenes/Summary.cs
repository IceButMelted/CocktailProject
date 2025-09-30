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
    class Summary : Scene
    {
        private const String COCKTAIL_TEXT = "Cocktail : 410";

        private BG_Parallax bg;
        private Point screenCenter;

        public override void Initialize()
        {
            screenCenter = new Point(Core.Graphics.PreferredBackBufferWidth / 2, Core.Graphics.PreferredBackBufferHeight / 2);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape)) Core.Instance.Exit();
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E)) Core.ChangeScene(new MainMenu());

            MouseState mouse = Mouse.GetState();
            bg.Update(mouse);

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            Texture2D BG_Texture = Content.Load<Texture2D>("images/Background/BG_Summary");
            bg = new BG_Parallax(BG_Texture, screenCenter, 0.01f, 0.005f, 1.05f); // Parallax || xSensitivity, ySensitivity, extraImageScale

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            Core.SpriteBatch.Begin();
            bg.Draw(Core.SpriteBatch, Core.Graphics, Vector2.Zero);
            Core.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
