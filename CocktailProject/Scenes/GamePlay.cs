using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using CocktailProject.ClassCocktail;
using CocktailProject.ClassMotion;
using CocktailProject.ClassTime;
using CocktailProject.MiniGame;
using CocktailProject.ClassNPC;
using CocktailProject.Utilities;

using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Source.Entities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using System.Text.Json;
using System.Collections;




namespace CocktailProject.Scenes
{
    class GamePlay : Scene
    {
        #region Cocktail
        private static string str_targetCocktail_Name;
        private Cocktail _targetCoctail = new Cocktail();
        private static string str_currentCocktail_Name;
        private CocktailBuilder _currentCocktail = new CocktailBuilder();
        private Queue<Enum_TextType> ListTextTypes = new Queue<Enum_TextType>();
        private Enum_CocktaillResualt cocktaillResualt = Enum_CocktaillResualt.None;
        private Dictionary<string, string> ComplexTextCocktail = new Dictionary<string, string> {
            { "Cosmopolitan"    ,"'Tart and lightly sweet'" },
            { "Martini"         ,"'Dry crisp and aromatic'" },
            { "White Lady"      ,"'Smooth but Sharp'" },
            { "Gin Fizz"        ,"'Bright and refreshing'" },
            { "Greyhound"       ,"'Clean and slightly bitter'" },
            { "Sea Breeze"      ,"'Cool and breezy refreshing'" },
            { "Nojito"          ,"'Sweet and sparklingly refreshing'" },
            { "Cranberry Fizz"  ,"'Tangy-sweet and Light'" },
            { "Grapefruit Spritz","'Gentle bittersweetness and Brightened'"}
        };
        #endregion

        #region NPC
        protected byte Day = GlobalVariable.Day;
        protected bool shouldEndDay = false;
        protected int numbercustomer = GlobalVariable.customerNumber;
        protected List<BaseCharacter> Customers = new List<BaseCharacter>();

        #endregion

        #region Image Sprite Atlas
        TextureAtlas Atlas_CustomerNPC;
        TextureAtlas Recipes_Atlas;
        TextureAtlas ArtAfterServe_Atlas;
        TextureAtlas atlas;
        TextureAtlas Atlas_BGNPC;
        #endregion

        #region SoundVariable
        Song BGM_themeSong01;
        Song BGM_themeSong02;
        bool shouldPlayBGM_themeSong01 = true;
        bool shouldPlayBGM_themeSong02 = false;

        SoundEffect SFX_PressedBTN;
        SoundEffect SFX_Welcome;
        SoundEffect SFX_Serve;
        SoundEffect SFX_Shaking;
        float cooldownTime_SFX_Shaking = 2f;
        bool canPlaySFX_Shaking = true;
        SoundEffect SFX_Stiring;
        float cooldownTime_SFX_Stiring = 1.2f;
        bool canPlaySFX_Stiring = true;
        SoundEffect SFX_Pouring;
        SoundEffect SFX_Peppermint;
        SoundEffect SFX_AddIce;
        SoundEffect SFX_Lemon;
        SoundEffect SFX_Book_Turnpage;
        SoundEffect SFX_Book_Open_Close;
        SoundEffect SFX_Open_Interface;

        
        #endregion

        #region Conversation Logic Variable

        public bool EndTutorial = false;

        public TaggedTextRevealer AnimationText;
        ConversationPhase currentPhase = ConversationPhase.SmallTalkBeforeOrder;

        protected bool canDoconversation = false;

        protected bool canSkipConversation = false;
        protected bool canGoNextConversation = false;
        protected bool haveDoneOrder = false;
        protected bool inStartConversation = false;

        protected bool openAlcoholPanel = false;
        protected bool openMixerPanel = false;

        protected Enum_PanelState openMinigamePanel = Enum_PanelState.InitPosWarp;
        protected Enum_PanelState stateBeforeServePanel = Enum_PanelState.InitPosWarp;
        protected Enum_PanelState stateArtAfterServePanel = Enum_PanelState.InitPosWarp;
        protected Enum_PanelState stateCocktailResultPanel = Enum_PanelState.InitPosWarp;
        protected Enum_PanelState stateImgCustomer = Enum_PanelState.InitPosWarp;
        protected Enum_CutomerState currentCustomerState = Enum_CutomerState.None;
        protected float timeToCloseBeforeAndAfteServePanel = 3f;

        protected Enum_MiniGameType currentMinigame = Enum_MiniGameType.None;

        protected int TextUIOffeset_BTN = -30;

        //Book recipe
        public int CurrentPage = 1;
        public int TotalPages = 0;

        //Sprite Font
        protected SpriteFont RegularFont;
        protected SpriteFont BoldFont;
        protected SpriteFont ItalicFont;

        //cocktail bar
        int XSizeBar_Stiring = 800;
        int PaddingLR_Bar_Stiring = 50;

        //fade panel
        private float fadeTimer = 0f;
        private bool shouldFadeIn = true;
        private bool shouldFadeOut = false;

        //visual Cocktail on table
        private List<Image> AllBars;
        private readonly Dictionary<Enum_Alcohol, Color> AlcoholColors = new Dictionary<Enum_Alcohol, Color>()
        {
            { Enum_Alcohol.Vodka, Color.OrangeRed },
            { Enum_Alcohol.Gin, Color.OrangeRed },
            { Enum_Alcohol.Triplesec, Color.OrangeRed },
            { Enum_Alcohol.Vermouth, Color.OrangeRed }
        };
        private readonly Dictionary<Enum_Mixer, Color> MixerColors = new Dictionary<Enum_Mixer, Color>()
        {
            { Enum_Mixer.CanberryJuice, Color.Olive },
            { Enum_Mixer.GrapefruitJuice, Color.Olive },
            { Enum_Mixer.LemonJuice, Color.Olive },
            { Enum_Mixer.Soda, Color.Olive },
            { Enum_Mixer.Syrup, Color.Olive },
            { Enum_Mixer.PepperMint, Color.Olive }
        };
        #endregion

        #region Panel UI
        //Panel Main Game
        public Panel P_MainGame;
        // Panel ingredient
        public Panel P_Ingredient;
        // Mixer
        public Button BTN_Mixer; 
        public FullImagePanel FP_Mixer; 
        public Button BTN_Mixer_CanberryJuice;
        public Button BTN_Mixer_GrapefruitJuice;
        public Button BTN_Mixer_LemonJuice;
        public Button BTN_Mixer_Soda;
        public Button BTN_Mixer_Syrup;
        public Button BTN_Mixer_PepperMint;

        // Alcohol
        public Button BTN_Alcohol; 
        public FullImagePanel FP_Alcohol; 
        public Button BTN_Alcohol_Vodka;
        public Button BTN_Alcohol_Gin;
        public Button BTN_Alcohol_Triplesec; 
        public Button BTN_Alcohol_Vermouth;
       
        // Making Zone
        public Panel P_MakeingZone;
        public Button BTN_Stiring;
        public Button BTN_Shaking;
        public Image Img_CocktailBottle;
        public Button BTN_Reset_OnTable;
        public Button BTN_BookRecipes;
        // Before Serve
        
        public Image P_BeforeServe; 
            Texture2D T_P_BeforeServe;  
            Texture2D T_P_BeforeServe_Addice;
        public Button BTN_AddIce;
        public Button BTN_Serve;
        public Button BTN_Rest_BeforeServe;
        // Minigame
        public Panel P_Minigame;
        // Minigame Shaking
        public Panel P_Minigame_Shaking;
        public Image Img_Minigame_Shaking; 
            TextureAtlas MiniGame_Shaking_Atlas; 
            AnimatedSprite Shaking_Anim;
        public Panel BG_ProgressBar;
        public Panel ProgressBar;
        public Panel BG_TargetZone;
        public Panel TargetZone;
        public Panel Pointing;
        // Minigame Stiring
        public Panel P_Minigame_Stirring;
        public Image Img_MiniGame_Stirring; 
            TextureAtlas MinGame_Stirring_Atlas; 
            AnimatedSprite Stirring_Anim;
        public CustomProgressBar PB_Stirring;
        public Panel BG_Stirring_TargetZone;
        public Image Img_Stirring_TargetZone;
        public Image Arrow_Stirring;
        // Book Recipe
        public Panel P_BGBookRecipes;
        public Image Img_BookRecipes;
        public Image Img_LeftPage;
        public Image Img_RightPage;
        public Button BTN_PreviousPage;
        public Button BTN_NextPage;
        // Art After Serve Panel
        public Panel P_ArtAfterServe;
        public Image Img_Art1; 
        public Image Img_Art2; 
        //visual Cocktail on table
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
        // Image Cocktil Resuilt
        protected Image Img_CocktailResult;  
            TextureAtlas CocktailResult_Atlas;
        // Image Mouse 
        protected Image Img_Mouse;      
            TextureAtlas Mouse_Atlas; 
            AnimatedSprite Mouse_Anim;
        // Fading Close Visual
        protected Panel P_Fade;
        protected RichParagraph RP_Fade;

        // customer and order panel
        public Image Img_Customer;
        public RichParagraph RP_CustomerName;
        public Image ImgP_OrderPanel;
        public RichParagraph RP_ConversationCustomer;

#if DEBUG
        public Paragraph P_Debug_targetCocktail;
        public Paragraph P_Debug_CurrentCocktail;
        public Paragraph P_Debug_GlobalVariable;
#endif

        //BG
        public Image Img_BG_Foreground;
        public Image Img_BG_Midground;
        public Image Img_BG_Background;
        #endregion

        #region BG NPC
        public Image Img_BG_NPC; 
        private List<BG_NPC> movingnpcs = new List<BG_NPC>();
        private int npcCount = 8; //NPC Counts
        #endregion

