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

namespace CocktailProject.Scenes
{
    class Previs : Scene
    {

        private Texture2D PrevisAtlas;
        private List<Rectangle> frames = new List<Rectangle>();
        private List<double> frameDurations = new List<double>();
        private int currentFrame = 0;
        private double timer = 0;

        private double elapsedTime = 0;

        Song Theme01;

        public Panel P_Skip;
        public Button BTN_Skip;
        public Texture2D T_BTN_Skip_Default;
        public Texture2D T_BTN_Skip_Hover;

        private bool skipButtonVisible = false;

        public override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape)) Core.Instance.Exit();

            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            // Visible Button
            if (!skipButtonVisible && elapsedTime >= 3.0) // Change invisible time here
            {
                skipButtonVisible = true;
                UserInterface.Active.AddEntity(P_Skip);
            }

            if (currentFrame >= frames.Count)
            {
                Core.ChangeScene(new GamePlay()); // Switch Scene here
                return;
            }

            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameDurations[currentFrame])
            {
                timer = 0;
                currentFrame++;
            }

            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            Theme01 = Content.Load<Song>("Sound/Background_Music/PreviseScene");
            Core.Audio.PlaySong(Theme01, false);
            Core.Audio.SongVolume = (0.1f);
            PrevisAtlas = Content.Load<Texture2D>("images/Previs/Atlas_Previs");

            // Frames Configs
            frames.Add(new Rectangle(0, 0, 1920, 700));
            frameDurations.Add(4.0);
            frames.Add(new Rectangle(0, 700, 1920, 700));
            frameDurations.Add(4.0);
            frames.Add(new Rectangle(0, 1400, 1920, 700));
            frameDurations.Add(2.0);
            frames.Add(new Rectangle(0, 2100, 1920, 700));
            frameDurations.Add(0.6);
            frames.Add(new Rectangle(0, 2800, 1920, 700));
            frameDurations.Add(3.0);

            T_BTN_Skip_Default = Content.Load<Texture2D>("images/Previs/Skip_Default");
            T_BTN_Skip_Hover = Content.Load<Texture2D>("images/Previs/Skip_Hover");

            // Panels
            P_Skip = new Panel(new Vector2(240, 80), PanelSkin.None, Anchor.BottomRight);
            P_Skip.Offset = new Vector2(40, 60);

            // Buttons
            BTN_Skip = new Button("", ButtonSkin.Default, Anchor.Center);
            BTN_Skip.Size = new Vector2(240, 80);
            BTN_Skip.SetCustomSkin(T_BTN_Skip_Default, T_BTN_Skip_Hover, T_BTN_Skip_Hover);
            BTN_Skip.OnMouseDown = (Entity e) => {
                
                Core.ChangeScene(new GamePlay());
            };

            P_Skip.AddChild(BTN_Skip);

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
                Core.SpriteBatch.Draw(PrevisAtlas, dest, src, Color.White);
                Core.SpriteBatch.End();
            }
            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);
        }
    }
}
