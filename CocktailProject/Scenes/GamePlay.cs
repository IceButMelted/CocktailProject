using GeonBit.UI.Entities;
using GeonBit.UI;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailProject.ClassCocktail;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.UI.Source.Entities;

namespace CocktailProject.Scenes
{
    class GamePlay : Scene
    {
        #region Cocktail
        private static string str_targetCocktail_Name;
        private Cocktail _targetCoctail = new Cocktail();
        private CocktailBuilder _currentCocktail = new CocktailBuilder();
        #endregion

        #region Image Sprite Atlas
        TextureAtlas Atlas_CustomerNPC;
        TextureAtlas atlas;
        #endregion

        #region UI Logic Variable
        protected bool openAlcoholPanel = false;
        protected bool openMixerPanel = false;
        #endregion

        #region Panel UI
        // Panel
        public Panel P_Ingredient;
            public Button BTN_Mixer;
            public FullImagePanel FP_Mixer;         public Texture2D T_Mixer_Panel;
                public Button BTN_Mixer_CanberryJuice;
                public Button BTN_Mixer_GrapefruitJuice;
                public Button BTN_Mixer_LemonJuice;
                public Button BTN_Mixer_Soda;
                public Button BTN_Mixer_Syrup;
                public Button BTN_Mixer_PepperMint;
            public Button BTN_Alcohol;
            public FullImagePanel FP_Alcohol;        public Texture2D T_Alchohol_Panel;
                public Button BTN_Alcohol_Vodka;
                public Button BTN_Alcohol_Gin;
                public Button BTN_Alcohol_Triplesec;
                public Button BTN_Alcohol_Vermouth;
            public Panel P_MakeingZone; public Texture2D T_MakingZone_Panel;
                public Button BTN_AddIce;
                public Button BTN_Stiring;
                public Button BTN_Shaking;
                public Image Img_CocktailBottle; public TextureAtlas Atlas_Cocktail; public Texture2D T_CocktailBase;
                public Button BTN_Reset_OnTable;
            public Button BTN_Rest_BeforeServe;


        //BG
        public Image Img_BG_Foreground; public Texture2D T_BG_Foreground;
        public Image Img_BG_Background; public Texture2D T_BG_Background;
        #endregion


        public override void Initialize()
        {
            //Add Code Here

            

            //Base DO NOT DELETE
            base.Initialize();
        }

        public override void LoadContent()
        {   //Base DO NOT DELETE
            UserInterface.Initialize(Content, BuiltinThemes.lowres);
            UserInterface.Active.ShowCursor = false;
            SpriteFont myFont = Content.Load<SpriteFont>("Fonts/MyUIFont");

            //Add Code Here
            //Load Image with Batch
            atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
            Atlas_CustomerNPC = TextureAtlas.FromFile(Content, "images/Customer/CustomerNPC_Define.xml");

            //Load Ui image
            T_Alchohol_Panel = Content.Load<Texture2D>("images/UI/Img_Panel_Alcohol");
            T_Mixer_Panel = Content.Load<Texture2D>("images/UI/Img_Panel_Alcohol");

            T_BG_Background = Content.Load<Texture2D>("images/Background/BG_Background");
            T_BG_Foreground = Content.Load<Texture2D>("images/Background/BG_ForeGroun");
            T_CocktailBase = Content.Load<Texture2D>("images/Cocktail/BaseCocktailGlass");

            //Add Code Above

            //This is Base DO NOT DELETE
            InitUI();
            base.LoadContent();
        }

