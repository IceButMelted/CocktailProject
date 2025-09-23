using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI.Entities;
using GeonBit.UI.Source.Entities;
using GeonBit.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

using CocktailProject.ClassCocktail;
using CocktailProject.ClassTime;
using CocktailProject.NPC;
using CocktailProject.Class_DialogLogic;
using CocktailProject.MiniGame;



namespace CocktailProject.Scenes
{
    class GamePlay : Scene
    {
        #region Cocktail
        private static string str_targetCocktail_Name;
        private Cocktail _targetCoctail = new Cocktail();
        private CocktailBuilder _currentCocktail = new CocktailBuilder();
        #endregion

        #region NPC
        protected int numbercustomer = 0;
        protected string _NPC_Name;
        protected string _tmp_NPC_Name;
        #endregion  

        #region Image Sprite Atlas
        TextureAtlas Atlas_CustomerNPC;
        TextureAtlas atlas;
        #endregion

        #region Conversation Logic Variable
        public TaggedTextRevealer AnimationText;
        ConversationPhase currentPhase = ConversationPhase.SmallTalkBeforeOrder;

        protected bool canSkipConversation = false;
        protected bool canGoNextConversation = false;
        protected bool haveDoneOrder = false;

        protected bool inStartConversation = false;
        protected bool inOrderConversation = false;
        protected bool inServerComplain = false;
        #endregion

        #region UI Logic Variable
        protected bool openAlcoholPanel = false;
        protected bool openMixerPanel = false;
        protected bool openMinigamePanel = false;
        protected bool openBeforeServePanel = false;
        protected bool openArt1Panel = false;
        protected bool openArt2Panel = false;

        protected bool playingMinigameShaking = false;
        #endregion

        #region Panel UI
        // Panel
        public Panel P_Ingredient;
        public Button BTN_Mixer; public Texture2D T_BTN_Mixer;
        public FullImagePanel FP_Mixer; public Texture2D T_Mixer_Panel;
            public Button BTN_Mixer_CanberryJuice;      public Texture2D T_BTN_Mixer_CanberryJuice;
            public Button BTN_Mixer_GrapefruitJuice;    public Texture2D T_BTN_Mixer_GrapefruitJuice;
            public Button BTN_Mixer_LemonJuice;         public Texture2D T_BTN_Mixer_LemonJuice;
            public Button BTN_Mixer_Soda;               public Texture2D T_BTN_Mixer_Soda;
            public Button BTN_Mixer_Syrup;              public Texture2D T_BTN_Mixer_Syrup;
            public Button BTN_Mixer_PepperMint;         public Texture2D T_BTN_Mixer_PepperMint;
        public Button BTN_Alcohol; public Texture2D T_BTN_Alchol;
        public FullImagePanel FP_Alcohol; public Texture2D T_Alchohol_Panel;
            public Button BTN_Alcohol_Vodka;            public Texture2D T_BTN_Alcohol_Vodka;
            public Button BTN_Alcohol_Gin;              public Texture2D T_BTN_Alcohol_Gin;
            public Button BTN_Alcohol_Triplesec;        public Texture2D T_BTN_Alcohol_Triplesec;
            public Button BTN_Alcohol_Vermouth;         public Texture2D T_BTN_Alcohol_Vermouth;
        public Panel P_MakeingZone; public Texture2D T_MakingZone_Panel;
            public Button BTN_Stiring;
            public Button BTN_Shaking;
        public Image Img_CocktailBottle; public TextureAtlas Atlas_Cocktail; public Texture2D T_CocktailBase;
            public Button BTN_Reset_OnTable;
        public Panel P_BeforeServe;
            public Button BTN_AddIce;
            public Button BTN_Serve;
            public Button BTN_Rest_BeforeServe;
        public Panel P_Minigame;
            public Panel P_Minigame_Shaking;
                public Panel BG_ProgressBar;
                public Panel ProgressBar;
                public Panel BG_TargetZone;
                public Panel TargetZone;
                public Panel Pointing;
            public Panel P_Minigame_Stiring;