        public override void Initialize()
        {
            //Add Code Here

            //RandomTargetCocktail();
            //_NPC_Name = RandomNPC();
            InitNpc();
            ShuffleCustomers();

            ListTextTypes = GlobalVariable.ListOfTextTpyeEachDay[GlobalVariable.Day-1];


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
            UserInterface.Active.ShowCursor = true;
            UserInterface.TimeToShowTooltipText = 0.6f;

            // #EBE4C6 light cream
            RichParagraphStyleInstruction.AddInstruction("MENU_TEXT", new RichParagraphStyleInstruction(fillColor: new Color(192, 130, 30)));


            //Add Code Here

            //load font
            RegularFont = Content.Load<SpriteFont>("Fonts/Regular");
            BoldFont = Content.Load<SpriteFont>("Fonts/Bold");
            ItalicFont = Content.Load<SpriteFont>("Fonts/Italic");
            UserInterface._tooltipFont = RegularFont;

            //Add Code Above
            InitBGM();
            InitSFX();

            //This is Base DO NOT DELETE
            InitUI();
            ActiveMixerAndAlcholButton(false);
            base.LoadContent();
        }
        public void InitUI()
        {
            atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
            Atlas_CustomerNPC = TextureAtlas.FromFile(Content, "images/Customer/CustomerNPC_Define.xml");
            Atlas_BGNPC = TextureAtlas.FromFile(Content, "images/Background/BG_NPC_Define.xml");
            MiniGame_Shaking_Atlas = TextureAtlas.FromFile(Content, "images/MiniGame/QTE_Shake_Define.xml");
            Shaking_Anim = MiniGame_Shaking_Atlas.CreateAnimatedSprite("Shaking_Animation");
            MinGame_Stirring_Atlas = TextureAtlas.FromFile(Content, "images/MiniGame/QTE_Stir_Define.xml");
            Stirring_Anim = MinGame_Stirring_Atlas.CreateAnimatedSprite("Stirring_Animation");
            Mouse_Atlas = TextureAtlas.FromFile(Content, "images/UI/MouseButton/Mouse_Define.xml");
            Mouse_Anim = Mouse_Atlas.CreateAnimatedSprite("LeftMouseClick");

            //Load Ui image
            Texture2D T_Alchohol_Panel = Content.Load<Texture2D>("images/UI/Shelf");
            Texture2D T_Mixer_Panel = Content.Load<Texture2D>("images/UI/Shelf");
            Texture2D T_DialogBG_Panel = Content.Load<Texture2D>("images/UI/Img_Panel_DialogBG");

            Texture2D T_BTN_Alchol = Content.Load<Texture2D>("images/UI/BTN_Icon_Alcohol");
            Texture2D T_BTN_Mixer = Content.Load<Texture2D>("images/UI/BTN_Icon_Mixer");

            Texture2D T_BG_Background = Content.Load<Texture2D>("images/Background/BG_Background");
            Texture2D T_BG_Midgroud = Content.Load<Texture2D>("images/Background/BG_MidGround");
            Texture2D T_BG_Foreground = Content.Load<Texture2D>("images/Background/BG_ForeGroun");

            #region Load Image Button Alcohol
            //load image button alcohol
            Texture2D T_BTN_Alcohol_Gin_Default = Content.Load<Texture2D>("images/UI/Alcohol/Gin_160x175");
            Texture2D T_BTN_Alcohol_Gin_Hover = Content.Load<Texture2D>("images/UI/Alcohol/Gin_160x175_Hover");
            Texture2D T_BTN_Alcohol_Gin_Pressed = Content.Load<Texture2D>("images/UI/Alcohol/Gin_160x175_Pressed");

            Texture2D T_BTN_Alcohol_Vodka_Default = Content.Load<Texture2D>("images/UI/Alcohol/Vodka_160x175");
            Texture2D T_BTN_Alcohol_Vodka_Hover = Content.Load<Texture2D>("images/UI/Alcohol/Vodka_160x175_Hover");
            Texture2D T_BTN_Alcohol_Vodka_Pressed = Content.Load<Texture2D>("images/UI/Alcohol/Vodka_160x175_Pressed");

            Texture2D T_BTN_Alcohol_Triplesec_Default = Content.Load<Texture2D>("images/UI/Alcohol/Triplesec_160x175");
            Texture2D T_BTN_Alcohol_Triplesec_Hover = Content.Load<Texture2D>("images/UI/Alcohol/Triplesec_160x175_Hover");
            Texture2D T_BTN_Alcohol_Triplesec_Pressed = Content.Load<Texture2D>("images/UI/Alcohol/Triplesec_160x175_Pressed");

            Texture2D T_BTN_Alcohol_Vermouth_Default = Content.Load<Texture2D>("images/UI/Alcohol/Vermouth_160x175");
            Texture2D T_BTN_Alcohol_Vermouth_Hover = Content.Load<Texture2D>("images/UI/Alcohol/Vermouth_160x175_Hover");
            Texture2D T_BTN_Alcohol_Vermouth_Pressed = Content.Load<Texture2D>("images/UI/Alcohol/Vermouth_160x175_Pressed");
            #endregion

            #region Load Image Button Mixer
            //load image buttone Mixer
            Texture2D T_BTN_Mixer_CanberryJuice_Default = Content.Load<Texture2D>("images/UI/Mixer/CanberryJuice_160x175");
            Texture2D T_BTN_Mixer_CanberryJuice_Hover = Content.Load<Texture2D>("images/UI/Mixer/CanberryJuice_160x175_Hover");
            Texture2D T_BTN_Mixer_CanberryJuice_Pressed = Content.Load<Texture2D>("images/UI/Mixer/CanberryJuice_160x175_Pressed");

            Texture2D T_BTN_Mixer_GrapefruitJuice_Default = Content.Load<Texture2D>("images/UI/Mixer/GrapefruitJuice_160x175");
            Texture2D T_BTN_Mixer_GrapefruitJuice_Hover = Content.Load<Texture2D>("images/UI/Mixer/GrapefruitJuice_160x175_Hover");
            Texture2D T_BTN_Mixer_GrapefruitJuice_Pressed = Content.Load<Texture2D>("images/UI/Mixer/GrapefruitJuice_160x175_Pressed");

            Texture2D T_BTN_Mixer_LemonJuice_Default = Content.Load<Texture2D>("images/UI/Mixer/Lemon_160x175");
            Texture2D T_BTN_Mixer_LemonJuice_Hover = Content.Load<Texture2D>("images/UI/Mixer/Lemon_160x175_Hover");
            Texture2D T_BTN_Mixer_LemonJuice_Pressed = Content.Load<Texture2D>("images/UI/Mixer/Lemon_160x175_Pressed");

            Texture2D T_BTN_Mixer_Soda_Default = Content.Load<Texture2D>("images/UI/Mixer/Soda_160x175");
            Texture2D T_BTN_Mixer_Soda_Hover = Content.Load<Texture2D>("images/UI/Mixer/Soda_160x175_Hover");
            Texture2D T_BTN_Mixer_Soda_Pressed = Content.Load<Texture2D>("images/UI/Mixer/Soda_160x175_Pressed");

            Texture2D T_BTN_Mixer_Syrup_Default = Content.Load<Texture2D>("images/UI/Mixer/Syrup_160x175");
            Texture2D T_BTN_Mixer_Syrup_Hover = Content.Load<Texture2D>("images/UI/Mixer/Syrup_160x175_Hover");
            Texture2D T_BTN_Mixer_Syrup_Pressed = Content.Load<Texture2D>("images/UI/Mixer/Syrup_160x175_Pressed");

            Texture2D T_BTN_Mixer_PepperMint_Default = Content.Load<Texture2D>("images/UI/Mixer/PepperMint_160x175");
            Texture2D T_BTN_Mixer_PepperMint_Hover = Content.Load<Texture2D>("images/UI/Mixer/PepperMint_160x175_Hover");
            Texture2D T_BTN_Mixer_PepperMint_Pressed = Content.Load<Texture2D>("images/UI/Mixer/PepperMint_160x175_Pressed");
            #endregion

            Texture2D T_CocktailBase = Content.Load<Texture2D>("images/Cocktail/BaseCocktailGlass");




            P_MainGame = new Panel(new Vector2(1920, 1080), PanelSkin.None, anchor: Anchor.Center);
            P_MainGame.Padding = Vector2.Zero;

            P_Ingredient = new Panel(new Vector2(800, 600), PanelSkin.None, anchor: Anchor.TopRight, offset: new Vector2(0, 0));
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
                Core.Audio.PlaySoundEffect(SFX_Open_Interface);
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

            BTN_Alcohol_Vodka = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Alcohol_Vodka.Padding = Vector2.Zero;
            BTN_Alcohol_Vodka.Offset = new Vector2((50 * 1), 38);
            BTN_Alcohol_Vodka.SetCustomSkin(T_BTN_Alcohol_Vodka_Default, T_BTN_Alcohol_Vodka_Hover, T_BTN_Alcohol_Vodka_Pressed);
            BTN_Alcohol_Vodka.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Alcohol_Vodka.ButtonParagraph.OutlineWidth = 0;
            BTN_Alcohol_Vodka.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Alcohol_Vodka.ButtonParagraph.FontOverride = BoldFont;
            BTN_Alcohol_Vodka.ToolTipText = "Vodka";
            BTN_Alcohol_Vodka.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vodka, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Vodka. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };
            

            BTN_Alcohol_Gin = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Alcohol_Gin.Padding = Vector2.Zero;
            BTN_Alcohol_Gin.Offset = new Vector2(160 + (50 * 2), 38);
            BTN_Alcohol_Gin.SetCustomSkin(T_BTN_Alcohol_Gin_Default, T_BTN_Alcohol_Gin_Hover, T_BTN_Alcohol_Gin_Pressed);
            BTN_Alcohol_Gin.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Alcohol_Gin.ButtonParagraph.OutlineWidth = 0;
            BTN_Alcohol_Gin.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Alcohol_Gin.ButtonParagraph.FontOverride = BoldFont;
            BTN_Alcohol_Gin.ToolTipText = "Gin";
            BTN_Alcohol_Gin.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Gin, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Gin. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };


