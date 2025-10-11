using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CocktailProject.ClassCocktail;

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
    class Test_VisualMakingCocktail : Scene
    {
        private CocktailBuilder _currentCocktail;

        private List<Image> AllBars;

        // map model enums -> UI colors (use any colors you like)
        private readonly Dictionary<Enum_Alcohol, Color> AlcoholColors = new Dictionary<Enum_Alcohol, Color>()
        {
            { Enum_Alcohol.Vodka, Color.LightBlue },
            { Enum_Alcohol.Gin, Color.MediumPurple },
            { Enum_Alcohol.Triplesec, Color.Orange },
            { Enum_Alcohol.Vermouth, Color.Goldenrod }
        };

        private readonly Dictionary<Enum_Mixer, Color> MixerColors = new Dictionary<Enum_Mixer, Color>()
        {
            { Enum_Mixer.CanberryJuice, Color.Red },
            { Enum_Mixer.GrapefruitJuice, Color.Pink },
            { Enum_Mixer.LemonJuice, Color.Yellow },
            { Enum_Mixer.Soda, Color.LightGray },
            { Enum_Mixer.Syrup, Color.Brown },
            { Enum_Mixer.PepperMint, Color.Green }
        };

        protected Panel P_MakingCocktailVisual;
        protected Image Img_MainDisplay;
        protected Image Img_Visual01;
        protected Image Img_Visual02;
        protected Image Img_Visual03;
        protected Image Img_Visual04;
        protected Image Img_Visual05;
        protected Image Img_Visual06;
        protected Image Img_Visual07;
        protected Image Img_Visual08;
        protected Image Img_Visual09;
        protected Image Img_Visual10;



        public override void Initialize()
        {

            if (_currentCocktail == null)
            {
                _currentCocktail = new CocktailBuilder();
            }
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            UserInterface.Active.Update(gameTime);

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.ShowCursor = false;


            P_MakingCocktailVisual = new Panel(new Vector2(500, 500), PanelSkin.Simple, Anchor.Center);
            P_MakingCocktailVisual.Padding = new Vector2(0, 0);

            //load and init image
            Img_Visual01 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar01"));
            Img_Visual01.Size = new Vector2(Img_Visual01.Texture.Width, Img_Visual01.Texture.Height);
            Img_Visual01.Anchor = Anchor.BottomCenter;
            Img_Visual01.Offset = new Vector2(-Img_Visual01.Texture.Width / 2, 0);

            Img_Visual02 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar02"));
            Img_Visual02.Size = new Vector2(Img_Visual02.Texture.Width, Img_Visual02.Texture.Height);
            Img_Visual02.Anchor = Anchor.BottomCenter;
            Img_Visual02.Offset = new Vector2(-Img_Visual02.Texture.Width / 2, 0);

            Img_Visual03 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar03"));
            Img_Visual03.Size = new Vector2(Img_Visual03.Texture.Width, Img_Visual03.Texture.Height);
            Img_Visual03.Anchor = Anchor.BottomCenter;
            Img_Visual03.Offset = new Vector2(-Img_Visual03.Texture.Width / 2, 0);

            Img_Visual04 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar04"));
            Img_Visual04.Size = new Vector2(Img_Visual04.Texture.Width, Img_Visual04.Texture.Height);
            Img_Visual04.Anchor = Anchor.BottomCenter;
            Img_Visual04.Offset = new Vector2(-Img_Visual04.Texture.Width / 2, 0);

            Img_Visual05 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar05"));
            Img_Visual05.Size = new Vector2(Img_Visual05.Texture.Width, Img_Visual05.Texture.Height);
            Img_Visual05.Anchor = Anchor.BottomCenter;
            Img_Visual05.Offset = new Vector2(-Img_Visual05.Texture.Width / 2, 0);

            Img_Visual06 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar06"));
            Img_Visual06.Size = new Vector2(Img_Visual06.Texture.Width, Img_Visual06.Texture.Height);
            Img_Visual06.Anchor = Anchor.BottomCenter;
            Img_Visual06.Offset = new Vector2(Img_Visual06.Texture.Width / 2, 0);

            Img_Visual07 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar07"));
            Img_Visual07.Size = new Vector2(Img_Visual07.Texture.Width, Img_Visual07.Texture.Height);
            Img_Visual07.Anchor = Anchor.BottomCenter;
            Img_Visual07.Offset = new Vector2(Img_Visual07.Texture.Width / 2, 0);

            Img_Visual08 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar08"));
            Img_Visual08.Size = new Vector2(Img_Visual08.Texture.Width, Img_Visual08.Texture.Height);
            Img_Visual08.Anchor = Anchor.BottomCenter;
            Img_Visual08.Offset = new Vector2(Img_Visual08.Texture.Width / 2, 0);

            Img_Visual09 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar09"));
            Img_Visual09.Size = new Vector2(Img_Visual09.Texture.Width, Img_Visual09.Texture.Height);
            Img_Visual09.Anchor = Anchor.BottomCenter;
            Img_Visual09.Offset = new Vector2(Img_Visual09.Texture.Width / 2, 0);

            Img_Visual10 = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/Bar10"));
            Img_Visual10.Size = new Vector2(Img_Visual10.Texture.Width, Img_Visual10.Texture.Height);
            Img_Visual10.Anchor = Anchor.BottomCenter;
            Img_Visual10.Offset = new Vector2(Img_Visual10.Texture.Width / 2, 0);

            Img_MainDisplay = new Image(Content.Load<Texture2D>("images/UI/Making_Cocktail_Visual/MakingCocktail_Bar"));
            Img_MainDisplay.Size = new Vector2(Img_MainDisplay.Texture.Width, Img_MainDisplay.Texture.Height);
            Img_MainDisplay.Anchor = Anchor.BottomCenter;
            Img_MainDisplay.Offset = new Vector2(0, 0);

            AllBars = new List<Image>
            {
                Img_Visual01, Img_Visual02, Img_Visual03, Img_Visual04, Img_Visual05,
                Img_Visual06, Img_Visual07, Img_Visual08, Img_Visual09, Img_Visual10
            };

            P_MakingCocktailVisual.AddChild(Img_Visual01);
            P_MakingCocktailVisual.AddChild(Img_Visual02);
            P_MakingCocktailVisual.AddChild(Img_Visual03);
            P_MakingCocktailVisual.AddChild(Img_Visual04);
            P_MakingCocktailVisual.AddChild(Img_Visual05);
            P_MakingCocktailVisual.AddChild(Img_Visual06);
            P_MakingCocktailVisual.AddChild(Img_Visual07);
            P_MakingCocktailVisual.AddChild(Img_Visual08);
            P_MakingCocktailVisual.AddChild(Img_Visual09);
            P_MakingCocktailVisual.AddChild(Img_Visual10);
            P_MakingCocktailVisual.AddChild(Img_MainDisplay);


            UserInterface.Active.AddEntity(P_MakingCocktailVisual);

            // Create a panel to hold all buttons
            Panel SamplePanel = new Panel(new Vector2(500, 900), PanelSkin.Simple, Anchor.AutoInline);

            Button BTN_al_Gin = new Button("Add Gin", ButtonSkin.Default, Anchor.Auto);
            BTN_al_Gin.Size = new Vector2(200, 100);
            BTN_al_Gin.OnClick += (Entity btn) =>
            {    
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Gin, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_al_Gin);

            Button BTN_al_Vodka = new Button("Add Vodka", ButtonSkin.Default, Anchor.AutoInline);
            BTN_al_Vodka.Size = new Vector2(200, 100);
            BTN_al_Vodka.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vodka, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_al_Vodka);

            Button BTN_al_Triplesec = new Button("Add Tonic", ButtonSkin.Default, Anchor.AutoInline);
            BTN_al_Triplesec.Size = new Vector2(200, 100);
            BTN_al_Triplesec.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Triplesec, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_al_Triplesec);

            Button BTN_al_Vermouth = new Button("Add Vermoth", ButtonSkin.Default, Anchor.AutoInline);
            BTN_al_Vermouth.Size = new Vector2(200, 100);
            BTN_al_Vermouth.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vermouth, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_al_Vermouth);

            Button BTN_mx_LemonJuice = new Button("Add Lemon Juice", ButtonSkin.Default, Anchor.AutoInline);
            BTN_mx_LemonJuice.Size = new Vector2(200, 100);
            BTN_mx_LemonJuice.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.LemonJuice, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_mx_LemonJuice);

            Button BTN_mx_CanberryJuice = new Button("Add Canberry Juice", ButtonSkin.Default, Anchor.AutoInline);
            BTN_mx_CanberryJuice.Size = new Vector2(200, 100);
            BTN_mx_CanberryJuice.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.CanberryJuice, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_mx_CanberryJuice);

            Button BTN_mx_GrapefruitJuice = new Button("Add Grapefruit Juice", ButtonSkin.Default, Anchor.AutoInline);
            BTN_mx_GrapefruitJuice.Size = new Vector2(200, 100);
            BTN_mx_GrapefruitJuice.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.GrapefruitJuice, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_mx_GrapefruitJuice);
            
            Button BTN_mx_Soda = new Button("Add Soda", ButtonSkin.Default, Anchor.AutoInline);
            BTN_mx_Soda.Size = new Vector2(200, 100);
            BTN_mx_Soda.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Soda, 1);
                UpdateCocktailBars();
            };

            SamplePanel.AddChild(BTN_mx_Soda);
            Button BTN_mx_Syrup = new Button("Add Syrup", ButtonSkin.Default, Anchor.AutoInline);
            BTN_mx_Syrup.Size = new Vector2(200, 100);
            BTN_mx_Syrup.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Syrup, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_mx_Syrup);

            Button BTN_mx_PepperMint = new Button("Add Pepper Mint", ButtonSkin.Default, Anchor.AutoInline);
            BTN_mx_PepperMint.Size = new Vector2(200, 100);
            BTN_mx_PepperMint.OnClick += (Entity btn) =>
            {
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.PepperMint, 1);
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_mx_PepperMint);

            Button BTN_reset = new Button("Reset Cocktail", ButtonSkin.Default, Anchor.AutoInline);
            BTN_reset.Size = new Vector2(200, 100);
            BTN_reset.OnClick += (Entity btn) =>
            {
                _currentCocktail.ClearAllIngredients();
                UpdateCocktailBars();
            };
            SamplePanel.AddChild(BTN_reset);



            UserInterface.Active.AddEntity(SamplePanel);


            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }

        private void UpdateCocktailBars()
        {
            if (_currentCocktail == null) return;

            // get flattened parts from model (type depends on your model/wrapper)
            List<Cocktail.IngredientPart> parts;

            // if using CocktailBuilder wrapper:
            // parts = _currentCocktail.GetFlattenedParts(10);

            // if using Cocktail directly:
            parts = _currentCocktail.GetFlattenedParts(10);

            for (int i = 0; i < AllBars.Count; i++)
            {
                if (i < parts.Count)
                {
                    var p = parts[i];
                    if (p.IsAlcohol)
                    {
                        Color c = AlcoholColors.ContainsKey(p.Alcohol) ? AlcoholColors[p.Alcohol] : Color.White;
                        AllBars[i].FillColor = c;
                        AllBars[i].Opacity = 255;
                    }
                    else
                    {
                        Color c = MixerColors.ContainsKey(p.Mixer) ? MixerColors[p.Mixer] : Color.White;
                        AllBars[i].FillColor = c;
                        AllBars[i].Opacity = 255;
                    }
                }
                else
                {
                    // empty bars
                    AllBars[i].FillColor = Color.Transparent;
                    // AllBars[i].Opacity = 128; // optional: dim empty bars instead of hide
                }
            }
        }
    }
}