        public Image Img_Customer;

        public Panel P_OrderPanel;
        public RichParagraph RP_ConversationCustomer;

#if DEBUG
        public Paragraph P_Debug_targetCocktail;
        public Paragraph P_Debug_CurrentCocktail;

#endif

        //BG
        public Image Img_BG_Foreground; public Texture2D T_BG_Foreground;
        public Image Img_BG_Midground; public Texture2D T_BG_Midgroud;
        public Image Img_BG_Background; public Texture2D T_BG_Background;
        #endregion


        public override void Initialize()
        {
            //Add Code Here

            RandomTargetCocktail();
            _NPC_Name = RandomNPC();
           
            

            Debug.WriteLine("Name : " + str_targetCocktail_Name + "\n" + _targetCoctail.Info());
            
            string welcomeText = "Welcome! To Project Cocktail";

            inStartConversation = true;
            canSkipConversation = true;

            AnimationText = new TaggedTextRevealer(welcomeText, 0.05);
            AnimationText.Start();


            //Base DO NOT DELETE
            base.Initialize();
        }

        public override void LoadContent()
        {   //Base DO NOT DELETE 
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.ShowCursor = false;

            SpriteFont myFont = Content.Load<SpriteFont>("Fonts/MyUIFont");

            //Add Code Here
            //Load Image with Batch

            LoadImageAndAtlas();
            //Add Code Above

            //This is Base DO NOT DELETE
            InitUI();
            base.LoadContent();
        }

        public void LoadImageAndAtlas()
        {
            atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
            Atlas_CustomerNPC = TextureAtlas.FromFile(Content, "images/Customer/CustomerNPC_Define.xml");

            //Load Ui image
            T_Alchohol_Panel = Content.Load<Texture2D>("images/UI/Shelf");
            T_Mixer_Panel = Content.Load<Texture2D>("images/UI/Shelf");

            T_BTN_Alchol = Content.Load<Texture2D>("images/UI/BTN_Icon_Alcohol");
            T_BTN_Mixer = Content.Load<Texture2D>("images/UI/BTN_Icon_Mixer");

            T_BG_Background = Content.Load<Texture2D>("images/Background/BG_Background");
            T_BG_Midgroud = Content.Load<Texture2D>("images/Background/BG_MidGround");
            T_BG_Foreground = Content.Load<Texture2D>("images/Background/BG_ForeGroun");

            //load image button alcohol
            T_BTN_Alcohol_Gin = Content.Load<Texture2D>("images/UI/Alcohol/Gin_160x160");
            T_BTN_Alcohol_Vodka = Content.Load<Texture2D>("images/UI/Alcohol/Vodka_160x160");
            T_BTN_Alcohol_Triplesec = Content.Load<Texture2D>("images/UI/Alcohol/Triplesec_160x160");
            T_BTN_Alcohol_Vermouth = Content.Load<Texture2D>("images/UI/Alcohol/Vermouth_160x160");



            T_CocktailBase = Content.Load<Texture2D>("images/Cocktail/BaseCocktailGlass");
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

            BTN_Alcohol = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopRight, size: new Vector2(198, 128));
            BTN_Alcohol.Offset = new Vector2(0, 172);
            BTN_Alcohol.SetCustomSkin(T_BTN_Alchol, T_BTN_Alchol, T_BTN_Alchol);
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
                BTN_Alcohol_Vodka.SetCustomSkin(T_BTN_Alcohol_Vodka, T_BTN_Alcohol_Vodka, T_BTN_Alcohol_Vodka);
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
                BTN_Alcohol_Gin.SetCustomSkin(T_BTN_Alcohol_Gin, T_BTN_Alcohol_Gin, T_BTN_Alcohol_Gin);
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
                BTN_Alcohol_Triplesec.SetCustomSkin(T_BTN_Alcohol_Triplesec, T_BTN_Alcohol_Triplesec, T_BTN_Alcohol_Triplesec);
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
                BTN_Alcohol_Vermouth.SetCustomSkin(T_BTN_Alcohol_Vermouth, T_BTN_Alcohol_Vermouth, T_BTN_Alcohol_Vermouth);
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