            BTN_Alcohol_Triplesec = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Alcohol_Triplesec.Padding = Vector2.Zero;
            BTN_Alcohol_Triplesec.Offset = new Vector2(50, 257);
            BTN_Alcohol_Triplesec.SetCustomSkin(T_BTN_Alcohol_Triplesec_Default, T_BTN_Alcohol_Triplesec_Hover, T_BTN_Alcohol_Triplesec_Pressed);
            BTN_Alcohol_Triplesec.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Alcohol_Triplesec.ButtonParagraph.OutlineWidth = 0;
            BTN_Alcohol_Triplesec.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Alcohol_Triplesec.ButtonParagraph.FontOverride = BoldFont;
            BTN_Alcohol_Triplesec.ToolTipText = "Triplesec";
            BTN_Alcohol_Triplesec.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Triplesec, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Triplesec. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };

            BTN_Alcohol_Vermouth = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Alcohol_Vermouth.Padding = Vector2.Zero;
            BTN_Alcohol_Vermouth.Offset = new Vector2(160 + (50 * 2), 257);
            BTN_Alcohol_Vermouth.SetCustomSkin(T_BTN_Alcohol_Vermouth_Default, T_BTN_Alcohol_Vermouth_Hover, T_BTN_Alcohol_Vermouth_Pressed);
            BTN_Alcohol_Vermouth.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Alcohol_Vermouth.ButtonParagraph.OutlineWidth = 0;
            BTN_Alcohol_Vermouth.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Alcohol_Vermouth.ButtonParagraph.FontOverride = BoldFont;
            BTN_Alcohol_Vermouth.ToolTipText = "Vermouth";
            BTN_Alcohol_Vermouth.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vermouth, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Vermouth. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
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
                Core.Audio.PlaySoundEffect(SFX_Open_Interface);
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

            BTN_Mixer_CanberryJuice = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Mixer_CanberryJuice.Padding = Vector2.Zero;
            BTN_Mixer_CanberryJuice.Offset = new Vector2((50 * 1) + (160 * 0), 38);
            BTN_Mixer_CanberryJuice.SetCustomSkin(T_BTN_Mixer_CanberryJuice_Default, T_BTN_Mixer_CanberryJuice_Hover, T_BTN_Mixer_CanberryJuice_Pressed);
            BTN_Mixer_CanberryJuice.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Mixer_CanberryJuice.ButtonParagraph.OutlineWidth = 0;
            BTN_Mixer_CanberryJuice.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Mixer_CanberryJuice.ButtonParagraph.FontOverride = BoldFont;
            BTN_Mixer_CanberryJuice.ToolTipText = "Canberry Juice";
            BTN_Mixer_CanberryJuice.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.CanberryJuice, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Canberry Juice. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };

            BTN_Mixer_GrapefruitJuice = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Mixer_GrapefruitJuice.Padding = Vector2.Zero;
            BTN_Mixer_GrapefruitJuice.Offset = new Vector2((50 * 2) + (160 * 1), 38);
            BTN_Mixer_GrapefruitJuice.SetCustomSkin(T_BTN_Mixer_GrapefruitJuice_Default, T_BTN_Mixer_GrapefruitJuice_Hover, T_BTN_Mixer_GrapefruitJuice_Pressed);
            BTN_Mixer_GrapefruitJuice.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Mixer_GrapefruitJuice.ButtonParagraph.OutlineWidth = 0;
            BTN_Mixer_GrapefruitJuice.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Mixer_GrapefruitJuice.ButtonParagraph.FontOverride = BoldFont;
            BTN_Mixer_GrapefruitJuice.ToolTipText = "Grapefruit Juice";
            BTN_Mixer_GrapefruitJuice.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.GrapefruitJuice, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Grapefruit Juice. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };

            BTN_Mixer_LemonJuice = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Mixer_LemonJuice.Padding = Vector2.Zero;
            BTN_Mixer_LemonJuice.Offset = new Vector2((50 * 3) + (160 * 2), 38);
            BTN_Mixer_LemonJuice.SetCustomSkin(T_BTN_Mixer_LemonJuice_Default, T_BTN_Mixer_LemonJuice_Hover, T_BTN_Mixer_LemonJuice_Pressed);
            BTN_Mixer_LemonJuice.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Mixer_LemonJuice.ButtonParagraph.OutlineWidth = 0;
            BTN_Mixer_LemonJuice.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Mixer_LemonJuice.ButtonParagraph.FontOverride = BoldFont;
            BTN_Mixer_LemonJuice.ToolTipText = "Lemon Juice";
            BTN_Mixer_LemonJuice.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_Lemon);
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.LemonJuice, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Lemon Juice. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };

            //new row
            BTN_Mixer_Soda = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Mixer_Soda.Padding = Vector2.Zero;
            BTN_Mixer_Soda.Offset = new Vector2((50 * 1) + (160 * 0), 257);
            BTN_Mixer_Soda.SetCustomSkin(T_BTN_Mixer_Soda_Default, T_BTN_Mixer_Soda_Hover, T_BTN_Mixer_Soda_Pressed);
            BTN_Mixer_Soda.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Mixer_Soda.ButtonParagraph.OutlineWidth = 0;
            BTN_Mixer_Soda.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Mixer_Soda.ButtonParagraph.FontOverride = BoldFont;
            BTN_Mixer_Soda.ToolTipText = "Soda";
            BTN_Mixer_Soda.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Soda, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Soda. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };

            BTN_Mixer_Syrup = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Mixer_Syrup.Padding = Vector2.Zero;
            BTN_Mixer_Syrup.Offset = new Vector2((50 * 2) + (160 * 1), 257);
            BTN_Mixer_Syrup.SetCustomSkin(T_BTN_Mixer_Syrup_Default, T_BTN_Mixer_Syrup_Hover, T_BTN_Mixer_Syrup_Pressed);
            BTN_Mixer_Syrup.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Mixer_Syrup.ButtonParagraph.OutlineWidth = 0;
            BTN_Mixer_Syrup.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Mixer_Syrup.ButtonParagraph.FontOverride = BoldFont;
            BTN_Mixer_Syrup.ToolTipText = "Syrup";
            BTN_Mixer_Syrup.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Pouring);
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Syrup, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Syrup. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };

            BTN_Mixer_PepperMint = new Button("", skin: ButtonSkin.Default, anchor: Anchor.TopLeft, size: new Vector2(160, 175));
            BTN_Mixer_PepperMint.Padding = Vector2.Zero;
            BTN_Mixer_PepperMint.Offset = new Vector2((50 * 3) + (160 * 2), 257);
            BTN_Mixer_PepperMint.SetCustomSkin(T_BTN_Mixer_PepperMint_Default, T_BTN_Mixer_PepperMint_Hover, T_BTN_Mixer_PepperMint_Pressed);
            BTN_Mixer_PepperMint.ButtonParagraph.Anchor = Anchor.BottomCenter;
            BTN_Mixer_PepperMint.ButtonParagraph.OutlineWidth = 0;
            BTN_Mixer_PepperMint.ButtonParagraph.Offset = new Vector2(0, TextUIOffeset_BTN);
            BTN_Mixer_PepperMint.ButtonParagraph.FontOverride = BoldFont;
            BTN_Mixer_PepperMint.ToolTipText = "Pepper Mint";
            BTN_Mixer_PepperMint.OnMouseDown = (Entity e) =>
            {
                PlaySoundEffectWithRandomPitch(SFX_Peppermint);
                _currentCocktail.AddOrUpdateMixer(Enum_Mixer.PepperMint, 1);
                UpdateCocktailBars();
                Debug.WriteLine("Added Pepper Mint. Current cocktail parts: " + _currentCocktail.GetCountPart());
                Debug.WriteLine(_currentCocktail.Info());
                VisibleMakingCocktailVisual(true);

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
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
            P_MakeingZone.Offset = new Vector2(0, 600);

            Texture2D T_BTN_Stiring = Content.Load<Texture2D>("images/UI/MakingZone/Button_Stir_Default");
            Texture2D T_BTN_Stiring_Hover = Content.Load<Texture2D>("images/UI/MakingZone/Button_Stir_Hover");
            Texture2D T_BTN_Stiring_Pressed = Content.Load<Texture2D>("images/UI/MakingZone/Button_Stir_Off");
            BTN_Stiring = new Button("", skin: ButtonSkin.Default, anchor: Anchor.CenterRight, size: new Vector2(220, 80));
            BTN_Stiring.Padding = Vector2.Zero;
            BTN_Stiring.Offset = new Vector2(75, 0);
            BTN_Stiring.ButtonParagraph.OutlineWidth = 0;
            BTN_Stiring.SetCustomSkin(T_BTN_Stiring, T_BTN_Stiring_Hover, T_BTN_Stiring_Pressed);
            BTN_Stiring.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_PressedBTN);
                _currentCocktail.UseMethod(Enum_Method.Mixing);
                openAlcoholPanel = false;
                openMixerPanel = false;

                Debug.WriteLine("Selected Method: Stiring");
                Debug.WriteLine(_currentCocktail.Info());

                currentMinigame = Enum_MiniGameType.Stiring;
                ShowMinigame(Enum_MiniGameType.Stiring);

                StiringMinigame.StartGame();

                BTNMethodActive(false);
                BTNMethodVisible(false);
                EnableBTNBeforeServe(true);
                EnableBookRecipes(false);
            };

            Texture2D T_BTN_Shaking = Content.Load<Texture2D>("images/UI/MakingZone/Button_Shake_Default");
            Texture2D T_BTN_Shaking_Hover = Content.Load<Texture2D>("images/UI/MakingZone/Button_Shake_Hover");
            Texture2D T_BTN_Shaking_Pressed = Content.Load<Texture2D>("images/UI/MakingZone/Button_Shake_Off");
            BTN_Shaking = new Button("", skin: ButtonSkin.Default, anchor: Anchor.CenterRight, size: new Vector2(220, 80));
            BTN_Shaking.Padding = Vector2.Zero;
            BTN_Shaking.Offset = new Vector2(75, 60 + 20);
            BTN_Shaking.ButtonParagraph.OutlineWidth = 0;
            BTN_Shaking.SetCustomSkin(T_BTN_Shaking, T_BTN_Shaking_Hover, T_BTN_Shaking_Pressed);
            BTN_Shaking.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_PressedBTN);
                _currentCocktail.UseMethod(Enum_Method.Shaking);
                openAlcoholPanel = false;
                openMixerPanel = false;

                Debug.WriteLine("Selected Method: Shaking");
                Debug.WriteLine(_currentCocktail.Info());

                currentMinigame = Enum_MiniGameType.Shaking;
                ShowMinigame(Enum_MiniGameType.Shaking);

                ShakingMinigame.StartGame();

                BTNMethodActive(false);
                BTNMethodVisible(false);
                EnableBTNBeforeServe(true);
                EnableBookRecipes(false);
            };

            Texture2D T_BTN_Reset = Content.Load<Texture2D>("images/UI/MakingZone/Button_Reset_Default");
            Texture2D T_BTN_Reset_Hover = Content.Load<Texture2D>("images/UI/MakingZone/Button_Reset_Hover");
            Texture2D T_BTN_Reset_Pressed = Content.Load<Texture2D>("images/UI/MakingZone/Button_Reset_Off");
            BTN_Reset_OnTable = new Button("", skin: ButtonSkin.Default, anchor: Anchor.Center, size: new Vector2(60,60));
            BTN_Reset_OnTable.Padding = Vector2.Zero;
            BTN_Reset_OnTable.Offset = new Vector2(-100, 150);
            BTN_Reset_OnTable.ButtonParagraph.OutlineWidth = 0;
            BTN_Reset_OnTable.SetCustomSkin(T_BTN_Reset, T_BTN_Reset_Hover, T_BTN_Reset_Pressed);
            BTN_Reset_OnTable.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_PressedBTN);
                _currentCocktail.ClearAllIngredients();
                openAlcoholPanel = false;
                openMixerPanel = false;

                Debug.WriteLine("Reset On Table");
                Debug.WriteLine(_currentCocktail.Info());

                currentMinigame = Enum_MiniGameType.None;
                ShowMinigame(Enum_MiniGameType.None);

                ShowMinigame(false);

                BTNIngredeientActive(true);
                BTNMethodVisible(false);

                ResetUI();
                UpdateCocktailBars();


                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            };
            

            Img_CocktailBottle = new Image(T_CocktailBase, new Vector2(80, 140), anchor: Anchor.Center);
            Img_CocktailBottle.Offset = new Vector2(-100, 25);
            Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();
            
            

            //visual cocktail
            InitMakingVisualCocktail();

            //Book Recipe Button

            Texture2D T_BookRecipes_Default = Content.Load<Texture2D>("images/UI/BookButton/Book_Default");
            Texture2D T_BookRecipes_Hover = Content.Load<Texture2D>("images/UI/BookButton/Book_Hover");

            BTN_BookRecipes = new Button("", skin: ButtonSkin.Default, anchor: Anchor.BottomLeft, size: new Vector2(128, 128));
            BTN_BookRecipes.SetCustomSkin(T_BookRecipes_Default, T_BookRecipes_Hover, T_BookRecipes_Hover);
            BTN_BookRecipes.Offset = new Vector2(-50, 75);
            BTN_BookRecipes.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_Book_Open_Close);
                ToggleBookRecipes();

            };


            #endregion

            #region Add Child to Panel Making Zone
            P_MakeingZone.AddChild(BTN_Stiring);
            P_MakeingZone.AddChild(BTN_Shaking);
            P_MakeingZone.AddChild(BTN_Reset_OnTable);
            P_MakeingZone.AddChild(Img_CocktailBottle);
            P_MakeingZone.AddChild(P_MakingCocktailVisual);
            P_MakeingZone.AddChild(BTN_BookRecipes);

            #endregion

            #region Panel Minigame Zone
            P_Minigame = new Panel(new Vector2(800, 600), PanelSkin.None, anchor: Anchor.TopRight);
            P_Minigame.Padding = Vector2.Zero;
            P_Minigame.Offset = new Vector2(-800, 0);

            P_Minigame_Shaking = new Panel(new Vector2(800, 600), PanelSkin.Fancy, anchor: Anchor.TopRight);
            P_Minigame_Shaking.Padding = Vector2.Zero;
            P_Minigame_Shaking.Offset = new Vector2(0, 0);

            
            Img_Minigame_Shaking = new Image(MiniGame_Shaking_Atlas.Texture, new Vector2(800, 600), anchor: Anchor.TopCenter);
            Img_Minigame_Shaking.SourceRectangle = Shaking_Anim.GetRectangleCurrentFrame();
            P_Minigame_Shaking.AddChild(Img_Minigame_Shaking);

            InitShakingMinigameUI();

            P_Minigame_Stirring = new Panel(new Vector2(800, 600), PanelSkin.Fancy, anchor: Anchor.TopRight);
            P_Minigame_Stirring.Padding = Vector2.Zero;
            P_Minigame_Stirring.Offset = new Vector2(0, 0);
            P_Minigame_Shaking.FillColor = Color.Red;

            
            Img_MiniGame_Stirring = new Image(MinGame_Stirring_Atlas.Texture, new Vector2(800, 600), anchor: Anchor.TopCenter);
            Img_MiniGame_Stirring.SourceRectangle = Stirring_Anim.GetRectangleCurrentFrame();
            P_Minigame_Stirring.AddChild(Img_MiniGame_Stirring);

            InitStiringMinigameUI();

            Img_Mouse = new Image(Mouse_Atlas.Texture, new Vector2(100, 100), anchor: Anchor.BottomLeft);
            Img_Mouse.Offset = new Vector2(-100, 0);
            Img_Mouse.SourceRectangle = Mouse_Anim.GetRectangleCurrentFrame();
            Img_Mouse.Visible = false;
            #endregion

            #region Add Child To Panel Minigame Zone

            P_Minigame.AddChild(P_Minigame_Shaking);
            P_Minigame.AddChild(P_Minigame_Stirring);
            P_Minigame.AddChild(Img_Mouse);

            #endregion

            #region Before Serve Panel
            T_P_BeforeServe = Content.Load<Texture2D>("images/UI/MakingZone/Panel_BeforeServe");
            T_P_BeforeServe_Addice = Content.Load<Texture2D>("images/UI/MakingZone/Panel_BeforeServe_AddIce");
            P_BeforeServe = new Image(T_P_BeforeServe, new Vector2(800, 480),  anchor: Anchor.TopRight);
            P_BeforeServe.Padding = Vector2.Zero;
            P_BeforeServe.Offset = new Vector2(-800, 600);



            Texture2D T_BTN_AddIce_Default = Content.Load<Texture2D>("images/UI/MakingZone/Button_AddIce_Defalut");
            Texture2D T_BTN_AddIce_Hover = Content.Load<Texture2D>("images/UI/MakingZone/Button_AddIce_Hover");
            Texture2D T_BTN_AddIce_Pressed = Content.Load<Texture2D>("images/UI/MakingZone/Button_AddIce_Off");
            BTN_AddIce = new Button("", skin: ButtonSkin.Default, anchor: Anchor.CenterLeft, size: new Vector2(220, 80));
            BTN_AddIce.Padding = Vector2.Zero;
            BTN_AddIce.Offset = new Vector2(50,0);
            BTN_AddIce.ButtonParagraph.OutlineWidth = 0;
            BTN_AddIce.SetCustomSkin(T_BTN_AddIce_Default, T_BTN_AddIce_Hover, T_BTN_AddIce_Pressed);
            BTN_AddIce.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_AddIce);
                _currentCocktail.AddIce(true);
                Debug.WriteLine("Added Ice");
                Debug.WriteLine(_currentCocktail.Info());
                P_BeforeServe.Texture = T_P_BeforeServe_Addice;
                BTN_AddIce.Enabled = false;
                BTN_AddIce.Visible = false;
            };
            

            Texture2D T_BTN_Serve_Default = Content.Load<Texture2D>("images/UI/MakingZone/Button_Serve_Default");
            Texture2D T_BTN_Serve_Hover = Content.Load<Texture2D>("images/UI/MakingZone/Button_Serve_Hover");
            Texture2D T_BTN_Serve_Pressed = Content.Load<Texture2D>("images/UI/MakingZone/Button_Serve_Off");
            BTN_Serve = new Button("", skin: ButtonSkin.Default, anchor: Anchor.CenterRight, size: new Vector2(220, 80));
            BTN_Serve.Padding = Vector2.Zero;
            BTN_Serve.Offset = new Vector2(50,0);
            BTN_Serve.ButtonParagraph.OutlineWidth = 0;
            BTN_Serve.SetCustomSkin(T_BTN_Serve_Default, T_BTN_Serve_Hover, T_BTN_Serve_Pressed);
            BTN_Serve.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_PressedBTN);
                ActiveMixerAndAlcholButton(false);
                _currentCocktail.SetTypeOfCocktailBySearch();
                _currentCocktail.SetNameOfCocktailBySearch();
                str_currentCocktail_Name = _currentCocktail.GetName();
                Img_CocktailResult.SourceRectangle = CocktailResult_Atlas.GetRegion(_currentCocktail.GetName()).SourceRectangle;
                Debug.WriteLine("--------***************** \n Target Cocktail is: " + _currentCocktail.GetName());
                Debug.WriteLine("Served Cocktail");
                Debug.WriteLine(_currentCocktail.Info());
                float price = CalcualatePrice(_targetCoctail);
                Debug.WriteLine("Earned Price: " + price);

                Debug.WriteLine("New Target Cocktail is: " + str_targetCocktail_Name);

                BTNIngredeientActive(true);
                BTNMethodVisible(false);
                EnableBTNBeforeServe(false);

                ResetUI();
                EnableBTNBeforeServe(false);
                stateBeforeServePanel = Enum_PanelState.Pos1;
                stateArtAfterServePanel = Enum_PanelState.Pos1;
                EnableOrderPanel(false);
            };


            Texture2D T_BTN_Rest_BeforeServe = Content.Load<Texture2D>("images/UI/MakingZone/Button_Reset_Default");
            Texture2D T_BTN_Rest_BeforeServe_Hover = Content.Load<Texture2D>("images/UI/MakingZone/Button_Reset_Hover");
            Texture2D T_BTN_Rest_BeforeServe_Pressed = Content.Load<Texture2D>("images/UI/MakingZone/Button_Reset_Off");
            BTN_Rest_BeforeServe = new Button("", skin: ButtonSkin.Default, anchor: Anchor.BottomCenter, size: new Vector2(60, 60));
            BTN_Rest_BeforeServe.Padding = Vector2.Zero;
            BTN_Rest_BeforeServe.Offset = new Vector2(0, 50);
            BTN_Rest_BeforeServe.ButtonParagraph.OutlineWidth = 0;
            BTN_Rest_BeforeServe.SetCustomSkin(T_BTN_Rest_BeforeServe, T_BTN_Rest_BeforeServe_Hover, T_BTN_Rest_BeforeServe_Pressed);
            BTN_Rest_BeforeServe.OnMouseDown = (Entity e) =>
            {
                Core.Audio.PlaySoundEffect(SFX_PressedBTN);
                _currentCocktail.ClearAllIngredients();
                openAlcoholPanel = false;
                openMixerPanel = false;

                ResetUI();
                MiniGame.ShakingMinigame.Reset();
                MiniGame.StiringMinigame.Reset();

                Img_CocktailBottle.ToolTipText = _currentCocktail.GetSimpleInfo();

            };
            #endregion

            #region Add Child Before Serve
            P_BeforeServe.AddChild(BTN_AddIce);
            P_BeforeServe.AddChild(BTN_Serve);
            P_BeforeServe.AddChild(BTN_Rest_BeforeServe);

            #endregion

            #region Oreder Panel
            ImgP_OrderPanel = new Image(T_DialogBG_Panel, new Vector2(600, 250), anchor: Anchor.TopLeft);
            ImgP_OrderPanel.Padding = Vector2.Zero;
            ImgP_OrderPanel.Offset = new Vector2(375, 620);

            RP_CustomerName = new RichParagraph(Customers[numbercustomer]._Name, anchor: Anchor.TopCenter, size: new Vector2(300, 50));
            RP_CustomerName.OutlineWidth = 0;
            RP_CustomerName.Offset = new Vector2(-5, 20);
            RP_CustomerName.OutlineOpacity = 0;
            RP_CustomerName.FontOverride = BoldFont;
            RP_CustomerName.FillColor = new Color(218, 180, 120);

            RP_ConversationCustomer = new RichParagraph("", anchor: Anchor.TopCenter, size: new Vector2(500, 200));
            RP_ConversationCustomer.Offset = new Vector2(0, 70);
            RP_ConversationCustomer.OutlineWidth = 0;
            RP_ConversationCustomer.OutlineOpacity = 0;
            RP_ConversationCustomer.FontOverride = RegularFont;
            RP_ConversationCustomer.AlignToCenter = true;
            RP_ConversationCustomer.FillColor = new Color(235, 228, 202);


            #endregion

            #region Debug Part for dev
