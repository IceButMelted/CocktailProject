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
        // ---------------------------------------------------------------------
        // UI Constants and Fonts
        // ---------------------------------------------------------------------
        private const int LabelWidth = 28;   // space for text labels
        private const int ValueWidth = 5;    // space for numeric values (0–1000)
        private static readonly Color GoldColor = new Color(192, 130, 30);

        protected Image Img_Summary;
        protected Texture2D T_Img_Summary;

        protected SpriteFont RegularFont;
        protected SpriteFont BoldFont;
        protected SpriteFont ItalicFont;

        // ---------------------------------------------------------------------
        // Background
        // ---------------------------------------------------------------------
        private BG_Parallax bg;
        private Point screenCenter;

        // ---------------------------------------------------------------------
        // Scene Lifecycle
        // ---------------------------------------------------------------------
        public override void Initialize()
        {
            screenCenter = new Point(
                Core.Graphics.PreferredBackBufferWidth / 2,
                Core.Graphics.PreferredBackBufferHeight / 2);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
                Core.Instance.Exit();

            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E))
                Core.ChangeScene(new MainMenu());

            bg.Update(Mouse.GetState());
            UserInterface.Active.Update(gameTime);

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            // Initialize GeonBit UI
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            // Load fonts
            RegularFont = Content.Load<SpriteFont>("Fonts/Regular");
            BoldFont = Content.Load<SpriteFont>("Fonts/Bold");
            ItalicFont = Content.Load<SpriteFont>("Fonts/Italic");

            // Set default UI style colors
            RichParagraphStyleInstruction.AddInstruction(
                "MENU_TEXT",
                new RichParagraphStyleInstruction(fillColor: new Color(235, 228, 202)));

            // Load background and parallax layer
            Texture2D bgTexture = Content.Load<Texture2D>("images/Background/BG_Summary");
            bg = new BG_Parallax(bgTexture, screenCenter, 0.01f, 0.005f, 1.05f);

            // Load main summary panel
            T_Img_Summary = Content.Load<Texture2D>("images/UI/Summary/Summary_Panel");
            Img_Summary = new Image(T_Img_Summary)
            {
                Size = new Vector2(600, 800),
                Anchor = Anchor.TopLeft,
                Offset = new Vector2(130, 140)
            };

            // -----------------------------------------------------------------
            // Title Text
            // -----------------------------------------------------------------
            Paragraph title = new Paragraph("Summary", Anchor.TopCenter)
            {
                Size = new Vector2(500, 60),
                Offset = new Vector2(0, 40),
                FontOverride = BoldFont,
                FillColor = new Color(235, 228, 202),
                OutlineOpacity = 0
            };
           

            // -----------------------------------------------------------------
            // Info Panel
            // -----------------------------------------------------------------
            Panel panel = new Panel(new Vector2(500, 500), PanelSkin.None, Anchor.Center);

            // Format top section: Customer / Income / Tip
            string formattedCustomer = $"{"Customer".PadRight(LabelWidth)}{GlobalVariable.customerNumber,ValueWidth}";
            string formattedIncome = $"{"Income".PadRight(LabelWidth)}{GlobalVariable.Income,ValueWidth}";
            string formattedTip = $"{"Tip".PadRight(LabelWidth)}{GlobalVariable.Tip,ValueWidth}";

            Paragraph txtCustomer = CreateLabel(formattedCustomer);
            Paragraph txtIncome = CreateLabel(formattedIncome);

            // Add main summary lines
            panel.AddChild(txtCustomer);
            panel.AddChild(txtIncome);

            // -----------------------------------------------------------------
            // Cocktail List
            // -----------------------------------------------------------------
            int maxNameLength = GlobalVariable.CocktailHaveDone.Keys.Max(k => k.Length);

            foreach (var kvp in GlobalVariable.CocktailHaveDone)
            {
                string formattedLine = $"   {kvp.Key.PadRight(maxNameLength + 5)}{kvp.Value,5}";
                panel.AddChild(CreateLabel(formattedLine));
            }

            // Add tip line last
            Paragraph txtTip = CreateLabel(formattedTip);
            panel.AddChild(txtTip);

            // Total line
            int grandTotal = GlobalVariable.Income + GlobalVariable.Tip;
            Paragraph txt_Total = new Paragraph($"Total".PadRight(maxNameLength + 12) + $"{grandTotal,5}", Anchor.BottomCenter);
            txt_Total.FontOverride = BoldFont;
            txt_Total.FillColor = new Color(235, 228, 202);
            txt_Total.OutlineOpacity = 0;
            panel.AddChild(txt_Total);

            // -----------------------------------------------------------------
            // Combine UI Elements
            // -----------------------------------------------------------------
            Img_Summary.AddChild(title);
            Img_Summary.AddChild(panel);
            UserInterface.Active.AddEntity(Img_Summary);

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

        // ---------------------------------------------------------------------
        // Helper: Create styled paragraph label
        // ---------------------------------------------------------------------
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