            BTN_Mixer = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopRight, size: new Vector2(198, 128));
            BTN_Mixer.Offset = new Vector2(0, 400);
            BTN_Mixer.SetCustomSkin(T_BTN_Mixer, T_BTN_Mixer, T_BTN_Mixer);
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

                ShowMinigame(Enum_Method.Mixing);

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

                ShowMinigame(Enum_Method.Shaking);

                playingMinigameShaking = true;
                ShakingMinigame.StartGame();

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

                ShowMinigame(false);

                BTNIngredeientActive(true);
                BTNMethodVisible(false);

                ResetUI();
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

#region Panel Minigame Zone
            P_Minigame = new Panel(new Vector2(800, 600), PanelSkin.None, anchor: Anchor.TopRight);
            P_Minigame.Padding = Vector2.Zero;
            P_Minigame.Offset = new Vector2(-800,0);

            P_Minigame_Shaking = new Panel(new Vector2(800, 600), PanelSkin.Fancy, anchor: Anchor.TopRight);
            P_Minigame_Shaking.Padding = Vector2.Zero;
            P_Minigame_Shaking.Offset = new Vector2(0, 0);
            InitShakingMinigameUI();

            P_Minigame_Stiring = new Panel(new Vector2(800, 600), PanelSkin.Fancy, anchor: Anchor.TopRight);
            P_Minigame_Stiring.Padding = Vector2.Zero;
            P_Minigame_Stiring.Offset = new Vector2(0, 0);
            P_Minigame_Shaking.FillColor = Color.Red;


            #endregion

    #region Add Child To Panel Minigame Zone

            P_Minigame.AddChild(P_Minigame_Shaking);
            P_Minigame.AddChild(P_Minigame_Stiring);

            #endregion

#region Before Serve Panel
            P_BeforeServe = new Panel(new Vector2(800, 480), PanelSkin.Default, anchor: Anchor.TopRight);
            P_BeforeServe.Padding = Vector2.Zero;
            P_BeforeServe.Offset = new Vector2(-600, 600);

            BTN_AddIce = new Button("Add Ice", skin: ButtonSkin.Default, anchor: Anchor.AutoCenter, size: new Vector2(150, 60));
            BTN_AddIce.Padding = Vector2.Zero;
            BTN_AddIce.Offset = Vector2.Zero;
            BTN_AddIce.ButtonParagraph.OutlineWidth = 0;
            BTN_AddIce.OnMouseDown = (Entity e) =>
            {
                _currentCocktail.AddIce(true);
                Debug.WriteLine("Added Ice");
                Debug.WriteLine(_currentCocktail.Info());
                BTN_AddIce.Enabled = false;
            };

