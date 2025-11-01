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
using Microsoft.Xna.Framework.Media;
using CocktailProject.Utilities;
using System.IO;
using System.Diagnostics;



namespace CocktailProject.Scenes
{
    class MainMenu : Scene
    {
        Song BGM_themeSong01;
        Song BGM_themeSong02;
        bool shouldPlayBGM_themeSong01 = true;
        bool shouldPlayBGM_themeSong02 = false;

        private BG_Parallax bg;
        private BG_Parallax fg;
        private Point screenCenter;
        public Image Logo; 
        public Panel P_Start;
        public Panel P_Credit;
        public Panel P_Exit;
        public Button BTN_Start; 
        public Button BTN_Credit; 
        public Button BTN_Exit; 
        public Image Img_Credit_Book;

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

            if(Core.Audio.IsSongFinished && shouldPlayBGM_themeSong01)
            {
                Core.Audio.PlaySong(BGM_themeSong02, false);
                Core.Audio.SongVolume = 0.25f;
                shouldPlayBGM_themeSong01 = false;
                shouldPlayBGM_themeSong02 = true;
            }
            else if (Core.Audio.IsSongFinished && shouldPlayBGM_themeSong02)
            {
                Core.Audio.PlaySong(BGM_themeSong01, false);
                Core.Audio.SongVolume = 0.25f;
                shouldPlayBGM_themeSong02 = false;
                shouldPlayBGM_themeSong01 = true;
            }

            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);

        }

        public override void LoadContent()
        {
            InitBGM();

            Texture2D BG_Texture = Content.Load<Texture2D>("images/Background/BGP_Background");
            Texture2D FG_Texture = Content.Load<Texture2D>("images/Background/BGP_Foreground");

            Texture2D T_BTN_Start_Default = Content.Load<Texture2D>("images/UI/MainMenu_UI/Start_Default");
            Texture2D T_BTN_Start_Hover = Content.Load<Texture2D>("images/UI/MainMenu_UI/Start_Hover");
            Texture2D T_BTN_Credit_Default = Content.Load<Texture2D>("images/UI/MainMenu_UI/Credit_Default");
            Texture2D T_BTN_Credit_Hover = Content.Load<Texture2D>("images/UI/MainMenu_UI/Credit_Hover");
            Texture2D T_BTN_Exit_Default = Content.Load<Texture2D>("images/UI/MainMenu_UI/Exit_Default");
            Texture2D T_BTN_Exit_Hover = Content.Load<Texture2D>("images/UI/MainMenu_UI/Exit_Hover");
            Texture2D T_Logo = Content.Load<Texture2D>("images/UI/MainMenu_UI/Bar410_Logo");

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
            Logo.Size = new Vector2(470+75, 320+75);
            Logo.Offset = new Vector2(100, 120-25);

            // Buttons
            BTN_Start = new Button("", ButtonSkin.Default, Anchor.Center);
            BTN_Start.Size = new Vector2(240, 80);
            BTN_Start.SetCustomSkin(T_BTN_Start_Default, T_BTN_Start_Hover, T_BTN_Start_Hover);
            BTN_Start.OnMouseDown = (Entity e) => {
                Core.ChangeScene(new Previs()); // Switch Scene here
                GlobalVariable.Reset();
            };
            BTN_Credit = new Button("", ButtonSkin.Default, Anchor.Center);
            BTN_Credit.Size = new Vector2(240, 80);
            BTN_Credit.SetCustomSkin(T_BTN_Credit_Default, T_BTN_Credit_Hover, T_BTN_Credit_Hover);
            BTN_Credit.OnMouseDown = (Entity e) =>
            {
                ToggleBookCredit();
            };

            BTN_Exit = new Button("", ButtonSkin.Default, Anchor.Center);
            BTN_Exit.Size = new Vector2(240, 80);
            BTN_Exit.SetCustomSkin(T_BTN_Exit_Default, T_BTN_Exit_Hover, T_BTN_Exit_Hover);
            BTN_Exit.OnMouseDown = (Entity e) => {
                Core.Instance.Exit();
            };


            P_Start.AddChild(BTN_Start);
            P_Credit.AddChild(BTN_Credit);
            P_Exit.AddChild(BTN_Exit);

            Img_Credit_Book = new Image(Content.Load<Texture2D>("images/Book_Credit"), new Vector2(1100, 800), ImageDrawMode.Stretch, Anchor.Center);

            EnableBookCredit(false);

            Button BTN_CloseBookRecipes = new Button("", ButtonSkin.Default, Anchor.TopRight, new Vector2(98-50, 140-50));
            BTN_CloseBookRecipes.OnClick += (Entity e) =>
            {
                ToggleBookCredit();
            };
            BTN_CloseBookRecipes.OnMouseDown += (Entity e) =>
            {
                BTN_CloseBookRecipes.Offset += new Vector2(0, +5);
                BTN_CloseBookRecipes.FillColor = Color.DarkGray;
            };
            BTN_CloseBookRecipes.OnMouseReleased += (Entity e) =>
            {
                BTN_CloseBookRecipes.Offset += new Vector2(0, -5);
                BTN_CloseBookRecipes.FillColor = Color.White;
            };
            BTN_CloseBookRecipes.Offset = new Vector2(80, 0);
            Texture2D T_BTN_CloseBookRecipes = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Close");
            Texture2D T_BTN_CloseBookRecipes_hover = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Close_Hover");
            BTN_CloseBookRecipes.SetCustomSkin(T_BTN_CloseBookRecipes, T_BTN_CloseBookRecipes, T_BTN_CloseBookRecipes);

            Img_Credit_Book.AddChild(BTN_CloseBookRecipes);

            UserInterface.Active.AddEntity(P_Start);
            UserInterface.Active.AddEntity(P_Credit);
            UserInterface.Active.AddEntity(P_Exit);
            UserInterface.Active.AddEntity(Logo);
            UserInterface.Active.AddEntity(Img_Credit_Book);

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

        public void InitBGM()
        {
            BGM_themeSong01 = Content.Load<Song>("Sound/Background_Music/ThemeSong01");
            BGM_themeSong02 = Content.Load<Song>("Sound/Background_Music/ThemeSong02");
            Core.Audio.PlaySong(BGM_themeSong01, false);
            Core.Audio.SongVolume = 0.25f;
        }

        public void ToggleBookCredit()
        {
            bool isActive = Img_Credit_Book.Visible;
            EnableBookCredit(!isActive);
        }

        public void EnableBookCredit(bool _Enable)
        {
            Img_Credit_Book.Visible = _Enable;
            Img_Credit_Book.Enabled = _Enable;
        }

        public void TakeScreenCap()
        {

            int w = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;

            //force a frame to be drawn (otherwise back buffer is empty) 
            Draw(new GameTime());

            //pull the picture from the buffer 
            int[] backBuffer = new int[w * h];
            Core.GraphicsDevice.GetBackBufferData(backBuffer);

            //copy into a texture 
            Texture2D texture = new Texture2D(Core.GraphicsDevice, w, h, false, Core.GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);

            //save to disk 
            Stream stream = File.OpenWrite("Test.png");
            Debug.WriteLine("Screenshot saved to " + Path.GetFullPath("Test.png"));

            texture.SaveAsPng(stream, w, h);
            stream.Dispose();

            texture.Dispose();
        }
    }
}