        public void InitUI()
        {


            P_Ingredient = new Panel(new Vector2(800, 600),PanelSkin.None, anchor: Anchor.TopRight, offset: new Vector2(0, 0));
            P_Ingredient.Padding = Vector2.Zero;

#region Panel Alcohol UI
            //P_Alcohol = new Panel(new Vector2(800, 600), PanelSkin.Default, anchor: Anchor.TopRight);
            FP_Alcohol = new FullImagePanel(T_Alchohol_Panel, new Vector2(800, 600), anchor: Anchor.TopRight);
            FP_Alcohol.Offset = new Vector2(-800, 0);
            FP_Alcohol.Padding = Vector2.Zero;

            BTN_Alcohol = new Button("Alcohol", skin: ButtonSkin.Default, anchor: Anchor.TopRight, size: new Vector2(200, 130));
            BTN_Alcohol.Offset = new Vector2(-100, 100);
            BTN_Alcohol.OnMouseDown = (Entity e) =>
            {
                if (openAlcoholPanel)
                    openAlcoholPanel = false;
                else
                    openAlcoholPanel = true;

                openMixerPanel = false;
                Debug.WriteLine("BTN_Alcohol was clicked");
            };
            //P_Alcohol.SetCustomSkin(T_Alchohol_Panel);
            //Add Button Alcohol to P_Alcohol
    #region BTN Alcohol UI
            
                BTN_Alcohol_Vodka = new Button("Vodka", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Alcohol_Vodka.Padding = Vector2.Zero;
                BTN_Alcohol_Vodka.Offset = new Vector2((50*1), 50);
                BTN_Alcohol_Vodka.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vodka, 1);
                        Debug.WriteLine("Added Vodka. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                    
                };

                BTN_Alcohol_Gin = new Button("Gin", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Alcohol_Gin.Padding = Vector2.Zero;
                BTN_Alcohol_Gin.Offset = new Vector2(160+(50*2), 50);
                BTN_Alcohol_Gin.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Gin, 1);
                        Debug.WriteLine("Added Gin. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };

                BTN_Alcohol_Triplesec = new Button("Triplesec", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Alcohol_Triplesec.Padding = Vector2.Zero;
                BTN_Alcohol_Triplesec.Offset = new Vector2(50,160+(50*2));
                BTN_Alcohol_Triplesec.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Triplesec, 1);
                        Debug.WriteLine("Added Triplesec. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };

                BTN_Alcohol_Vermouth = new Button("Vermouth", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Alcohol_Vermouth.Padding = Vector2.Zero;
                BTN_Alcohol_Vermouth.Offset = new Vector2(160 + (50 * 2), 160 + (50 * 2));
                BTN_Alcohol_Vermouth.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vermouth, 1);
                        Debug.WriteLine("Added Vermouth. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };
            
            #endregion

            #endregion

#region Panel Mixer UI
            FP_Mixer = new FullImagePanel(T_Mixer_Panel, new Vector2(800, 600), anchor: Anchor.TopRight);
            FP_Mixer.Offset = new Vector2(-800, 0);
            FP_Mixer.Padding = Vector2.Zero;

            BTN_Mixer = new Button("Mixer", skin: ButtonSkin.Default, anchor: Anchor.TopRight, size: new Vector2(200, 130));
            BTN_Mixer.Offset = new Vector2(-100, 400);
            BTN_Mixer.OnMouseDown = (Entity e) =>
            {
                if (openMixerPanel)
                {
                    openMixerPanel = false;
                }
                else
                    openMixerPanel = true;

                openAlcoholPanel = false;
                Debug.WriteLine("BTN_Mixer was clicked");
            };
    #region BTN Mixer UI
            
                BTN_Mixer_CanberryJuice = new Button("Canberry Juice", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Mixer_CanberryJuice.Padding = Vector2.Zero;
                BTN_Mixer_CanberryJuice.Offset = new Vector2((50 * 1) + (160*0), 50);
                BTN_Mixer_CanberryJuice.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateMixer(Enum_Mixer.CanberryJuice, 1);
                        Debug.WriteLine("Added Canberry Juice. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };

                BTN_Mixer_GrapefruitJuice = new Button("Grapefruit Juice", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Mixer_GrapefruitJuice.Padding = Vector2.Zero;
                BTN_Mixer_GrapefruitJuice.Offset = new Vector2((50 * 2) + (160*1), 50);
                BTN_Mixer_GrapefruitJuice.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateMixer(Enum_Mixer.GrapefruitJuice, 1);
                        Debug.WriteLine("Added Grapefruit Juice. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };

                BTN_Mixer_LemonJuice = new Button("Lemon Juice", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Mixer_LemonJuice.Padding = Vector2.Zero;
                BTN_Mixer_LemonJuice.Offset = new Vector2((50*3) + (160 * 2),50);
                BTN_Mixer_LemonJuice.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateMixer(Enum_Mixer.LemonJuice, 1);
                        Debug.WriteLine("Added Lemon Juice. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };

                //new row
                BTN_Mixer_Soda = new Button("Soda", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Mixer_Soda.Padding = Vector2.Zero;
                BTN_Mixer_Soda.Offset = new Vector2((50 * 1) + (160 * 0), 160 + (50 * 2));
                BTN_Mixer_Soda.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Soda, 1);
                        Debug.WriteLine("Added Soda. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };

                BTN_Mixer_Syrup = new Button("Syrup", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Mixer_Syrup.Padding = Vector2.Zero;
                BTN_Mixer_Syrup.Offset = new Vector2((50 * 2) + (160 * 1), 160 + (50 * 2));
                BTN_Mixer_Syrup.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Syrup, 1);
                        Debug.WriteLine("Added Syrup. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };

                BTN_Mixer_PepperMint = new Button("Pepper Mint", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 160));
                BTN_Mixer_PepperMint.Padding = Vector2.Zero;
                BTN_Mixer_PepperMint.Offset = new Vector2((50 * 3) + (160 * 2), 160 + (50 * 2));
                BTN_Mixer_PepperMint.OnMouseDown = (Entity e) =>
                {
                    if (_currentCocktail.GetCountPart() < 10)
                    {
                        _currentCocktail.AddOrUpdateMixer(Enum_Mixer.PepperMint, 1);
                        Debug.WriteLine("Added Pepper Mint. Current cocktail parts: " + _currentCocktail.GetCountPart());
                        Debug.WriteLine(_currentCocktail.Info());
                    }
                    else
                    {
                        Debug.WriteLine("Cannot add more ingredients. Cocktail is full.");
                    }
                };



            #endregion
            #endregion

    #region Add Child to Panel Alcohol
            //add image to panel
            //Panel Alcohol
            FP_Alcohol.AddChild(BTN_Alcohol_Vodka);
            FP_Alcohol.AddChild(BTN_Alcohol_Gin);
            FP_Alcohol.AddChild(BTN_Alcohol_Triplesec);
            FP_Alcohol.AddChild(BTN_Alcohol_Vermouth);
            #endregion

    #region Add Child to Penel Mixer
            //Panel Mixer
            FP_Mixer.AddChild(BTN_Mixer_CanberryJuice);
            FP_Mixer.AddChild(BTN_Mixer_GrapefruitJuice);
            FP_Mixer.AddChild(BTN_Mixer_LemonJuice);
            FP_Mixer.AddChild(BTN_Mixer_Soda);
            FP_Mixer.AddChild(BTN_Mixer_Syrup);
            FP_Mixer.AddChild(BTN_Mixer_PepperMint);
            #endregion

#region Panel Making Zone

            P_MakeingZone = new Panel(new Vector2(800, 400), PanelSkin.None, anchor: Anchor.TopRight);
            P_MakeingZone.Padding = Vector2.Zero;
            P_MakeingZone.Offset = new Vector2(0,600);

            BTN_Stiring = new Button("Stiring", skin: ButtonSkin.Default, anchor: Anchor.CenterRight, size: new Vector2(150, 60));
            BTN_Stiring.Padding = Vector2.Zero;
            BTN_Stiring.Offset = new Vector2(75, 0);
            BTN_Stiring.ButtonParagraph.OutlineWidth = 0;
            BTN_Stiring.OnMouseDown = (Entity e) =>
            {
                _currentCocktail.UseMethod(Enum_Method.Mixing);
                openAlcoholPanel = false;
                openMixerPanel = false;

                Debug.WriteLine("Selected Method: Stiring");
                Debug.WriteLine(_currentCocktail.Info());

                BTNMethodActive(false);
                BTNMethodVisible(false);
            };

            BTN_Shaking = new Button("Shaking", skin: ButtonSkin.Default, anchor: Anchor.CenterRight, size: new Vector2(150, 60));
            BTN_Shaking.Padding = Vector2.Zero;
            BTN_Shaking.Offset = new Vector2(75, 60+20);
            BTN_Shaking.ButtonParagraph.OutlineWidth = 0;
            BTN_Shaking.OnMouseDown = (Entity e) =>
            {
                _currentCocktail.UseMethod(Enum_Method.Shaking);
                openAlcoholPanel = false;
                openMixerPanel = false;

                Debug.WriteLine("Selected Method: Shaking");
                Debug.WriteLine(_currentCocktail.Info());

                BTNMethodActive(false);
                BTNMethodVisible(false);
            };

            BTN_Reset_OnTable = new Button("Reset", skin: ButtonSkin.Default, anchor: Anchor.Center, size: new Vector2(100, 80));
            BTN_Reset_OnTable.Padding = Vector2.Zero;
            BTN_Reset_OnTable.Offset = new Vector2(-100, 120);
            BTN_Reset_OnTable.ButtonParagraph.OutlineWidth = 0;
            BTN_Reset_OnTable.OnMouseDown = (Entity e) =>
            {
                _currentCocktail.ClearAllIngredients();
                openAlcoholPanel = false;
                openMixerPanel = false;

                Debug.WriteLine("Reset On Table");
                Debug.WriteLine(_currentCocktail.Info());

                BTNIngredeientActive(true);
                BTNMethodVisible(false);
            };

            Img_CocktailBottle = new Image(T_CocktailBase, new Vector2(100, 120), anchor: Anchor.Center);
            Img_CocktailBottle.Offset = new Vector2(-100, 0);


            #endregion
            #region Add Child to Panel Making Zone
            P_MakeingZone.AddChild(BTN_Stiring);
            P_MakeingZone.AddChild(BTN_Shaking);
            P_MakeingZone.AddChild(BTN_Reset_OnTable);
            P_MakeingZone.AddChild(Img_CocktailBottle);

            #endregion


            #region Add Child And Entites 
            //add child
            P_Ingredient.AddChild(FP_Alcohol);
            P_Ingredient.AddChild(FP_Mixer);
            P_Ingredient.AddChild(BTN_Alcohol);
            P_Ingredient.AddChild(BTN_Mixer);

            // add Entity
            Img_BG_Background = new Image(T_BG_Background, new Vector2(1920, 1080), anchor: Anchor.Center);
            Img_BG_Foreground = new Image(T_BG_Foreground, new Vector2(1920, 1080), anchor: Anchor.Center);
            UserInterface.Active.AddEntity(Img_BG_Background);
            UserInterface.Active.AddEntity(Img_BG_Foreground);

            UserInterface.Active.AddEntity(P_Ingredient);
            UserInterface.Active.AddEntity(P_MakeingZone);
#endregion
        }

        public override void Update(GameTime gameTime)
        {

            //Add Code Here
            UpdateUILogic();
            //Add Code Above

            //base DO NOT DELETE
            base.Update(gameTime);
            UserInterface.Active.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            Core.SpriteBatch.Begin();

            //Core.SpriteBatch.Draw(img_Alchohol_Panel, new Vector2(0, 0), Color.White);

            Core.SpriteBatch.End();

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }

        // _____________________main funciton__________________
        protected void UpdateUILogic() {
            CheckCurrentCountPart();
            if (openAlcoholPanel)
            {
                SlidePanel_X_Axis(FP_Alcohol, 0, 20, true);
                SlidePanel_X_Axis(BTN_Alcohol, 0, 20, true);
            }
            else { 
                SlidePanel_X_Axis(FP_Alcohol, -800, 20, false);
                SlidePanel_X_Axis(BTN_Alcohol, -100, 20, false);
            }

            if (openMixerPanel)
            {
                SlidePanel_X_Axis(FP_Mixer, 0, 20, true);
                SlidePanel_X_Axis(BTN_Mixer, 0, 20, true);
            }
            else
            {
                SlidePanel_X_Axis(FP_Mixer, -800, 20, false);
                SlidePanel_X_Axis(BTN_Mixer, -100, 20, false);
            }

        }   

        // ----------------------Fucntion-----------------------
        protected string GetRandomCocktailName()
        {
            if (CocktailDicMaker.CocktailDictionary.Count == 0)
                return string.Empty;

            var random = new Random();
            int randomIndex = random.Next(CocktailDicMaker.CocktailDictionary.Count);
            return CocktailDicMaker.CocktailDictionary.Keys.ElementAt(randomIndex);
        }
        protected void RandomTargetCocktail() {
            str_targetCocktail_Name = GetRandomCocktailName();
            CocktailDicMaker.CocktailDictionary.TryGetValue(str_targetCocktail_Name, out _targetCoctail);
        }
        protected float CalcualatePrice(Cocktail _targetCocktail)
        {

            float _price = 0;
            _price = _targetCocktail.GetPrice();

            if (_targetCocktail.Equals(_currentCocktail))
            {
                _price = _price * 1.2f;
            }
            else
            {
                if (!_targetCocktail.IsSameTypeOfCocktail(_currentCocktail))
                {
                    _price = 0;
                    return _price;
                }
                if (!_targetCocktail.IsSameMethod(_currentCocktail))
                {
                    _price = _price * 0.8f;
                }
                if (!_targetCocktail.IsAddIceBoth(_currentCocktail))
                {
                    _price = _price * 0.8f;
                }
            }

            return _price;
        }
        protected void CheckCurrentCountPart()
        {
            bool method = _currentCocktail.Getmethod() == Enum_Method.None;
            if (_currentCocktail.GetCountPart() == 0)
            {
                BTNMethodActive(false);
                BTNMethodVisible(false);
                BTNIngredeientActive(true);
            }
            if (_currentCocktail.GetCountPart() > 0 && _currentCocktail.GetCountPart() < 10 && method)
            {
                BTNMethodActive(true);
                BTNMethodVisible(true);
                BTNIngredeientActive(true);
            }
            if (_currentCocktail.GetCountPart() == 10 && method)
            {
                BTNMethodActive(true);
                BTNMethodVisible(true);
                BTNIngredeientActive(false);
            }
        }

        protected void BTNIngredeientActive(bool Enable) {
            BTN_Mixer_CanberryJuice.Enabled = Enable;
            BTN_Mixer_GrapefruitJuice.Enabled = Enable;
            BTN_Mixer_LemonJuice.Enabled = Enable;
            BTN_Mixer_Soda.Enabled = Enable;
            BTN_Mixer_Syrup.Enabled = Enable;
            BTN_Mixer_PepperMint.Enabled = Enable;
            BTN_Alcohol_Vodka.Enabled = Enable;
            BTN_Alcohol_Gin.Enabled = Enable;
            BTN_Alcohol_Triplesec.Enabled = Enable;
            BTN_Alcohol_Vermouth.Enabled = Enable;
        }
        protected void BTNMethodActive(bool Enable)
        {
            BTN_Stiring.Enabled = Enable;
            BTN_Shaking.Enabled = Enable;
            BTN_Reset_OnTable.Enabled = Enable;
        }

        protected void BTNMethodVisible(bool visible) {
            BTN_Stiring.Visible = visible;
            BTN_Shaking.Visible = visible;
            BTN_Reset_OnTable.Visible = visible;
        }

        public bool SlidePanel_X_Axis(Entity _panel,int _endPoint, int _speed, bool _moreThan ) {
            if (_moreThan)
            {
                if (_panel.Offset.X < _endPoint)
                {
                    _panel.Offset += new Vector2(_speed, 0);
                    return false;
                }
            }
            else
            {
                if (_panel.Offset.X > _endPoint)
                {
                    _panel.Offset -= new Vector2(_speed, 0);
                    return false;
                }
            }

            return true;
        }
        public bool SlidePanel_Y_Axis(Entity _panel, int _endPoint, int _speed, bool _moreThan)
        {
            if (_moreThan)
            {
                if (_panel.Offset.Y < _endPoint)
                {
                    _panel.Offset += new Vector2(0, _speed);
                    return false;
                }
            }
            else
            {
                if (_panel.Offset.Y > _endPoint)
                {
                    _panel.Offset -= new Vector2(0, _speed);
                    return false;
                }
            }

            return true;
        }
    }
}