#if DEBUG
            Button FinishString = new Button("Finish Stiring", skin: ButtonSkin.Default, Anchor.Center, new Vector2(200, 100));
            FinishString.OnMouseDown = (Entity e) =>
            {
                Debug.WriteLine("Finish Stiring Minigame");
                stateBeforeServePanel = Enum_PanelState.Open;
                //openMinigamePanel = false;
                //P_Minigame_Stiring.Offset = new Vector2(-700, 0);
            };

            Button FinishShake = new Button("Finish Shaking", skin: ButtonSkin.Default, Anchor.Center, new Vector2(200, 100));
            FinishShake.OnMouseDown = (Entity e) =>
            {
                Debug.WriteLine("Finish Shaking Minigame");
                stateBeforeServePanel = Enum_PanelState.Open;
                //openMinigamePanel = false;
                //P_Minigame_Shaking.Offset = new Vector2(-700, 0);
            };

            //P_Minigame_Shaking.AddChild(FinishShake);
            //P_Minigame_Stiring.AddChild(FinishString);


            P_Debug_targetCocktail = new Paragraph("Target Cocktail: " + str_targetCocktail_Name + _targetCoctail.Info(), anchor: Anchor.TopLeft, size: new Vector2(300, 500));
            P_Debug_targetCocktail.Offset = new Vector2(0, 0);

            P_Debug_CurrentCocktail = new Paragraph("Current Cocktail: " + _currentCocktail.Info(), anchor: Anchor.TopLeft, size: new Vector2(300, 500));
            P_Debug_CurrentCocktail.Offset = new Vector2(300, 0);

            P_Debug_GlobalVariable = new Paragraph(GlobalVariable.DebugPrintString() + 0 , anchor: Anchor.TopLeft, size: new Vector2(300, 500));
            P_Debug_GlobalVariable.Offset = new Vector2(600, 0);
#endif
            #endregion

            #region Add Child Order Panel

            ImgP_OrderPanel.AddChild(RP_ConversationCustomer);
            ImgP_OrderPanel.AddChild(RP_CustomerName);
            EnableOrderPanel(false);

            #endregion

            #region Customer Image

            Img_Customer = new Image(Atlas_CustomerNPC.GetRegion(Customers[numbercustomer].GetID() + "_default").GetTexture2D(), new Vector2(450, 650), anchor: Anchor.TopLeft);
            Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(Customers[numbercustomer].GetID() + "_default").SourceRectangle;
            Img_Customer.Offset = new Vector2(1920, 40);

            #endregion

            #region BGNPC Image Atlas

            Img_BG_NPC = new Image(Atlas_BGNPC.GetRegion("FaceRight").GetTexture2D(), new Vector2(300, 650), anchor: Anchor.CenterLeft);
            Img_BG_NPC.SourceRectangle = Atlas_BGNPC.GetRegion("FaceRight").SourceRectangle;
            #endregion

            #region Art After Serve Panel
            ArtAfterServe_Atlas = TextureAtlas.FromFile(Content, "images/ArtAfterServe/ArtAfterServe_Define.xml");

            P_ArtAfterServe = new Panel(new Vector2(800, 600), PanelSkin.Default, anchor: Anchor.TopRight);
            P_ArtAfterServe.Padding = Vector2.Zero;
            P_ArtAfterServe.Offset = new Vector2(0, 1080);

            Img_Art1 = new Image(ArtAfterServe_Atlas.Texture);
            Img_Art1.Size = new Vector2(400, 600);
            Img_Art1.Anchor = Anchor.TopLeft;
            Img_Art1.SourceRectangle = ArtAfterServe_Atlas.GetRegion("ArtAfterServe_Art01").SourceRectangle;

            Img_Art2 = new Image(ArtAfterServe_Atlas.Texture);
            Img_Art2.Size = new Vector2(400, 600);
            Img_Art2.Anchor = Anchor.TopRight;
            Img_Art2.SourceRectangle = ArtAfterServe_Atlas.GetRegion("ArtAfterServe_Art02").SourceRectangle;

            P_ArtAfterServe.AddChild(Img_Art1);
            P_ArtAfterServe.AddChild(Img_Art2);
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

            P_MainGame.AddChild(Img_BG_Background);

            #region Image BGNPC
            //Create BGNPC
            Atlas_BGNPC = TextureAtlas.FromFile(Content, "images/Background/BG_NPC_Define.xml");
            movingnpcs.Clear();

            // Get both regions once
            var regionLeft = Atlas_BGNPC.GetRegion("FaceLeft");
            var regionRight = Atlas_BGNPC.GetRegion("FaceRight");

            for (int i = 0; i < npcCount; i++)
            {
                Image BGNPC = new Image(regionRight.GetTexture2D(), new Vector2(300, 650));
                BGNPC.SourceRectangle = regionRight.SourceRectangle;
                BGNPC.Scale = 0.25f;
                BGNPC.Anchor = Anchor.TopLeft;
                P_MainGame.AddChild(BGNPC);

                float initialDelay = (float)(new Random().NextDouble() * 5.0); // Random delay for NPC spawning
                movingnpcs.Add(new BG_NPC(BGNPC, new Rectangle(0, 0, 300, 650), BG_NPC.MovementMode.PingPong, regionLeft.SourceRectangle, regionRight.SourceRectangle));
            }
            #endregion

            P_MainGame.AddChild(Img_BG_Midground);
            P_MainGame.AddChild(Img_Customer);
            P_MainGame.AddChild(Img_BG_Foreground);
            InitCocktailResultEntity();
            P_MainGame.AddChild(Img_CocktailResult);

