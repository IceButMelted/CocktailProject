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
    class MainMenu : Scene
    {
        private const String COCKTAIL_TEXT = "Cocktail : 410";

        private BG_Parallax bg;
        private BG_Parallax fg;
        private Point screenCenter;

        public Panel P_Start;
        public Panel P_Credit;
        public Panel P_Exit;
        public Button BTN_Start;
        public Button BTN_Credit;
        public Button BTN_Exit;

        public override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            screenCenter = new Point(Core.Graphics.PreferredBackBufferWidth / 2, Core.Graphics.PreferredBackBufferHeight / 2);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape)) Core.Instance.Exit();
            else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E)) Core.ChangeScene(new GamePlay());

            MouseState mouse = Mouse.GetState();
            bg.Update(mouse);
            fg.Update(mouse);

            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            Texture2D BG_Texture = Content.Load<Texture2D>("images/Background/BGP_Background");
            Texture2D FG_Texture = Content.Load<Texture2D>("images/Background/BGP_Foreground");

            // Parallax || xSensitivity, ySensitivity, extraImageScale
            bg = new BG_Parallax(BG_Texture, screenCenter, 0.09f, 0.03f, 1.1f);
            fg = new BG_Parallax(FG_Texture, screenCenter, 0.02f, 0.005f, 1.02f);

            P_Start = new Panel(new Vector2(300, 100), PanelSkin.Default, Anchor.Center);
            P_Start.Offset = new Vector2(-400, -50);
            P_Credit = new Panel(new Vector2(300, 100), PanelSkin.Default, Anchor.Center);
            P_Credit.Offset = new Vector2(-400, 100);
            P_Exit = new Panel(new Vector2(300, 100), PanelSkin.Default, Anchor.Center);
            P_Exit.Offset = new Vector2(-400, 250);

            BTN_Start = new Button("Start", ButtonSkin.Default, Anchor.Center);
            BTN_Start.Size = new Vector2(300, 100);
            BTN_Start.OnMouseDown  = (Entity e) => {
                Core.ChangeScene(new GamePlay()); 
            };
            BTN_Credit = new Button("Credit", ButtonSkin.Default, Anchor.Center);
            BTN_Credit.Size = new Vector2(300, 100);

            BTN_Exit = new Button("Exit", ButtonSkin.Default, Anchor.Center);
            BTN_Exit.Size = new Vector2(300, 100);
            BTN_Exit.OnMouseDown = (Entity e) => {
                Core.Instance.Exit();
            };


            P_Start.AddChild(BTN_Start);
            P_Credit.AddChild(BTN_Credit);
            P_Exit.AddChild(BTN_Exit);

            UserInterface.Active.AddEntity(P_Start);
            UserInterface.Active.AddEntity(P_Credit);
            UserInterface.Active.AddEntity(P_Exit);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.Black);

            Core.SpriteBatch.Begin();
            bg.Draw(Core.SpriteBatch, Core.Graphics, new Vector2(0, -50));
            fg.Draw(Core.SpriteBatch, Core.Graphics, new Vector2(0, 0));
            
            Core.SpriteBatch.End();

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);
        }
    }
}