            BTN_Serve = new Button("Serve", skin: ButtonSkin.Default, anchor: Anchor.AutoCenter, size: new Vector2(150, 60));
            BTN_Serve.Padding = Vector2.Zero;
            BTN_Serve.Offset = Vector2.Zero;
            BTN_Serve.ButtonParagraph.OutlineWidth = 0;
            BTN_Serve.OnMouseDown = (Entity e) =>
            {
                _currentCocktail.SetTypeOfCocktailBySearch();
                Debug.WriteLine("Served Cocktail");
                Debug.WriteLine(_currentCocktail.Info());
                float price = CalcualatePrice(_targetCoctail);
                Debug.WriteLine("Earned Price: " + price);

                //_currentCocktail.ClearAllIngredients();
                //RandomTargetCocktail();\

                //--------- from conversation phase Order to Small Talk After Order --------------
                //apply text and change conversation phase
                Debug.WriteLine("Cocktail Served! Small Talk About Cocktail.");

                if (CalculateAccurateCocktail() == Enum_CocktaillResualt.Success)
                {
                    AnimationText = new TaggedTextRevealer("Thanks for the {{RED}}" + str_targetCocktail_Name + "{{WHITE}}, it was great!", 0.05);
                    Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(_NPC_Name + "_happy").SourceRectangle;
                }
                else if (CalculateAccurateCocktail() == Enum_CocktaillResualt.Aceptable)
                {
                    AnimationText = new TaggedTextRevealer("This is not {{ORANGE}}" + str_targetCocktail_Name + "{{WHITE}} i knew but it was okay I guess.", 0.05);
                    Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(_NPC_Name + "_default").SourceRectangle;
                }
                else
                {
                    AnimationText = new TaggedTextRevealer("Ugh, this is not  {{BLUE}}" + str_targetCocktail_Name + "{{WHITE}}, i have ordered ", 0.05);
                    Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(_NPC_Name + "_upset").SourceRectangle;
                }
                canSkipConversation = true;
                canGoNextConversation = false;
                haveDoneOrder = false; // reset
                openBeforeServePanel = false;
                currentPhase = ConversationPhase.SmallTalkAfterOrder;
                AnimationText.Start();
                //-------------------------------------------------------------------------------

                Debug.WriteLine("New Target Cocktail is: " + str_targetCocktail_Name);
                BTN_AddIce.Enabled = true;

                BTNIngredeientActive(true);
                BTNMethodVisible(false);

                haveDoneOrder = true;
                

                ResetUI();
            };

            BTN_Rest_BeforeServe = new Button("Reset", skin: ButtonSkin.Default, anchor: Anchor.AutoCenter, size: new Vector2(100, 80));
            BTN_Rest_BeforeServe.Padding = Vector2.Zero;
            BTN_Rest_BeforeServe.Offset = Vector2.Zero;
            BTN_Rest_BeforeServe.ButtonParagraph.OutlineWidth = 0;
            BTN_Rest_BeforeServe.OnMouseDown = (Entity e) =>
            {
                _currentCocktail.ClearAllIngredients();
                openAlcoholPanel = false;
                openMixerPanel = false;

                ResetUI();

            };
            #endregion

    #region Add Child Before Serve
            P_BeforeServe.AddChild(BTN_AddIce);
            P_BeforeServe.AddChild(BTN_Serve);
            P_BeforeServe.AddChild(BTN_Rest_BeforeServe);

            #endregion


#region Oreder Panel
            P_OrderPanel = new Panel(new Vector2(500, 200), PanelSkin.Default, anchor: Anchor.CenterLeft);
            P_OrderPanel.Padding = Vector2.Zero;
            P_OrderPanel.Offset = new Vector2(400, 225);

            RP_ConversationCustomer = new RichParagraph("Welcome! Please make me a cocktail.", anchor: Anchor.Center, size: new Vector2(470, 100));
            RP_ConversationCustomer.OutlineWidth = 0;
            #endregion


#if DEBUG
            Button FinishString = new Button("Finish Stiring", skin: ButtonSkin.Default, Anchor.Center, new Vector2(200, 100));
            FinishString.OnMouseDown = (Entity e) =>
            {
                Debug.WriteLine("Finish Stiring Minigame");
                openBeforeServePanel = true;
                //openMinigamePanel = false;
                //P_Minigame_Stiring.Offset = new Vector2(-700, 0);
            };

            Button FinishShake = new Button("Finish Shaking", skin: ButtonSkin.Default, Anchor.Center, new Vector2(200, 100));
            FinishShake.OnMouseDown = (Entity e) =>
            {
                Debug.WriteLine("Finish Shaking Minigame");
                openBeforeServePanel = true;
                //openMinigamePanel = false;
                //P_Minigame_Shaking.Offset = new Vector2(-700, 0);
            };