#if DEBUG
            //UserInterface.Active.AddEntity(P_Debug_CurrentCocktail);
            //UserInterface.Active.AddEntity(P_Debug_targetCocktail);
            P_MainGame.AddChild(P_Debug_CurrentCocktail);
            P_MainGame.AddChild(P_Debug_targetCocktail);
            P_MainGame.AddChild(P_Debug_GlobalVariable);
#endif

            P_MainGame.AddChild(P_Ingredient);


            P_MainGame.AddChild(P_MakeingZone);
            P_MainGame.AddChild(P_Minigame);
            P_MainGame.AddChild(P_BeforeServe);
            P_MainGame.AddChild(ImgP_OrderPanel);

            P_MainGame.AddChild(P_ArtAfterServe);

            InitBookRecipes();
            //UserInterface.Active.AddEntity(P_BGBookRecipes);
            P_MainGame.AddChild(Img_BookRecipes);


            //add UI to UserInterface
            UserInterface.Active.AddEntity(P_MainGame);
            InitFadePanel();
            #endregion


            //disaple stir and shaking at start
            BTNMethodActive(false);
            BTNMethodVisible(false);

            Utilities.ShakeHelper.ShakingEntity(Img_Customer, 0.25f, false, speed: 2.5f);

        }
        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            Core.SpriteBatch.Begin();

            //Core.SpriteBatch.Draw(img_Alchohol_Panel, new Vector2(0, 0), Color.White);
            //foreach (var movnpc in movingnpcs)
            //    movnpc.Draw(Core.SpriteBatch);

            Core.SpriteBatch.End();

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }

        public override void Update(GameTime gameTime)
        {
            // Moving NPC updates
            foreach (var movnpc in movingnpcs)
                movnpc.Update(gameTime);

            UpdateSong();

            // Handle fade-in
            if (shouldFadeIn)
            {
                if (FadeHelper.FadeEntity(P_Fade, gameTime, 255, 0, 3.0f, ref fadeTimer))
                {
                    EnableFadePanel(false);
                    shouldFadeIn = false;
                    OpenBookOnTutorial();
                }
                return;
            }

            UserInterface.Active.Update(gameTime);
            if (!EndTutorial) return; 

            if (shouldFadeOut) {

                if (FadeHelper.FadeEntity(P_Fade, gameTime, 0, 255, 5f, ref fadeTimer))
                {
                    // go to thank scene
                    shouldFadeOut = false;
                    Core.ChangeScene(new Summary());
                }
                return;
            }

                        
            UpdateUILogic(gameTime);
            Utilities.ShakeHelper.Update(gameTime);

            // Skip all updates if no customer interaction
            if (currentCustomerState != Enum_CutomerState.WaitingForServe)
            {
                base.Update(gameTime);
                UserInterface.Active.Update(gameTime);
                return;
            }

            // Update animations and UI
            Shaking_Anim.Update(gameTime);
            Stirring_Anim.Update(gameTime);
            Mouse_Anim.Update(gameTime);
            UpdateUILogic(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Handle Shaking minigame
            if (currentMinigame == Enum_MiniGameType.Shaking)
            {
                if (Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left) && canPlaySFX_Shaking)
                {
                    Core.Audio.PlaySoundEffect(SFX_Shaking);
                    canPlaySFX_Shaking = false;
                }

                cooldownTime_SFX_Shaking -= elapsed;
                if (cooldownTime_SFX_Shaking < 0 && !canPlaySFX_Shaking)
                {
                    canPlaySFX_Shaking = true;
                    cooldownTime_SFX_Shaking = 2f;
                }

                ShakingMinigame.Update(gameTime);
                UpdateMiniGameShakingUI();
                Shaking_Anim.Play();
                Img_Minigame_Shaking.SourceRectangle = Shaking_Anim.GetRectangleCurrentFrame();
                Img_Mouse.SourceRectangle = Mouse_Anim.GetRectangleCurrentFrame();

                if (ShakingMinigame.IsComplete())
                {
                    ShakingMinigame.Stop();
                    Shaking_Anim.Stop();
                    currentMinigame = Enum_MiniGameType.None;
                    stateBeforeServePanel = Enum_PanelState.Open;
                    cooldownTime_SFX_Shaking = 2.5f;
                }
            }

            // Handle Stirring minigame
            else if (currentMinigame == Enum_MiniGameType.Stiring)
            {
                if (Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left) && canPlaySFX_Stiring)
                {
                    PlaySoundEffectWithRandomPitch(SFX_Stiring, 0.5f);
                    //Core.Audio.PlaySoundEffect(SFX_Stiring);
                    canPlaySFX_Stiring = false;
                }

                cooldownTime_SFX_Stiring -= elapsed;
                if (cooldownTime_SFX_Stiring < 0 && !canPlaySFX_Stiring)
                {
                    canPlaySFX_Stiring = true;
                    cooldownTime_SFX_Stiring = 1.2f;
                }

                StiringMinigame.Update(gameTime);
                if (!StiringMinigame.GetIsHitCorrectValue()) { 
                    ShakeHelper.ShakingEntity(PB_Stirring, 5f, true, 0.6f, speed: 100f);
                    PB_Stirring.FillColor = Color.Orange;
                    PB_Stirring.ProgressFill.FillColor = Color.Orange;
                    StiringMinigame.IsHitCorrectValue = true;
                }
                if (ShakeHelper.IsComplete(PB_Stirring))
                {
                    PB_Stirring.Offset = new Vector2(0, 70);
                    PB_Stirring.FillColor = Color.White;
                    PB_Stirring.ProgressFill.FillColor = Color.Yellow;
                }
                UpdateMiniGameStiringUI();
                Stirring_Anim.Play();
                Img_MiniGame_Stirring.SourceRectangle = Stirring_Anim.GetRectangleCurrentFrame();
                Img_Mouse.SourceRectangle = Mouse_Anim.GetRectangleCurrentFrame();

                if (StiringMinigame.IsComplated())
                {
                    StiringMinigame.Stop();
                    Stirring_Anim.Stop();
                    currentMinigame = Enum_MiniGameType.None;
                    stateBeforeServePanel = Enum_PanelState.Open;
                    cooldownTime_SFX_Stiring = 4f;
                }
            }

            // Conversation and text
            UpdateConversation();
            AnimationText.Update(gameTime);

#if DEBUG
            P_Debug_CurrentCocktail.Text = "Current Cocktail:" + str_currentCocktail_Name + "\n" + _currentCocktail.Info();
            P_Debug_targetCocktail.Text = "Target Cocktail: " + str_targetCocktail_Name + "\n" + _targetCoctail.Info();
            P_Debug_GlobalVariable.Text = GlobalVariable.DebugPrintString();
