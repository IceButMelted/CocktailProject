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

        public Image Logo; public Texture2D T_Logo;
        public Panel P_Start;
        public Panel P_Credit;
        public Panel P_Exit;
        public Button BTN_Start; public Texture2D T_BTN_Start_Default; public Texture2D T_BTN_Start_Hover;
        public Button BTN_Credit; public Texture2D T_BTN_Credit_Default; public Texture2D T_BTN_Credit_Hover;
        public Button BTN_Exit; public Texture2D T_BTN_Exit_Default; public Texture2D T_BTN_Exit_Hover;

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

            T_BTN_Start_Default = Content.Load<Texture2D>("images/UI/MainMenu_UI/Start_Default");
            T_BTN_Start_Hover = Content.Load<Texture2D>("images/UI/MainMenu_UI/Start_Hover");
            T_BTN_Credit_Default = Content.Load<Texture2D>("images/UI/MainMenu_UI/Credit_Default");
            T_BTN_Credit_Hover = Content.Load<Texture2D>("images/UI/MainMenu_UI/Credit_Hover");
            T_BTN_Exit_Default = Content.Load<Texture2D>("images/UI/MainMenu_UI/Exit_Default");
            T_BTN_Exit_Hover = Content.Load<Texture2D>("images/UI/MainMenu_UI/Exit_Hover");
            T_Logo = Content.Load<Texture2D>("images/UI/MainMenu_UI/Bar410_Logo");

            // Parallax || xSensitivity, ySensitivity, extraImageScale
            bg = new BG_Parallax(BG_Texture, screenCenter, 0.09f, 0.03f, 1.1f);
            fg = new BG_Parallax(FG_Texture, screenCenter, 0.02f, 0.005f, 1.02f);

            // Panels
            P_Start = new Panel(new Vector2(240, 80), PanelSkin.None, Anchor.TopLeft);
            P_Start.Offset = new Vector2(200, 536);
            P_Credit = new Panel(new Vector2(240, 80), PanelSkin.None, Anchor.TopLeft);
            P_Credit.Offset = new Vector2(200, 660);
            P_Exit = new Panel(new Vector2(240, 80), PanelSkin.None, Anchor.TopLeft);
            P_Exit.Offset = new Vector2(200, 794);

            Logo = new Image(T_Logo, anchor: Anchor.TopLeft);
            Logo.Size = new Vector2(470, 320);
            Logo.Offset = new Vector2(100, 120);

            // Buttons
            BTN_Start = new Button("", ButtonSkin.Default, Anchor.Center);
            BTN_Start.Size = new Vector2(240, 80);
            BTN_Start.SetCustomSkin(T_BTN_Start_Default, T_BTN_Start_Hover, T_BTN_Start_Hover);
            BTN_Start.OnMouseDown = (Entity e) => {
                Core.ChangeScene(new GamePlay());
            };
            BTN_Credit = new Button("", ButtonSkin.Default, Anchor.Center);
            BTN_Credit.Size = new Vector2(240, 80);
            BTN_Credit.SetCustomSkin(T_BTN_Credit_Default, T_BTN_Credit_Hover, T_BTN_Credit_Hover);
            //BTN_Credit.OnMouseDown = (Entity e) => {
            //    Core.ChangeScene(new GamePlay());
            //};

            BTN_Exit = new Button("", ButtonSkin.Default, Anchor.Center);
            BTN_Exit.Size = new Vector2(240, 80);
            BTN_Exit.SetCustomSkin(T_BTN_Exit_Default, T_BTN_Exit_Hover, T_BTN_Exit_Hover);
            BTN_Exit.OnMouseDown = (Entity e) => {
                Core.Instance.Exit();
            };


            P_Start.AddChild(BTN_Start);
            P_Credit.AddChild(BTN_Credit);
            P_Exit.AddChild(BTN_Exit);

            UserInterface.Active.AddEntity(P_Start);
            UserInterface.Active.AddEntity(P_Credit);
            UserInterface.Active.AddEntity(P_Exit);
            UserInterface.Active.AddEntity(Logo);

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