            //P_Minigame_Shaking.AddChild(FinishShake);
            P_Minigame_Stiring.AddChild(FinishString);


            P_Debug_targetCocktail = new Paragraph("Target Cocktail: " + str_targetCocktail_Name + _targetCoctail.Info(), anchor: Anchor.TopLeft, size: new Vector2(300, 500));
            P_Debug_targetCocktail.Offset = new Vector2(0, 0);

            P_Debug_CurrentCocktail = new Paragraph("Current Cocktail: " + _currentCocktail.Info(), anchor: Anchor.TopLeft, size: new Vector2(300, 500));
            P_Debug_CurrentCocktail.Offset = new Vector2(300, 0);
#endif
            #region Add Child Order Panel

            P_OrderPanel.AddChild(RP_ConversationCustomer);

            #endregion

#region Customer Image
        
            Img_Customer = new Image(Atlas_CustomerNPC.GetRegion(_NPC_Name+"_default").GetTexture2D(), new Vector2(450, 650), anchor: Anchor.CenterLeft);
            Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(_NPC_Name + "_default").SourceRectangle;
            Img_Customer.Offset = new Vector2(450, -150);

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
            Img_BG_Midground = new Image(T_BG_Midgroud, new Vector2(1920, 1080), anchor: Anchor.Center);

            UserInterface.Active.AddEntity(Img_BG_Background);
            UserInterface.Active.AddEntity(Img_BG_Midground);   
            UserInterface.Active.AddEntity(Img_Customer);
            UserInterface.Active.AddEntity(Img_BG_Foreground);
#if DEBUG
            UserInterface.Active.AddEntity(P_Debug_CurrentCocktail);
            UserInterface.Active.AddEntity(P_Debug_targetCocktail);
#endif 