#endif

            base.Update(gameTime);
            UserInterface.Active.Update(gameTime);
        }
        protected void UpdateUILogic(GameTime gameTime)
        {
            CheckCurrentCountPart();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // --- Alcohol panel ---
            if (openAlcoholPanel)
            {
                SlidePanel(FP_Alcohol, 0, 25, Enum_SlideDirection.Right);
                SlidePanel(BTN_Alcohol, -125, 25, Enum_SlideDirection.Left);
            }
            else
            {
                SlidePanel(FP_Alcohol, -800, 25, Enum_SlideDirection.Left);
                SlidePanel(BTN_Alcohol, -25, 25, Enum_SlideDirection.Right);
            }

            // --- Mixer panel ---
            if (openMixerPanel)
            {
                SlidePanel(FP_Mixer, 0, 25, Enum_SlideDirection.Right);
                SlidePanel(BTN_Mixer, -125, 25, Enum_SlideDirection.Left);
            }
            else
            {
                SlidePanel(FP_Mixer, -800, 25, Enum_SlideDirection.Left);
                SlidePanel(BTN_Mixer, -25, 25, Enum_SlideDirection.Right);
            }

            // --- Minigame panel ---
            switch (openMinigamePanel)
            {
                case Enum_PanelState.Open:
                    SlidePanel(P_Minigame, 0, 20, Enum_SlideDirection.Right);
                    Img_Mouse.Visible = true;
                    Mouse_Anim.Play();
                    break;
                case Enum_PanelState.Close:
                    Mouse_Anim.Stop();
                    if (SlidePanel(P_Minigame, -600, 20, Enum_SlideDirection.Up))
                        openMinigamePanel = Enum_PanelState.InitPosWarp;
                    break;
                case Enum_PanelState.InitPosWarp:
                    P_Minigame.Offset = new Vector2(-800, 0);
                    Img_Mouse.Visible = false;
                    break;
            }

            // --- Before Serve panel ---
            switch (stateBeforeServePanel)
            {
                case Enum_PanelState.InitPosWarp:
                    P_BeforeServe.Offset = new Vector2(-800, 600);
                    P_BeforeServe.Texture = T_P_BeforeServe;
                    break;
                case Enum_PanelState.InitPosSlide:
                    SlidePanel(P_BeforeServe, -800, 20, Enum_SlideDirection.Left);
                    break;
                case Enum_PanelState.Open:
                    EnableAllBTN(false);
                    if (SlidePanel(P_BeforeServe, 0, 20, Enum_SlideDirection.Right)) {
                        EnableAllBTN(true);
                    }
                    break;
                case Enum_PanelState.Close:
                    if(SlidePanel(P_BeforeServe, 800, 20, Enum_SlideDirection.Left))
                        P_BeforeServe.Texture = T_P_BeforeServe;
                    break;
                case Enum_PanelState.Pos1:
                    SlidePanel(P_BeforeServe, 0, 20, Enum_SlideDirection.Up);
                    break;
                case Enum_PanelState.Pos2:
                    if (SlidePanel(P_BeforeServe, -800, 20, Enum_SlideDirection.Left))
                        P_BeforeServe.Texture = T_P_BeforeServe;
                        stateBeforeServePanel = Enum_PanelState.InitPosWarp;
                    break;
            }

            // --- After Serve panel ---
            switch (stateArtAfterServePanel)
            {
                case Enum_PanelState.InitPosWarp:
                    P_ArtAfterServe.Offset = new Vector2(0, 1080);
                    break;
                case Enum_PanelState.Pos1:
                    SlidePanel(P_ArtAfterServe, 480, 20, Enum_SlideDirection.Up);
                    break; 
                case Enum_PanelState.Pos2:
                    if (SlidePanel(P_ArtAfterServe, -800, 20, Enum_SlideDirection.Left))
                        stateArtAfterServePanel = Enum_PanelState.InitPosWarp;
                    break;
            }

            // --- Auto-close panels ---
            if (stateArtAfterServePanel == Enum_PanelState.Pos1 && stateBeforeServePanel == Enum_PanelState.Pos1)
            {
                timeToCloseBeforeAndAfteServePanel -= elapsed;
                if (timeToCloseBeforeAndAfteServePanel < 0)
                {
                    stateCocktailResultPanel = Enum_PanelState.Pos1;
                    Core.Audio.PlaySoundEffect(SFX_Serve);
                    timeToCloseBeforeAndAfteServePanel = 3;
                }
            }

            // --- Cocktail result panel ---
            switch (stateCocktailResultPanel)
            {
                case Enum_PanelState.InitPosWarp:
                    
                    Img_CocktailResult.Offset = new Vector2(1920, 75);
                    break;
                case Enum_PanelState.Pos1:
                    if (SlidePanel(Img_CocktailResult, 800, 5, Enum_SlideDirection.Left))
                    {
                        stateArtAfterServePanel = Enum_PanelState.Pos2;
                        stateBeforeServePanel = Enum_PanelState.Pos2;
                        timeToCloseBeforeAndAfteServePanel = 3.0f;
                        EnableOrderPanel(true);
                        canGoNextConversation = true;
                        haveDoneOrder = true;
                        stateCocktailResultPanel = Enum_PanelState.None;
                    }
                    break;
                case Enum_PanelState.Pos2:
                    
                    if (SlidePanel(Img_CocktailResult, -400, 7, Enum_SlideDirection.Left))
                        stateCocktailResultPanel = Enum_PanelState.InitPosWarp;
                    break;
            }

            // --- Customer Image Panel ---
            switch (stateImgCustomer)
            {
                case Enum_PanelState.InitPosWarp:
                    canDoconversation = false;
                    currentCustomerState = Enum_CutomerState.Entering;
                    Core.Audio.PlaySoundEffect(SFX_Welcome);
                    Img_Customer.Offset = new Vector2(1920, 40);
                    stateImgCustomer = Enum_PanelState.Pos1;
                    ShakeHelper.SetShakeAmplitude(Img_Customer, 2f);
                    ShakeHelper.SetShakeSpeed(Img_Customer, 10f);
                    break;

                case Enum_PanelState.Pos1:
                    if (SlidePanel(Img_Customer, 450, 7, Enum_SlideDirection.Left))
                    {
                        canDoconversation = true;
                        ShakeHelper.SetShakeSpeed(Img_Customer, 2.5f);
                        ShakeHelper.SetShakeAmplitude(Img_Customer, 0.25f);
                        stateImgCustomer = Enum_PanelState.None;
                        RandomTargetCocktail();
                        Debug.WriteLine("New Target Cocktail is: " + str_targetCocktail_Name);
                        inStartConversation = true;
                        currentCustomerState = Enum_CutomerState.WaitingForServe;
                    }
                    break;

                case Enum_PanelState.Pos2:
                    canDoconversation = false;
                    EnableOrderPanel(false);
                    if (SlidePanel(Img_Customer, -400, 7, Enum_SlideDirection.Left))
                    {
                        if (numbercustomer >= Customers.Count)
                        {
                            EnableFadePanel(true);
                            shouldFadeOut = true;
                            return;
                        }
                        else
                        {
                            RP_CustomerName.Text = Customers[numbercustomer]._Name;
                            Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(Customers[numbercustomer].GetID() + "_default").SourceRectangle;
                            stateImgCustomer = Enum_PanelState.InitPosWarp;
                            currentCustomerState = Enum_CutomerState.Leaving;
                        }
                    }
                    break;
            }

#if DEBUG
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.T)) stateCocktailResultPanel = Enum_PanelState.Pos1;
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Y)) stateCocktailResultPanel = Enum_PanelState.Pos2;
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.U)) stateCocktailResultPanel = Enum_PanelState.InitPosWarp;
#endif
        }
        protected void UpdateConversation()
        {

            if (!canDoconversation) return;
            RP_ConversationCustomer.Text = AnimationText.GetVisibleText();

            bool mouseClick = Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left);

            // Skip animation
            if (!AnimationText.IsFinished() && mouseClick && canSkipConversation)
            {
                AnimationText.Skip();
                canSkipConversation = false;
                canGoNextConversation = true;
                return;
            }

            // Animation finished automatically
            if (AnimationText.IsFinished())
            {
                canSkipConversation = false;
                canGoNextConversation = true;
            }

            // Advance conversation if conditions met
            if (!(canGoNextConversation && mouseClick || haveDoneOrder || inStartConversation)) return;

            switch (currentPhase)
            {
                case ConversationPhase.SmallTalkBeforeOrder:
                    inStartConversation = false;
                    EnableOrderPanel(true);

                    string beforeOrderLine = Customers[numbercustomer].GetConversationBeforeOrder(Day);
                    if (beforeOrderLine != null)
                    {
                        AnimationText = new TaggedTextRevealer(beforeOrderLine, 0.05);
                        AnimationText.Start();
                        canSkipConversation = true;
                        canGoNextConversation = false;
                    }
                    else
                    {
                        AnimationText = new TaggedTextRevealer(DecideToUseTypeOfText(), 0.05);
                        AnimationText.Start();
                        canSkipConversation = true;
                        canGoNextConversation = false;
                        currentPhase = ConversationPhase.Ordering;
                        ActiveMixerAndAlcholButton(false);
                    }
                    break;

                case ConversationPhase.Ordering:
                    if (AnimationText.IsFinished())
                        ActiveMixerAndAlcholButton(true);

                    if (haveDoneOrder && canGoNextConversation)
                    {
                        ActiveMixerAndAlcholButton(false);
                        if (cocktaillResualt == Enum_CocktaillResualt.None)
                            cocktaillResualt = CalculateAccurateCocktail();
                        

                        switch (cocktaillResualt)
                        {
                            case Enum_CocktaillResualt.Success:
                                GlobalVariable.AddIncome((int)(_targetCoctail.GetPrice()));
                                GlobalVariable.AddTip((int)(_targetCoctail.GetPrice() * 0.2f));
                                GlobalVariable.AddCocktailDone(str_currentCocktail_Name, (int)(_targetCoctail.GetPrice()));

                                Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(Customers[numbercustomer].GetID() + "_happy").SourceRectangle;
                                break;
                            case Enum_CocktaillResualt.Aceptable:
                                GlobalVariable.AddIncome((int)_targetCoctail.GetPrice());
                                GlobalVariable.AddCocktailDone(str_currentCocktail_Name, (int)_targetCoctail.GetPrice());

                                Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(Customers[numbercustomer].GetID() + "_default").SourceRectangle;
                                break;
                            case Enum_CocktaillResualt.Fail:
                                ShakeHelper.ShakingEntity(Img_Customer, 5, true, 1f, 20);
                                Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(Customers[numbercustomer].GetID() + "_upset").SourceRectangle;
                                break;
                        }

                        string afterServe1 = Customers[numbercustomer].GetConversationAfterServe(cocktaillResualt);

                        if (afterServe1 != null)
                        {
                            AnimationText = new TaggedTextRevealer(afterServe1, 0.05);
                            AnimationText.Start();
                            canSkipConversation = true;
                            canGoNextConversation = true;
                        }
                        else
                        {
                            haveDoneOrder = false;
                            canSkipConversation = true;
                            canGoNextConversation = false;
                            currentPhase = ConversationPhase.AfterServe;
                            _currentCocktail.ClearAllIngredients();
                            UpdateCocktailBars();
                        }
                    }
                    break;

                case ConversationPhase.AfterServe:
                    if (cocktaillResualt == Enum_CocktaillResualt.None)
                        cocktaillResualt = CalculateAccurateCocktail();

                    string afterServe = Customers[numbercustomer].GetConversationAfterServe(cocktaillResualt);
                    if (afterServe != null)
                    {
                        AnimationText = new TaggedTextRevealer(afterServe, 0.05);
                        AnimationText.Start();
                        canSkipConversation = true;
                        canGoNextConversation = true;
                    }
                    else
                    {
                        currentPhase = ConversationPhase.SmallTalkAfterOrder;
                        cocktaillResualt = Enum_CocktaillResualt.None;
                        haveDoneOrder = false;
                        canSkipConversation = true;
                        canGoNextConversation = false;
                    }
                    break;

                case ConversationPhase.SmallTalkAfterOrder:
                    string chitChatLine = Customers[numbercustomer].GetConversationChitChat(Day);

                    if (chitChatLine != null)
                    {
                        AnimationText = new TaggedTextRevealer(chitChatLine, 0.05);
                        AnimationText.Start();
                        Img_Customer.SourceRectangle = Atlas_CustomerNPC.GetRegion(Customers[numbercustomer].GetID() + "_default").SourceRectangle;
                        canSkipConversation = true;
                        canGoNextConversation = false;
                    }
                    else
                    {
                        stateCocktailResultPanel = Enum_PanelState.Pos2;
                        Customers[numbercustomer].InceaseNumberOfVisitTodya();

                        if (AnimationText.IsFinished())
                        {
                            currentPhase = ConversationPhase.SmallTalkBeforeOrder;
                            RandomTargetCocktail();
                            ActiveMixerAndAlcholButton(false);
                            numbercustomer++;
                            GlobalVariable.AddCustomer();
                            str_currentCocktail_Name = "";
                            stateImgCustomer = Enum_PanelState.Pos2;
                            AnimationText = new TaggedTextRevealer("", 0.05);
                            RP_ConversationCustomer.Text = AnimationText.GetVisibleText();
                        }
                    }
                    break;
            }
            if(ShakeHelper.IsComplete(Img_Customer))
                ShakeHelper.ShakingEntity(Img_Customer, 0.25f, false, speed: 2.5f);
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

        protected string GetRandomCocktailNameFromFavType(HashSet<Enum_TypeOfCocktail> favTypes)
        {
            var matchingCocktails = CocktailDicMaker.CocktailDictionary
                .Where(kv => favTypes.Contains(kv.Value.GetTypeOfCocktail()))
                .Select(kv => kv.Key)
                .ToList();
            if (matchingCocktails.Count == 0)
                return GetRandomCocktailName();
            var random = new Random();
            int randomIndex = random.Next(matchingCocktails.Count);
            return matchingCocktails[randomIndex];
        }

        protected void RandomTargetCocktail()
        {
            // Get the customer's favorite cocktail types
            HashSet<Enum_TypeOfCocktail> favCocktailTypes = Customers[numbercustomer]._FavoriteTypeOfCocktail;



            // Get a random cocktail name that matches the favorite types (fallback handled inside)
            str_targetCocktail_Name = GetRandomCocktailNameFromFavType(favCocktailTypes);

            // Retrieve the cocktail object from the dictionary
            if (!CocktailDicMaker.CocktailDictionary.TryGetValue(str_targetCocktail_Name, out _targetCoctail))
            {
                //fallback if not found for safety
                str_targetCocktail_Name = GetRandomCocktailName();
                CocktailDicMaker.CocktailDictionary.TryGetValue(str_targetCocktail_Name, out _targetCoctail);
            }
            
        }

        public string DecideToUseTypeOfText() {
            if (ListTextTypes.Count > 0)
            {
                Enum_TextType textType = ListTextTypes.Dequeue();

                switch (textType)
                {
                    case Enum_TextType.Normal:
                        // Use the name as-is
                        break;

                    case Enum_TextType.Complex:
                        Debug.WriteLine($"Looking for complex version of {str_targetCocktail_Name}");
                        if (ComplexTextCocktail.TryGetValue(str_targetCocktail_Name, out var complexName))
                        {
                            str_targetCocktail_Name = complexName;
                            Debug.WriteLine($"Found complex version: {str_targetCocktail_Name}");
                            return "Please make me something {{MENU_TEXT}}" + str_targetCocktail_Name + "{{DEFAULT}}.";
                        }
                        else
                        {
                            // Fallback if not found
                            Debug.WriteLine($"No complex version found for {str_targetCocktail_Name}");
                            return "Please make me a {{MENU_TEXT}}" + str_targetCocktail_Name + "{{DEFAULT}}.";
                        }
                        break;
                }
            }
            else
            {
                Debug.WriteLine("No text types left in queue!");
            }
            return "Please make me a {{MENU_TEXT}}" + str_targetCocktail_Name + "{{DEFAULT}}.";
        }

        protected string RandomNPC()
        {
            Random random = new Random();
            int numberNPC = random.Next(1, 5);
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
        protected Enum_CocktaillResualt CalculateAccurateCocktail()
        {
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
        protected void ActiveMixerAndAlcholButton(bool Enable)
        {
            BTN_Alcohol.Enabled = Enable;
            BTN_Mixer.Enabled = Enable;
        }
        protected void BTNIngredeientActive(bool Enable)
        {
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
        protected void BTNMethodVisible(bool visible)
        {
            BTN_Stiring.Visible = visible;
            BTN_Shaking.Visible = visible;
            BTN_Reset_OnTable.Visible = visible;
        }
        protected void ShowMinigame(Enum_MiniGameType method)
        {
            openMinigamePanel = Enum_PanelState.Open;

            if (method == Enum_MiniGameType.Shaking)
            {
                P_Minigame.Enabled = true;
                P_Minigame.Visible = true;

                P_Minigame_Stirring.Enabled = false;
                P_Minigame_Stirring.Visible = false;

                P_Minigame_Shaking.Enabled = true;
                P_Minigame_Shaking.Visible = true;
            }
            if (method == Enum_MiniGameType.Stiring)
            {
                P_Minigame.Enabled = true;
                P_Minigame.Visible = true;

                P_Minigame_Stirring.Enabled = true;
                P_Minigame_Stirring.Visible = true;

                P_Minigame_Shaking.Enabled = false;
                P_Minigame_Shaking.Visible = false;
            }
            if (method == Enum_MiniGameType.None)
            {
                P_Minigame_Stirring.Enabled = false;
                P_Minigame_Stirring.Visible = false;

                P_Minigame_Shaking.Enabled = false;
                P_Minigame_Shaking.Visible = false;
            }
        }
        protected void ShowMinigame(bool enable)
        {

        }
        protected void ResetUI()
        {
            openAlcoholPanel = false;
            openMixerPanel = false;
            openMinigamePanel = Enum_PanelState.Close;
            stateBeforeServePanel = Enum_PanelState.InitPosSlide;
            BTN_AddIce.Enabled = true;
            BTN_AddIce.Visible = true;
            BTNIngredeientActive(true);
            BTNMethodActive(false);
            BTNMethodVisible(false);
            VisibleMakingCocktailVisual(false);
            cooldownTime_SFX_Shaking = 2f;
            cooldownTime_SFX_Stiring = 1.2f;
            canPlaySFX_Shaking = true;
            canPlaySFX_Stiring = true;
        }
        protected void EnableBTNBeforeServe(bool eneble)
        {
            BTN_AddIce.Enabled = eneble;
            BTN_Serve.Enabled = eneble;
            BTN_Rest_BeforeServe.Enabled = eneble;
        }
        //-------------------------Conversation---------------------
        protected void SetNewTextForConversation(TaggedTextRevealer _animationText, string _txt)
        {
            _animationText = new TaggedTextRevealer(_txt, 0.05);
        }

        protected void EnableOrderPanel(bool enable)
        {
            ImgP_OrderPanel.Enabled = enable;
            ImgP_OrderPanel.Visible = enable;
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
            BG_ProgressBar.FillColor = Color.RosyBrown;
            BG_ProgressBar.Offset = new Vector2(50, 0);
            BG_ProgressBar.Padding = Vector2.Zero;

            Texture2D T_ProgressBar = new Texture2D(Core.GraphicsDevice, 1, 1);
            T_ProgressBar.SetData(new[] { Color.GreenYellow });
            ProgressBar = new Panel(new Vector2(40, 10), PanelSkin.Simple, Anchor.BottomCenter);
            ProgressBar.SetCustomSkin(T_ProgressBar);

            Texture2D T_TargetZone = new Texture2D(Core.GraphicsDevice, 1, 1);
            T_TargetZone.SetData(new[] { Color.DarkGreen * 0.75f });
            TargetZone = new Panel(new Vector2(40, 50), PanelSkin.Simple, Anchor.BottomCenter);
            TargetZone.SetCustomSkin(T_TargetZone);

            Texture2D T_Pointing = new Texture2D(Core.GraphicsDevice, 1, 1);
            T_Pointing.SetData(new[] { Color.LightBlue });
            Pointing = new Panel(new Vector2(40, 10), PanelSkin.Simple, Anchor.BottomCenter);
            Pointing.SetCustomSkin(T_Pointing);

            BG_TargetZone.AddChild(Pointing);
            BG_TargetZone.AddChild(TargetZone);
            P_Minigame_Shaking.AddChild(BG_TargetZone);

            BG_ProgressBar.AddChild(ProgressBar);
            BG_TargetZone.AddChild(BG_ProgressBar);
        }
        public void InitStiringMinigameUI()
        {
            PB_Stirring = new CustomProgressBar(0, (int)StiringMinigame.ProgressBar_SuccessTimeToWin, new Vector2(XSizeBar_Stiring - PaddingLR_Bar_Stiring, 50), null, null, Anchor.BottomCenter);
            PB_Stirring.Value = (int)StiringMinigame.PointingArrow_CurrentValue;
            PB_Stirring.Locked = true;
            PB_Stirring.Offset = new Vector2(0, 70);
            PB_Stirring.SliderSkin = SliderSkin.Default;
            PB_Stirring.ProgressFill.FillColor = Color.Yellow;

            Utilities.ShakeHelper.ShakingEntity(PB_Stirring, 0.3f, false, 0.3f, 10);
            ShakeHelper.StopShake(PB_Stirring);


            BG_Stirring_TargetZone = new Panel(new Vector2(XSizeBar_Stiring - PaddingLR_Bar_Stiring, 50), PanelSkin.Simple, Anchor.BottomCenter);
            BG_Stirring_TargetZone.Offset = new Vector2(0, 10);
            BG_Stirring_TargetZone.FillColor = Color.Gray;
            BG_Stirring_TargetZone.Padding = new Vector2(0, 0);

            Texture2D T_Stirring_TargetZone = new Texture2D(Core.GraphicsDevice, 1, 1);
            T_Stirring_TargetZone.SetData(new[] { Color.White });

            Img_Stirring_TargetZone = new Image(T_Stirring_TargetZone,new Vector2((StiringMinigame.TargetZone_CurrentSize / (StiringMinigame.MaxSize - StiringMinigame.MinSize)) * (XSizeBar_Stiring - PaddingLR_Bar_Stiring), 50), anchor: Anchor.CenterLeft);
            Img_Stirring_TargetZone.FillColor = Color.LightGreen;
            Img_Stirring_TargetZone.Padding = Vector2.Zero;
            Img_Stirring_TargetZone.Opacity = 128;

            Texture2D T_Arrow = new Texture2D(Core.GraphicsDevice, 1, 1);
            T_Arrow.SetData(new[] { Color.White });

            Arrow_Stirring = new Image(T_Arrow, new Vector2(5, 50), anchor: Anchor.CenterLeft);
            //Arrow_Stirring = new Panel(new Vector2(2, 50), PanelSkin.Simple, Anchor.CenterLeft);
            Arrow_Stirring.Offset = new Vector2((StiringMinigame.PointingArrow_CurrentValue) - 5, 0);
            Arrow_Stirring.FillColor = Color.LightBlue;

            BG_Stirring_TargetZone.AddChild(Img_Stirring_TargetZone);
            BG_Stirring_TargetZone.AddChild(Arrow_Stirring);


            P_Minigame_Stirring.AddChild(PB_Stirring);
            P_Minigame_Stirring.AddChild(BG_Stirring_TargetZone);
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
        }
        public void UpdateMiniGameStiringUI()
        {
            float normalizedMin = (StiringMinigame.TargetZone_Init) - (StiringMinigame.TargetZone_CurrentSize / 2);

            float normalizedWidth = (StiringMinigame.TargetZone_CurrentSize)
                                    / (StiringMinigame.MaxSize - StiringMinigame.MinSize);

            Img_Stirring_TargetZone.Offset = new Vector2((XSizeBar_Stiring - PaddingLR_Bar_Stiring) * (normalizedMin / 100), 0);
            Img_Stirring_TargetZone.Size = new Vector2(normalizedWidth * (XSizeBar_Stiring - PaddingLR_Bar_Stiring), 50);

            float normalizedArrow = (StiringMinigame.PointingArrow_CurrentValue - StiringMinigame.MinSize)
                                    / (StiringMinigame.MaxSize - StiringMinigame.MinSize);

            Arrow_Stirring.Offset = new Vector2(normalizedArrow * (XSizeBar_Stiring - PaddingLR_Bar_Stiring) - (Arrow_Stirring.Size.X / 2), 0);

            PB_Stirring.Value = StiringMinigame.ProgressBar_Success;
        }

        //------------------- Initi Cocktail Result
        public void InitCocktailResultEntity()
        {
            CocktailResult_Atlas = TextureAtlas.FromFile(Content, "images/Cocktail/CocktailResult_Define.xml");
            Img_CocktailResult = new Image(CocktailResult_Atlas.Texture, new Vector2(160, 160), anchor: Anchor.CenterLeft);
            Img_CocktailResult.SourceRectangle = CocktailResult_Atlas.GetRegion("Cosmopolitan").SourceRectangle;
            Img_CocktailResult.Offset = new Vector2(1920, 75);

        }

        //------------------------ Book Recipes-------------------
        public void InitBookRecipes()
        {
            Texture2D T_BookRecipes = Content.Load<Texture2D>("images/UI/RecipeBook/Book_Base");
            Recipes_Atlas = TextureAtlas.FromFile(Content, "images/UI/RecipeBook/Recipes_Define.xml");
            TotalPages = Recipes_Atlas.GetRegionCount() / 2;

            P_BGBookRecipes = new Panel(new Vector2(1920, 1080), PanelSkin.Default, Anchor.Center);
            P_BGBookRecipes.FillColor = Color.Black * 0.75f;

            Img_BookRecipes = new Image(T_BookRecipes, new Vector2(1100, 750), ImageDrawMode.Stretch, Anchor.TopLeft);
            Img_BookRecipes.Offset = new Vector2(66, 63);

            Img_LeftPage = new Image(Recipes_Atlas.Texture, new Vector2(480, 700), ImageDrawMode.Stretch, Anchor.TopLeft);
            Img_LeftPage.SourceRectangle = Recipes_Atlas.GetRegion("Recipe_01_L").SourceRectangle;
            Img_LeftPage.Offset = new Vector2(25, 0);

            Img_RightPage = new Image(Recipes_Atlas.Texture, new Vector2(480, 700), ImageDrawMode.Stretch, Anchor.TopLeft);
            Img_RightPage.SourceRectangle = Recipes_Atlas.GetRegion("Recipe_01_R").SourceRectangle;
            Img_RightPage.Offset = new Vector2(505, 0);


            BTN_PreviousPage = new Button("", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(166/2, 98/2));
            BTN_PreviousPage.OnClick += (Entity e) =>
            {
                ChangePage(Enum_Page.PreviousPage);
                UpdatePageView();
                Core.Audio.PlaySoundEffect(SFX_Book_Turnpage);
            };
            BTN_PreviousPage.OnMouseDown += (Entity e) =>
            {
                BTN_PreviousPage.Offset += new Vector2(0, -5);
                BTN_PreviousPage.FillColor = Color.DarkGray;
            };
            BTN_PreviousPage.OnMouseReleased += (Entity e) =>
            {
                BTN_PreviousPage.Offset += new Vector2(0, +5);
                BTN_PreviousPage.FillColor = Color.White;
            };
            BTN_PreviousPage.Offset = new Vector2(-20, 0);
            Texture2D T_BTN_PreviousPage = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Left");
            Texture2D T_BTN_PreviousPage_hover = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Left_Hover");
            BTN_PreviousPage.SetCustomSkin(T_BTN_PreviousPage, T_BTN_PreviousPage_hover, T_BTN_PreviousPage);


            BTN_NextPage = new Button("", ButtonSkin.Default, Anchor.BottomRight, new Vector2(166/2, 98/2));
            BTN_NextPage.OnClick += (Entity e) =>
            {
                ChangePage(Enum_Page.NextPage);
                UpdatePageView();
                Core.Audio.PlaySoundEffect(SFX_Book_Turnpage);
            };
            BTN_NextPage.OnMouseDown += (Entity e) =>
            {
                BTN_NextPage.Offset += new Vector2(0, -5);
                BTN_NextPage.FillColor = Color.DarkGray;
            };
            BTN_NextPage.OnMouseReleased += (Entity e) =>
            {
                BTN_NextPage.Offset += new Vector2(0, +5);
                BTN_NextPage.FillColor = Color.White;
            };
            BTN_NextPage.Offset = new Vector2(20, 0);
            Texture2D T_BTN_NextPage = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Right");
            Texture2D T_BTN_NextPage_hover = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Right_Hover");
            BTN_NextPage.SetCustomSkin(T_BTN_NextPage, T_BTN_NextPage_hover, T_BTN_NextPage);


            Button BTN_CloseBookRecipes = new Button("", ButtonSkin.Default, Anchor.TopRight, new Vector2(98/2, 140/2));
            BTN_CloseBookRecipes.OnClick += (Entity e) =>
            {
                ToggleBookRecipes();
                Core.Audio.PlaySoundEffect(SFX_Book_Open_Close);
                
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
            BTN_CloseBookRecipes.Offset = new Vector2(80, -40);
            Texture2D T_BTN_CloseBookRecipes = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Close");
            Texture2D T_BTN_CloseBookRecipes_hover = Content.Load<Texture2D>("images/UI/RecipeBook/Recipe_Button_Close_Hover");
            BTN_CloseBookRecipes.SetCustomSkin(T_BTN_CloseBookRecipes, T_BTN_CloseBookRecipes, T_BTN_CloseBookRecipes);


            Img_BookRecipes.AddChild(Img_LeftPage);
            Img_BookRecipes.AddChild(Img_RightPage);
            Img_BookRecipes.AddChild(BTN_PreviousPage);
            Img_BookRecipes.AddChild(BTN_NextPage);
            Img_BookRecipes.AddChild(BTN_CloseBookRecipes);


            EnableBookRecipes(false);

            BTN_PreviousPage.Enabled = false;
            BTN_PreviousPage.Visible = false;



        }
        public enum Enum_Page
        {
            LeftPage,
            RightPage,
            PreviousPage,
            NextPage
        }
        public void ChangePage(Enum_Page page)
        {
            // Handle page change
            if (page == Enum_Page.NextPage && CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
            else if (page == Enum_Page.PreviousPage && CurrentPage > 1)
            {
                CurrentPage--;
            }

            // Enforce valid page bounds
            if (CurrentPage <= 1)
            {
                CurrentPage = 1;
                BTN_PreviousPage.Enabled = false;
                BTN_PreviousPage.Visible = false;
            }
            else
            {
                BTN_PreviousPage.Enabled = true;
                BTN_PreviousPage.Visible = true;
            }

            if (CurrentPage >= TotalPages)
            {
                CurrentPage = TotalPages;
                BTN_NextPage.Enabled = false;
                BTN_NextPage.Visible = false;
            }
            else
            {
                BTN_NextPage.Enabled = true;
                BTN_NextPage.Visible = true;
            }

            // Refresh the display
            UpdatePageView();
        }
        private void UpdatePageView()
        {
            string leftKey = $"Recipe_{CurrentPage:D2}_L";
            string rightKey = $"Recipe_{CurrentPage:D2}_R";
            Img_LeftPage.SourceRectangle = Recipes_Atlas.GetRegion(leftKey).SourceRectangle;
            Img_RightPage.SourceRectangle = Recipes_Atlas.GetRegion(rightKey).SourceRectangle;
        }
        public void EnableBookRecipes(bool _Enable)
        {
            Img_BookRecipes.Visible = _Enable;
            Img_BookRecipes.Enabled = _Enable;
        }
        public void ToggleBookRecipes()
        {
            bool isActive = Img_BookRecipes.Visible;
            EnableBookRecipes(!isActive);
            if (!EndTutorial) 
            {
                stateImgCustomer = Enum_PanelState.Pos1;
                Core.Audio.PlaySoundEffect(SFX_Welcome);

                ShakeHelper.SetShakeAmplitude(Img_Customer, 2f);
                ShakeHelper.SetShakeSpeed(Img_Customer, 10f);
            }
            EndTutorial = true;
        }
        public void OpenBookOnTutorial()
        {
            EnableBookRecipes(true);
            CurrentPage = 1;
            UpdatePageView();
            BTN_PreviousPage.Enabled = false;
            BTN_PreviousPage.Visible = false;
        }

        //---------------------- Making Cocktail Visual On table-----------------------
        public void InitMakingVisualCocktail()
        {
            P_MakingCocktailVisual = new Panel(new Vector2(300, 200), PanelSkin.None, Anchor.TopCenter);
            P_MakingCocktailVisual.Padding = new Vector2(0, 0);
            P_MakingCocktailVisual.Offset = new Vector2(-100, -25);

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

            foreach (var bar in AllBars)
            {
                bar.Size = new Vector2(bar.Texture.Width, bar.Texture.Height);
                bar.Opacity = 0; // start hidden
                bar.Locked = true;
            }

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
            VisibleMakingCocktailVisual(false);

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
        public void VisibleMakingCocktailVisual(bool visible)
        {
            P_MakingCocktailVisual.Visible = visible;
            Img_CocktailBottle.Visible = visible;
        }

        // ----------------------Slide Panel-----------------------
        private void HandlePanel_X_Axis(bool isOpen, Entity panel, int openEndPoint, int closedEndPoint, int speed)
        {
            if (isOpen)
                SlidePanel_X_Axis(panel, openEndPoint, speed, true);
            else
                SlidePanel_X_Axis(panel, closedEndPoint, speed, false);
        }
        public bool SlidePanel(Entity panel, int endPoint, int speed, Enum_SlideDirection direction)
        {
            speed = Math.Abs(speed); // Ensure speed is positive
            switch (direction)
            {
                case Enum_SlideDirection.Right:
                    if (panel.Offset.X < endPoint)
                    {
                        panel.Offset += new Vector2(speed, 0);
                        return false;
                    }
                    break;

                case Enum_SlideDirection.Left:
                    if (panel.Offset.X > endPoint)
                    {
                        panel.Offset -= new Vector2(speed, 0);
                        return false;
                    }
                    break;

                case Enum_SlideDirection.Down:
                    if (panel.Offset.Y < endPoint)
                    {
                        panel.Offset += new Vector2(0, speed);
                        return false;
                    }
                    break;

                case Enum_SlideDirection.Up:
                    if (panel.Offset.Y > endPoint)
                    {
                        panel.Offset -= new Vector2(0, speed);
                        return false;
                    }
                    break;
            }

            return true; // Finished sliding
        }
        public bool SlidePanel_X_Axis(Entity _panel, int _endPoint, int _speed, bool _moreThan)
        {
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

        //---------------Sound and BGM----------------
        public void InitSFX()
        {
            SFX_Serve = Content.Load<SoundEffect>("Sound/Sound_Effect/Serve");
            SFX_Welcome = Content.Load<SoundEffect>("Sound/Sound_Effect/Welcome");
            SFX_AddIce = Content.Load<SoundEffect>("Sound/Sound_Effect/Ice");
            SFX_PressedBTN = Content.Load<SoundEffect>("Sound/Sound_Effect/Interface_Selection");
            SFX_Book_Turnpage = Content.Load<SoundEffect>("Sound/Sound_Effect/Book_Turnpage.");
            SFX_Lemon = Content.Load<SoundEffect>("Sound/Sound_Effect/Lemon");
            SFX_Peppermint = Content.Load<SoundEffect>("Sound/Sound_Effect/Peppermint");
            SFX_Pouring = Content.Load<SoundEffect>("Sound/Sound_Effect/Pouring");
            SFX_Shaking = Content.Load<SoundEffect>("Sound/Sound_Effect/Shaking");
            SFX_Stiring = Content.Load<SoundEffect>("Sound/Sound_Effect/Stiring");
            SFX_Book_Open_Close = Content.Load<SoundEffect>("Sound/Sound_Effect/Book_Open_Close");
            SFX_Open_Interface = Content.Load<SoundEffect>("Sound/Sound_Effect/Open_Interface");
        }
        public void InitBGM()
        {
            BGM_themeSong01 = Content.Load<Song>("Sound/Background_Music/ThemeSong01");
            BGM_themeSong02 = Content.Load<Song>("Sound/Background_Music/ThemeSong02");
            Core.Audio.PlaySong(BGM_themeSong01, true);
            Core.Audio.SongVolume = 0.75f;
        }

        public void UpdateSong() {
            if (Core.Audio.IsSongFinished && shouldPlayBGM_themeSong01)
            {
                Core.Audio.PlaySong(BGM_themeSong02, false);
                Core.Audio.SongVolume = 0.75f;
                shouldPlayBGM_themeSong01 = false;
                shouldPlayBGM_themeSong02 = true;
            }
            else if (Core.Audio.IsSongFinished && shouldPlayBGM_themeSong02)
            {
                Core.Audio.PlaySong(BGM_themeSong01, false);
                Core.Audio.SongVolume = 0.75f;
                shouldPlayBGM_themeSong02 = false;
                shouldPlayBGM_themeSong01 = true;
            }
        }

        //----------------NPC Init-------------------
        private void InitNpc()
        {
            BaseCharacter Walter = new BaseCharacter("Walter");
            Walter.AddDayConversationFromJson("Content/Conversation/Walter_Conversation.json");
            Walter.SetID("NPC_01");
            Walter.AddFavoriteTypeOfCocktail(Enum_TypeOfCocktail.LowAlcohol);
            Walter.AddFavoriteTypeOfCocktail(Enum_TypeOfCocktail.NonAlcoholic);

            BaseCharacter Owen = new BaseCharacter("Owen");
            Owen.AddDayConversationFromJson("Content/Conversation/Owen_Conversation.json");
            Owen.SetID("NPC_02");
            Owen.AddFavoriteTypeOfCocktail(Enum_TypeOfCocktail.LowAlcohol);

            BaseCharacter Freya = new BaseCharacter("Freya");
            Freya.AddDayConversationFromJson("Content/Conversation/Freya_Conversation.json");
            Freya.SetID("NPC_03");
            Freya.AddFavoriteTypeOfCocktail(Enum_TypeOfCocktail.HighAlcohol);

            BaseCharacter Isla = new BaseCharacter("Isla");
            Isla.AddDayConversationFromJson("Content/Conversation/Isla_Conversation.json");
            Isla.SetID("NPC_04");
            Isla.AddFavoriteTypeOfCocktail(Enum_TypeOfCocktail.NonAlcoholic);

            BaseCharacter Cole = new BaseCharacter("Cole");
            Cole.AddDayConversationFromJson("Content/Conversation/Cole_Conversation.json");
            Cole.SetID("NPC_05");
            Cole.AddFavoriteTypeOfCocktail(Enum_TypeOfCocktail.HighAlcohol);

            Customers.Add(Walter);
            Customers.Add(Owen);
            Customers.Add(Freya);
            Customers.Add(Isla);
            Customers.Add(Cole);
        }
        public void ShuffleCustomers()
        {
            Random rng = new Random();
            int n = Customers.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                var temp = Customers[n];
                Customers[n] = Customers[k];
                Customers[k] = temp;
            }
        }
        private void PlaySoundEffectWithRandomPitch(SoundEffect sfx)
        {
            var instance = sfx.CreateInstance();

            // Define 3 possible pitch values
            float[] pitches = { -0.5f, 0.0f, 0.5f };

            // Pick one at random
            instance.Pitch = pitches[new Random().Next(pitches.Length)];


            instance.Play();
        }
        private void PlaySoundEffectWithRandomPitch(SoundEffect sfx, float Volume)
        {
            var instance = sfx.CreateInstance();
            // Define 3 possible pitch values
            float[] pitches = { -0.5f, 0.0f, 0.5f };
            // Pick one at random
            instance.Pitch = pitches[new Random().Next(pitches.Length)];
            instance.Volume = Volume;
            instance.Play();
        }

        //--------------- Fade ----------------------
        public void InitFadePanel()
        {
            P_Fade = new Panel(new Vector2(2300, 1200), PanelSkin.Simple, Anchor.Center);
            //P_Fade.FillColor = Color.Black;
            P_Fade.Opacity = 255;
            RP_Fade = new RichParagraph("Day" + GlobalVariable.Day);
            RP_Fade.FontOverride = BoldFont;
            RP_Fade.FillColor = Color.Red;
            RP_Fade.Scale = 2f;
            P_Fade.AddChild(RP_Fade);
            UserInterface.Active.AddEntity(P_Fade);
        }
        public void EnableFadePanel(bool enable)
        {
            P_Fade.Enabled = enable;
            P_Fade.Visible = enable;
            if (enable) { 
                RP_Fade.Text = "DAY" + GlobalVariable.Day;
            }
            if (!enable)
            {
                RP_Fade.Text = "END DAY";
            }
        }

        public void EnableAllBTN(bool enable) { 
            BTNIngredeientActive(enable);
            BTNMethodActive(enable);
            //BTN_AddIce.Enabled = enable;
            BTN_Serve.Enabled = enable;
            BTN_Rest_BeforeServe.Enabled = enable;
            //BTN_Alcohol.Enabled = enable;
            //BTN_Mixer.Enabled = enable;
            BTN_Reset_OnTable.Enabled = enable;
            BTN_Stiring.Enabled = enable;
            BTN_Shaking.Enabled = enable;
            BTN_BookRecipes.Enabled = enable;
            BTN_BookRecipes.Enabled = enable;

        }

    }

    
}
