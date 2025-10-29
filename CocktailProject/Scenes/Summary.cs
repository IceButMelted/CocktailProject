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
        private const int LabelWidth = 28;
        private const int ValueWidth = 5;
        private static readonly Color GoldColor = new Color(192, 130, 30);

        protected Image Img_Summary;
        protected Texture2D T_Img_Summary;

        protected SpriteFont RegularFont;
        protected SpriteFont BoldFont;
        protected SpriteFont ItalicFont;

        private BG_Parallax bg;
        private Point screenCenter;

        // NEW: Timer for 3-second delay
        private double timeElapsed = 0;
        private bool canContinue = false;

        // NEW: Paragraph for “Click Anywhere to Continue”
        private RichParagraph continueText;

        public override void Initialize()
        {
            screenCenter = new Point(
                Core.Graphics.PreferredBackBufferWidth / 2,
                Core.Graphics.PreferredBackBufferHeight / 2);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // update time
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
                Core.Instance.Exit();

            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E))
                Core.ChangeScene(new MainMenu());

            bg.Update(Mouse.GetState());
            UserInterface.Active.Update(gameTime);

            // NEW: After 3 seconds, show the continue text
            if (!canContinue && timeElapsed >= 3)
            {
                canContinue = true;
                continueText.Visible = true;
            }

            // NEW: Handle Q or mouse click only after continueText appears
            if (canContinue &&
                (Core.Input.Keyboard.WasKeyJustPressed(Keys.Q) ||
                 Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left)))
            {
                if (GlobalVariable.Day >= 2)
                    Core.ChangeScene(new Thanks());
                else
                {
                    UserInterface.Active.Clear();
                    GlobalVariable.NextDay();
                    Core.ChangeScene(new GamePlay());
                }
            }

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            RegularFont = Content.Load<SpriteFont>("Fonts/Regular");
            BoldFont = Content.Load<SpriteFont>("Fonts/Bold");
            ItalicFont = Content.Load<SpriteFont>("Fonts/Italic");

            RichParagraphStyleInstruction.AddInstruction(
                "MENU_TEXT",
                new RichParagraphStyleInstruction(fillColor: new Color(235, 228, 202)));

            Texture2D bgTexture = Content.Load<Texture2D>("images/Background/BG_Summary");
            bg = new BG_Parallax(bgTexture, screenCenter, 0.01f, 0.005f, 1.05f);

            T_Img_Summary = Content.Load<Texture2D>("images/UI/Summary/Summary_Panel");
            Img_Summary = new Image(T_Img_Summary)
            {
                Size = new Vector2(600, 800),
                Anchor = Anchor.TopLeft,
                Offset = new Vector2(130, 140)
            };

            Paragraph title = new Paragraph("Summary", Anchor.TopCenter)
            {
                Size = new Vector2(500, 60),
                Offset = new Vector2(0, 40),
                FontOverride = BoldFont,
                FillColor = new Color(235, 228, 202),
                OutlineOpacity = 0
            };

            Panel panel = new Panel(new Vector2(500, 500), PanelSkin.None, Anchor.Center);

            string formattedCustomer = $"{"Customer".PadRight(LabelWidth)}{GlobalVariable.customerNumber,ValueWidth}";
            string formattedIncome = $"{"Income".PadRight(LabelWidth)}{GlobalVariable.Income,ValueWidth}";
            string formattedTip = $"{"Tip".PadRight(LabelWidth)}{GlobalVariable.Tip,ValueWidth}";
            Paragraph txtCustomer = CreateLabel(formattedCustomer);
            Paragraph txtIncome = CreateLabel(formattedIncome);

            panel.AddChild(txtCustomer);
            panel.AddChild(txtIncome);

            int maxNameLength = 10;

            if (GlobalVariable.CocktailHaveDone.Count != 0)
            {
                maxNameLength = GlobalVariable.CocktailHaveDone.Keys.Max(k => k.Length);

                foreach (var kvp in GlobalVariable.CocktailHaveDone)
                {
                    string formattedLine = $"   {kvp.Key.PadRight(maxNameLength + 5)}{kvp.Value,5}";
                    panel.AddChild(CreateLabel(formattedLine));
                }
            }
            else
            {
                panel.AddChild(CreateLabel("   No cocktails you made is good enough."));
            }

            Paragraph txtTip = CreateLabel(formattedTip);
            panel.AddChild(txtTip);

            int grandTotal = GlobalVariable.Income + GlobalVariable.Tip;
            Paragraph txt_Total = new Paragraph(
                $"Total".PadRight(maxNameLength + 9) + $"{grandTotal,5}",
                Anchor.BottomCenter
            );
            txt_Total.FontOverride = BoldFont;
            txt_Total.FillColor = new Color(235, 228, 202);
            txt_Total.OutlineOpacity = 0;
            panel.AddChild(txt_Total);

            Img_Summary.AddChild(title);
            Img_Summary.AddChild(panel);
            UserInterface.Active.AddEntity(Img_Summary);

            // NEW: Add “Click Anywhere to Continue” RichParagraph
            continueText = new RichParagraph(
                "Click Anywhere To Continue",
                Anchor.BottomCenter
            )
            {
                FontOverride = BoldFont,
                FillColor = new Color(235, 228, 202),
                OutlineOpacity = 0,
                Offset = new Vector2(0, -50),
                Visible = false // hidden until 3 sec passed
            };
            continueText.Anchor = Anchor.BottomCenter;
            continueText.Offset = new Vector2(0, 50);

            UserInterface.Active.AddEntity(continueText);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            Core.SpriteBatch.Begin();
            bg.Draw(Core.SpriteBatch, Core.Graphics, Vector2.Zero);
            Core.SpriteBatch.End();

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);
        }

        private Paragraph CreateLabel(string text)
        {
            return new Paragraph(text, Anchor.Auto)
            {
                FontOverride = RegularFont,
                FillColor = new Color(235, 228, 202),
                OutlineOpacity = 0
            };
        }
    }
}