            UserInterface.Active.AddEntity(P_Ingredient);
            UserInterface.Active.AddEntity(P_MakeingZone);
            UserInterface.Active.AddEntity(P_Minigame);
            UserInterface.Active.AddEntity(P_BeforeServe);
            UserInterface.Active.AddEntity(P_OrderPanel);
            #endregion
        }

        
        public override void Update(GameTime gameTime)
        {

            //Add Code Here
            UpdateUILogic();

            //update minigame Shaking
            if (playingMinigameShaking) { 
                ShakingMinigame.Update(gameTime);
                UpdateMiniGameShakingUI();
                if (ShakingMinigame.IsComplete()) {
                    ShakingMinigame.Stop();
                    playingMinigameShaking = false;
                    openBeforeServePanel = true;
                    //playingMinigameShaking = false;
                }
            }


            UpdateConversation();
            AnimationText.Update(gameTime);
            //Add Code Above

#if DEBUG
            P_Debug_CurrentCocktail.Text = "Current Cocktail: \n" + _currentCocktail.Info();
            P_Debug_targetCocktail.Text = "Target Cocktail: " + str_targetCocktail_Name + "\n" + _targetCoctail.Info();
#endif

            if (numbercustomer > 6) {
                Core.ChangeScene(new Scenes.Thanks());
            }

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

            //update panel
            HandlePanel_X_Axis(openAlcoholPanel, FP_Alcohol, 0, -800, 20);
            HandlePanel_X_Axis(!openAlcoholPanel, BTN_Alcohol, -25, -125, 20);

            HandlePanel_X_Axis(openMixerPanel, FP_Mixer, 0, -800, 20);
            HandlePanel_X_Axis(!openMixerPanel, BTN_Mixer, -25, -125, 20);

            HandlePanel_X_Axis(openMinigamePanel, P_Minigame, 0, -800, 20);

            HandlePanel_X_Axis(openBeforeServePanel, P_BeforeServe, 0, -800, 20);

        }
        protected void UpdateConversation()
        {
            RP_ConversationCustomer.Text = AnimationText.GetVisibleText();

            // --- Skip text animation if left-click while animating ---
            if (!AnimationText.IsFinished() &&
                Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left) &&
                canSkipConversation)
            {
                AnimationText.Skip();
                canSkipConversation = false;
                canGoNextConversation = true;
                return;
            }

            // --- When text animation finishes by itself ---
            if (AnimationText.IsFinished())
            {
                canSkipConversation = false;
                canGoNextConversation = true;
            }

            // --- Handle advancing conversation ---
            if (canGoNextConversation && Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
            {
                switch (currentPhase)
                {
                    case ConversationPhase.SmallTalkBeforeOrder:
                        // Move into ordering phase
                        Debug.WriteLine("Go Next Conversation (Now Ordering Cocktail)");
                        _currentCocktail.ClearAllIngredients();
                        AnimationText = new TaggedTextRevealer("Please make me a {{RED}}" + str_targetCocktail_Name + "{{WHITE}}.", 0.05);
                        AnimationText.Start();
                        canSkipConversation = true;
                        canGoNextConversation = false;
                        currentPhase = ConversationPhase.Ordering;
                        break;

                    case ConversationPhase.Ordering:
                        if (haveDoneOrder) // only advance after serving cocktail
                        {
                            //Debug.WriteLine("Cocktail Served! Small Talk About Cocktail.");
                            //AnimationText = new TaggedTextRevealer("Thanks for the {{RED}}" + str_targetCocktail_Name + "{{WHITE}}, it was great!", 0.05);
                            //AnimationText.Start();
                            //canSkipConversation = true;
                            //canGoNextConversation = false;
                            //haveDoneOrder = false; // reset
                            //currentPhase = ConversationPhase.SmallTalkAfterOrder;
                        }
                        break;

                    case ConversationPhase.SmallTalkAfterOrder:
                        // Loop back to small talk before order
                        Debug.WriteLine("Looping back to Small Talk Before Order.");
                        AnimationText = new TaggedTextRevealer("So, how's your day going?", 0.05);
                        AnimationText.Start();
                        canSkipConversation = true;
                        canGoNextConversation = false;
                        currentPhase = ConversationPhase.SmallTalkBeforeOrder;
                        RandomTargetCocktail();
                        //set new npc image

                        _NPC_Name = RandomNPC();
                        numbercustomer++;
                        Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(_NPC_Name + "_default").SourceRectangle;
                        break;
                }
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
        protected string RandomNPC() {
            Random random = new Random();
            int numberNPC = random.Next(1,5);
            string _NPC_Name = "NPC_0" + (int)numberNPC;
            return _NPC_Name;
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
                if (!_targetCocktail.IsSameMethod(_currentCocktail)! || _targetCocktail.IsAddIceBoth(_currentCocktail))
                {
                    _price = _price * 0.8f;
                }
            }

            return _price;
        }
        protected Enum_CocktaillResualt CalculateAccurateCocktail() { 
            if (_targetCoctail.Equals(_currentCocktail))
                return Enum_CocktaillResualt.Success;
            else if (!_targetCoctail.IsSameTypeOfCocktail(_currentCocktail))
            {
                return Enum_CocktaillResualt.Fail;
            }
            else if (!_targetCoctail.IsSameMethod(_currentCocktail) || !_targetCoctail.IsAddIceBoth(_currentCocktail))
            {
                return Enum_CocktaillResualt.Aceptable;
            }
            return Enum_CocktaillResualt.Fail;

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
        protected void ShowMinigame(Enum_Method method) {
            openMinigamePanel = true;

            if (method == Enum_Method.Shaking) { 
                P_Minigame.Enabled = true;
                P_Minigame.Visible = true;

                P_Minigame_Stiring.Enabled = false;
                P_Minigame_Stiring.Visible = false;

                P_Minigame_Shaking.Enabled = true;
                P_Minigame_Shaking.Visible = true;
            }
            if (method == Enum_Method.Mixing)
            {
                P_Minigame.Enabled = true;
                P_Minigame.Visible = true;

                P_Minigame_Stiring.Enabled = true;
                P_Minigame_Stiring.Visible = true;

                P_Minigame_Shaking.Enabled = false;
                P_Minigame_Shaking.Visible = false;
            }
        }
        protected void ShowMinigame(bool enable) { 
            
        }
        protected void ResetUI()
        {
            openAlcoholPanel = false;
            openMixerPanel = false;
            openMinigamePanel = false;
            openBeforeServePanel = false;
            BTNIngredeientActive(true);
            BTNMethodActive(false);
            BTNMethodVisible(false);
            _currentCocktail.ClearAllIngredients();
        }
        //-------------------------Conversation---------------------
        protected void SetNewTextForConversation(TaggedTextRevealer _animationText,string _txt) {
            _animationText = new TaggedTextRevealer(_txt, 0.05);
        }
        //-------------------------Mini Gaem---------------------
        public void InitShakingMinigameUI()
        {
            int SizeBar = 600;

            BG_TargetZone = new Panel(new Vector2(50, SizeBar), PanelSkin.Simple, Anchor.CenterLeft);
            BG_TargetZone.FillColor = Color.DarkGray;
            BG_TargetZone.Offset = new Vector2(0, 0);
            BG_TargetZone.Padding = Vector2.Zero;

            BG_ProgressBar = new Panel(new Vector2(50, SizeBar), PanelSkin.Simple, Anchor.CenterLeft);
            BG_ProgressBar.FillColor = Color.Blue;
            BG_ProgressBar.Offset = new Vector2(50, 0);
            BG_ProgressBar.Padding = Vector2.Zero;

            ProgressBar = new Panel(new Vector2(40, 10), PanelSkin.Simple, Anchor.BottomCenter);
            ProgressBar.FillColor = Color.Green;

            TargetZone = new Panel(new Vector2(40, 50), PanelSkin.Simple, Anchor.BottomCenter);
            TargetZone.FillColor = Color.Red;
            TargetZone.Opacity = 100;

            Pointing = new Panel(new Vector2(40, 10), PanelSkin.Simple, Anchor.BottomCenter);

            BG_TargetZone.AddChild(TargetZone);
            BG_TargetZone.AddChild(Pointing);
            P_Minigame_Shaking.AddChild(BG_TargetZone);

            BG_ProgressBar.AddChild(ProgressBar);
            BG_TargetZone.AddChild(BG_ProgressBar);
        }
        public void UpdateMiniGameShakingUI()
        {
            int SizeBar = 600;

            if (ShakingMinigame.CurrentValue != 0)
                Pointing.Size = new Vector2(40, (int)(SizeBar * (ShakingMinigame.CurrentValue / 100)));
            else
                Pointing.Size = new Vector2(40, 1);

            TargetZone.Size = new Vector2(40, (int)(SizeBar * (ShakingMinigame.TargetZone_CurrentSize / 100)));

            //cal offset targetZone
            int offsetX = (int)((ShakingMinigame.InitTargetZone) - (ShakingMinigame.TargetZone_CurrentSize / 2));

            TargetZone.Offset = new Vector2(0, (int)(SizeBar * (offsetX) / 100));

            ProgressBar.Size = new Vector2(40, (int)(SizeBar * (ShakingMinigame.ProgressBar_CurrentValue / 100)));

            //if (ShakingMinigame.IsComplete())
            //    ShakingMinigame.Reset();
            //Debug.WriteLine(ShakingMinigame.CurrentValue);
        }


        // ----------------------Slide Panel-----------------------
        private void HandlePanel_X_Axis(bool isOpen, Entity panel, int openEndPoint, int closedEndPoint, int speed)
        {
            if (isOpen)
                SlidePanel_X_Axis(panel, openEndPoint, speed, true);
            else
                SlidePanel_X_Axis(panel, closedEndPoint, speed, false);
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
