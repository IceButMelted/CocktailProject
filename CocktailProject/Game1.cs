using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CocktailProject.ClassCocktail;
using CocktailProject.ClassTime;

using MonoGameLibrary;
using MonoGameLibrary.Graphics;

// using GeonBit UI elements
using GeonBit.UI;
using GeonBit.UI.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace CocktailProject;

public class Game1 : Core
{
    Paragraph p_timer;

    Panel _titlePanel;
    Button _startBTN;
    Button _exitBTN;

    Panel _inGamePanelCatagoly;
    Panel _inGamePanelLeft;
    Panel _inGamePanelRight;
    Panel _inGame_Alcohol;
    Panel _inGame_Mixer;
    Panel _inGame_MethodScreen;
    Image _imgCustomerNPC; 

    Panel _summaryPanelCataory;
    Panel _summaryPanel;
    int _summaryPanelWidth = 600;
    int _summaryPanelHeight = 700;
    RichParagraph _summaryPanelText;
    CheckBox _checkBoxRent;
    Paragraph p_RentFeeCost;
    CheckBox _checkBoxAlcoholFee;
    Paragraph p_AlcoholFeeCost;
    CheckBox _checkBoxMixerFee;
    Paragraph p_MixerFeeCost;

    Panel _OrederPanel;
    RichParagraph _OrederText;

    Panel _UI_Table;
    Button _BTN_Alcohol;
    Button _BTN_Mixer;

    #region Button
    Button _BTN_AddIce;
    Button _BTN_Shake;
    Button _BTN_Mix;
    Button _BTN_Serve;
    Button _BTN_Reset;

    Button _BTN_Gin;
    Button _BTN_Vodka;
    Button _BTN_TripleSec;
    Button _BTN_Vermoth;

    Button _BTN_Lemon;
    Button _BTN_Syrup;
    Button _BTN_Soda;
    Button _BTN_Grape;
    Button _BTN_Cranberry;
    Button _BTN_Perpermint;

    Button _BTN_PaperMakeCocktail;
    #endregion  

    Paragraph p_currentCocktailInfo;
    Paragraph p_targetCockTailInfo;
    Paragraph p_result;

    //testing function in editor
#if DEBUG
    TaggedTextRevealer taggedTextRevealer;
    Button _BTN_Randomcocktail;
    RichParagraph withCustomer;
#endif

    int width_ReciptPanel = 960;
    int heigh_ReciptPanel = 550;
    int OffsetY_RecipPanel = 50;
    int Padding_ReciptPanel = 80;

    int BTN_Open_Width = 75;
    int BTN_Open_Height = 150;
    int BTN_Open_Padding = 20;

    bool _isOpenAlcohol = false;
    bool _isOpenMixer = false;
    bool _isOpenMethod = false;


    #region Cocktail
    private static string str_targetCocktail_Name;
    private Cocktail _targetCoctail = new Cocktail();
    private CocktailBuilder _currentCocktail = new CocktailBuilder();
    private int MixPartCount = 0;

    private int _currentMoney = 0;
    #endregion

    #region Image Sprite Atlas
    TextureAtlas Atlas_CustomerNPC;
    #endregion

    private int attemptCount = 0;
    private int maxAttempts = 3; // Maximum number of attempts allowed

    private AnimatedSprite _slime;
    private Vector2 _slimePosition = new Vector2(100, 100);

    private Timer timer;

    public Game1() : base("CocktialProject", 1920, 1080, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        timer = new Timer(20);
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        Atlas_CustomerNPC = TextureAtlas.FromFile(Content, "images/Customer/CustomerNPC_Define.xml");

        _slime = atlas.CreateAnimatedSprite("slime-animation");
        _slime.Scale = new Vector2(4.0f, 4.0f);

        #region Init Ui
        UserInterface.Initialize(Content, BuiltinThemes.hd);

        #region TitlePanel
        /// Add Chiildren to the panels
        _titlePanel = new Panel(new Vector2(500, 400), PanelSkin.Default, Anchor.Center);
        _startBTN = new Button("Start Game", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(200, 50));
        _startBTN.Padding = new Vector2(0, 25);
        _startBTN.ToolTipText = "Click to start the game!";
        _startBTN.OnClick = (Entity entity) =>
        {
            RandomeTargetCocktail();
            timer.Start();
            Debug.WriteLine($"Target Cocktail: {str_targetCocktail_Name}");
            _titlePanel.Enabled = false;
            _titlePanel.Visible = false;
            Enable_InGamePanel();

            UserInterface.Active.SetCursor(CursorType.Default);
        };
        _exitBTN = new Button("Exit Game", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(200, 50));
        _exitBTN.Padding = new Vector2(0, 25);
        _exitBTN.OnClick = (Entity entity) =>
        {
            Exit();
        };


        _titlePanel.AddChild(_startBTN);
        _titlePanel.AddChild(_exitBTN);
        #endregion

        #region InGamePanel

        #region InGamePanel Init
#if DEBUG
        _BTN_Randomcocktail = new Button("Random Cocktail", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(200, 50), new Vector2(0, 0));
        _BTN_Randomcocktail.OnMouseDown = (Entity e) =>
        {
            RandomeTargetCocktail();
        };
        UserInterface.Active.AddEntity(_BTN_Randomcocktail);

        p_timer = new Paragraph("Timer: " + timer, Anchor.TopCenter, new Vector2(200, 50), new Vector2(0, 0));

#endif
        _UI_Table = new Panel(new Vector2(2450, 360), PanelSkin.Default, Anchor.BottomCenter);
        _inGame_MethodScreen = new Panel(new Vector2(width_ReciptPanel - 220, 400), PanelSkin.Default, Anchor.BottomLeft, new Vector2(width_ReciptPanel, 100));

        _imgCustomerNPC = new Image(Atlas_CustomerNPC.GetRegion("NPC1").GetTexture2D(), new Vector2(400, 500), anchor: Anchor.Center);
        _imgCustomerNPC.SourceRectangle = Atlas_CustomerNPC.GetRegion("NPC1").SourceRectangle;

        _inGamePanelCatagoly = new Panel(new Vector2(1920, 1080), PanelSkin.None, Anchor.Center);
        _inGamePanelCatagoly.Padding = new Vector2(0, 0);
        //_inGamePanelCatagoly.SetCustomSkin(atlas.GetRegion("slime-1").GetTexture2D());

        /// Create the in-game panel
        _inGamePanelRight = new Panel(new Vector2(960, 1080), PanelSkin.Default, Anchor.TopRight);
        _inGamePanelRight.Padding = new Vector2(0, 0);
        _inGamePanelRight.Enabled = false;
        _inGamePanelRight.Visible = false;
        _inGamePanelRight.SetCustomSkin(atlas.GetRegion("slime-1").GetTexture2D());

        _inGamePanelLeft = new Panel(new Vector2(960, 1080-300), PanelSkin.None, Anchor.TopLeft);
        _inGamePanelLeft.Padding = new Vector2(0, 0);
        _inGamePanelLeft.Enabled = false;
        _inGamePanelLeft.Visible = false;

        _BTN_AddIce = new Button("AddIce", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(200, 200), new Vector2(0, 200));
        _BTN_AddIce.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddIce(true);
            _BTN_AddIce.Enabled = false;
        };

        _BTN_Shake = new Button("Shake", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(100, 100), new Vector2(700, 300));
        _BTN_Shake.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.UseMethod(Enum_Method.Shaking);
            _BTN_Shake.Enabled = false;
            _BTN_Mix.Enabled = false;

            _isOpenMethod = true;
            DiableBTN_inGame();
            DisableBTNMakeCocktail();
            _currentCocktail.SetTypeOfCocktailBySearch();
            Debug.WriteLine("shakin");
        };

        _BTN_Mix = new Button("Mix", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(100, 100), new Vector2(800, 300));
        _BTN_Mix.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.UseMethod(Enum_Method.Mixing);
            _BTN_Mix.Enabled = false;
            _BTN_Shake.Enabled = false;

            _isOpenMethod = true;
            DiableBTN_inGame();
            DisableBTNMakeCocktail();
            _currentCocktail.SetTypeOfCocktailBySearch();
            Debug.WriteLine("mixing");
        };
        _BTN_Serve = new Button("Serve", ButtonSkin.Default, Anchor.BottomCenter, new Vector2(150, 75), new Vector2(0, 0));
        _BTN_Serve.OnMouseDown = (Entity e) =>
        {
            // Logic for Serve button click
            if (!CocktailDicMaker.CocktailDictionary.TryGetValue(str_targetCocktail_Name, out Cocktail targetCocktail))
            {
                Console.WriteLine("Error: Target cocktail not found!");
                return;
            }

            float price = CalcualatePrice(targetCocktail);

            if (targetCocktail.Equals(_currentCocktail))
            {
                p_result.Text = "You made a cocktail: " + str_targetCocktail_Name + "!" + $"{price}";
            }
            else {
                p_result.Text = "You made a cocktail, but it is not " + str_targetCocktail_Name + "!" +$"{price}";
            }

            _isOpenMethod = false;

            EnableBTNALLInGamePanel();
            Debug.WriteLine("Serve button clicked!");
        };

        _BTN_Reset = new Button("Reset", ButtonSkin.Default, Anchor.BottomRight, new Vector2(100, 100), new Vector2(0, 0));
        _BTN_Reset.OnMouseDown = (Entity e) =>
        {
            _isOpenMethod = false;

            EnableBTNALLInGamePanel();
            _currentCocktail.ClearAllIngredients();
            MixPartCount = 0;

            Debug.WriteLine("Reset button clicked!");
        };


        _BTN_PaperMakeCocktail = new Button("Paper Make Cocktail", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(150, 75), new Vector2(0, 20));
        _BTN_PaperMakeCocktail.OnMouseDown = (Entity e) =>
        {
            Debug.WriteLine("Paper Make Cocktail button clicked!");
        };

        /// Create the in-game alcohol and mixer panels
        /// Alchol
        _inGame_Alcohol = new Panel(new Vector2(width_ReciptPanel, heigh_ReciptPanel), PanelSkin.Default, Anchor.TopLeft, new Vector2(width_ReciptPanel, OffsetY_RecipPanel));
        _inGame_Alcohol.Padding = new Vector2(20, 10);
        _BTN_Alcohol = new Button("Alchol", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(BTN_Open_Width, BTN_Open_Height), new Vector2(-BTN_Open_Width - BTN_Open_Padding, BTN_Open_Height));
        _BTN_Alcohol.OnMouseDown = (Entity e) =>
        {
            if (!_isOpenAlcohol)
            {
                _isOpenAlcohol = true;
                _isOpenMixer = false;
                _inGame_Alcohol.BringToFront();
            }
            else
            {
                _isOpenAlcohol = false;
                _isOpenMixer = false;
            }
        };

        _BTN_Gin = new Button("Gin", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Gin.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Gin, 1);
            MixPartCount++;
            // Logic for Gin button click
            Debug.WriteLine("Gin button clicked!");
        };

        _BTN_Vodka = new Button("Vodka", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(30, 0));
        _BTN_Vodka.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vodka, 1);
            MixPartCount++;
            Debug.WriteLine("Vodka button clicked!");
        };

        HorizontalLine hz = new HorizontalLine(Anchor.Center);

        _BTN_TripleSec = new Button("Triple Sec", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_TripleSec.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Triplesec, 1);
            MixPartCount++;
            Debug.WriteLine("Triple Sec button clicked!");
        };

        _BTN_Vermoth = new Button("Vermouth", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Vermoth.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vermouth, 1);
            MixPartCount++;
            Debug.WriteLine("Vermouth button clicked!");
        };

        /// Mixer
        _inGame_Mixer = new Panel(new Vector2(width_ReciptPanel, heigh_ReciptPanel), PanelSkin.Default, Anchor.TopLeft, new Vector2(width_ReciptPanel, OffsetY_RecipPanel));
        _inGame_Mixer.Padding = new Vector2(20, 10);
        _BTN_Mixer = new Button("Mixer", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(BTN_Open_Width, BTN_Open_Height), new Vector2(-BTN_Open_Width - BTN_Open_Padding, 0));
        _BTN_Mixer.OnMouseDown = (Entity e) =>
        {
            if (!_isOpenMixer)
            {
                _isOpenMixer = true;
                _isOpenAlcohol = false;
                _inGame_Mixer.BringToFront();
            }
            else
            {
                _isOpenMixer = false;
                _isOpenAlcohol = false;
            }
        };

        _BTN_Lemon = new Button("Lemon", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Lemon.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateMixer(Enum_Mixer.LemonJuice, 1);
            MixPartCount++;
            Debug.WriteLine("Lemon button clicked!");
        };

        _BTN_Syrup = new Button("Syrup", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Syrup.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Syrup, 1);
            MixPartCount++;
            Debug.WriteLine("Syrup button clicked!");
        };

        _BTN_Soda = new Button("Soda", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Soda.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateMixer(Enum_Mixer.Soda, 1);
            MixPartCount++;
            Debug.WriteLine("Soda button clicked!");
        };

        HorizontalLine hz1 = new HorizontalLine(Anchor.Center);

        _BTN_Grape = new Button("Grapefruit juice", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Grape.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateMixer(Enum_Mixer.GrapefruitJuice, 1);
            MixPartCount++;
            Debug.WriteLine("Grapefruit juice button clicked!");
        };

        _BTN_Cranberry = new Button("Cranberry juice", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Cranberry.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateMixer(Enum_Mixer.CanberryJuice, 1);
            MixPartCount++;
            Debug.WriteLine("Cranberry juice button clicked!");
        };

        _BTN_Perpermint = new Button("Paper mint", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Perpermint.OnMouseDown = (Entity e) =>
        {
            _currentCocktail.AddOrUpdateMixer(Enum_Mixer.PepperMint, 1);
            MixPartCount++;
            Debug.WriteLine("Paper mint button clicked!");
        };

        p_currentCocktailInfo = new Paragraph(_currentCocktail.Info(), Anchor.TopLeft, new Vector2(300, 500));
        p_currentCocktailInfo.FillColor = Color.White;

        p_targetCockTailInfo = new Paragraph("Target Cocktail: " + str_targetCocktail_Name + _targetCoctail.Info(), Anchor.TopLeft, new Vector2(300, 500), new Vector2(300, 0));
        p_targetCockTailInfo.FillColor = Color.White;

        
        p_result = new Paragraph("Result: ", Anchor.BottomLeft, new Vector2(500, 200), new Vector2(0, 0));
        p_result.FillColor = Color.White;



        #endregion

        #region AddChild IngamePanel
        _inGame_Alcohol.AddChild(_BTN_Gin);
        _inGame_Alcohol.AddChild(_BTN_Vodka);
        _inGame_Alcohol.AddChild(hz);
        _inGame_Alcohol.AddChild(_BTN_TripleSec);
        _inGame_Alcohol.AddChild(_BTN_Vermoth);
        _inGame_Alcohol.AddChild(_BTN_Alcohol);


        _inGame_Mixer.AddChild(_BTN_Lemon);
        _inGame_Mixer.AddChild(_BTN_Syrup);
        _inGame_Mixer.AddChild(_BTN_Soda);
        _inGame_Mixer.AddChild(hz1);
        _inGame_Mixer.AddChild(_BTN_Grape);
        _inGame_Mixer.AddChild(_BTN_Cranberry);
        _inGame_Mixer.AddChild(_BTN_Perpermint);
        _inGame_Mixer.AddChild(_BTN_Mixer);


        _inGamePanelRight.AddChild(_BTN_AddIce);
        _inGamePanelRight.AddChild(_BTN_Shake);
        _inGamePanelRight.AddChild(_BTN_Mix);
        _inGamePanelRight.AddChild(_BTN_PaperMakeCocktail);
        _inGamePanelRight.AddChild(_BTN_Reset);

        _inGamePanelRight.AddChild(_inGame_Alcohol);
        _inGamePanelRight.AddChild(_inGame_Mixer);
        _inGame_MethodScreen.AddChild(_BTN_Serve);
        _inGamePanelRight.AddChild(_inGame_MethodScreen);

        _inGamePanelLeft.AddChild(_imgCustomerNPC);

        _inGamePanelCatagoly.AddChild(_inGamePanelRight);
        _inGamePanelCatagoly.AddChild(_inGamePanelLeft);
        #endregion

        #endregion

        #region SummaryPanel
        _summaryPanelCataory = new Panel(new Vector2(1920, 1080), PanelSkin.None, Anchor.Center);
        _checkBoxRent = new CheckBox("",Anchor.AutoInline, new Vector2(50,10));
        _checkBoxRent.OnValueChange = (Entity cb) =>
        {
            _summaryPanelText.Text = _checkBoxRent.Checked ? "You have selected to rent the cocktail." : "You have not selected to rent the cocktail.";
        };
        p_RentFeeCost = new Paragraph("Rent Fee: 100", Anchor.AutoInline, new Vector2(200, 50));

        _checkBoxAlcoholFee = new CheckBox("", Anchor.AutoInline, new Vector2(50, 10));
        _checkBoxAlcoholFee.OnValueChange = (Entity cb) =>
        {
            _summaryPanelText.Text = _checkBoxAlcoholFee.Checked ? "You have selected to include alcohol fee." : "You have not selected to include alcohol fee.";
        };
        p_AlcoholFeeCost = new Paragraph("Alcohol Fee: 50", Anchor.AutoInline, new Vector2(200, 50));

        _checkBoxMixerFee = new CheckBox("Mixer Fee");
        _checkBoxMixerFee.OnValueChange = (Entity cb) =>
        {
            _summaryPanelText.Text = _checkBoxMixerFee.Checked ? "You have selected to include mixer fee." : "You have not selected to include mixer fee.";
        };

        #region SummaryPanel Init
        _summaryPanel = new Panel(new Vector2(_summaryPanelWidth, _summaryPanelHeight), PanelSkin.Default, Anchor.Center);
        _summaryPanelText = new RichParagraph("Summary Panel", Anchor.Center, new Vector2(_summaryPanelWidth, _summaryPanelHeight), new Vector2(0, 0));
        _summaryPanelText.Text = @"This text will have default color, but {{RED}}this part will be red{{DEFAULT}}. This text will have regular weight, but {{BOLD}}this part will be bold{{DEFAULT}}.";
        #endregion

        #region summaryPanel AddChild
        _summaryPanelCataory.AddChild(_summaryPanel);
        //_summaryPanel.AddChild(_summaryPanelText);
        _summaryPanel.AddChild(_checkBoxRent);
        _summaryPanel.AddChild(p_RentFeeCost);
        HorizontalLine hz001 = new HorizontalLine();
        hz001.Visible = true;
        _summaryPanel.AddChild(hz001);
        _summaryPanel.AddChild(_checkBoxAlcoholFee);
        _summaryPanel.AddChild(p_AlcoholFeeCost);
        HorizontalLine hz002 = new HorizontalLine();
        hz001.Visible = true;
        _summaryPanel.AddChild(hz002);

        //_summaryPanel.AddChild(_checkBoxAlcoholFee);
        //_summaryPanel.AddChild(_checkBoxMixerFee);

        _summaryPanelCataory.Visible = false;
        _summaryPanelCataory.Enabled = false;
        #endregion

        #endregion

        UserInterface.Active.SetCursor(CursorType.Default);

        UserInterface.Active.AddEntity(_UI_Table);
        _UI_Table.SendToBack();

        UserInterface.Active.AddEntity(_inGamePanelCatagoly);
        UserInterface.Active.AddEntity(_summaryPanelCataory);
        UserInterface.Active.AddEntity(_titlePanel);
       

#if DEBUG
        UserInterface.Active.AddEntity(p_currentCocktailInfo);
        UserInterface.Active.AddEntity(p_targetCockTailInfo);
        UserInterface.Active.AddEntity(p_result);
        UserInterface.Active.AddEntity(p_timer);

        string Astring = @"GAMEER ins isder{{RED}}ashahahah{{DEFAULT}}";
        RichParagraph simple = new RichParagraph(Astring, Anchor.CenterLeft, new Vector2(500, 200), new Vector2(200, 0));
        withCustomer = new RichParagraph(".", Anchor.CenterLeft, new Vector2(500, 200), new Vector2(500, 0));
        taggedTextRevealer = new TaggedTextRevealer(Astring, 0.05);

        UserInterface.Active.AddEntity(simple);
        UserInterface.Active.AddEntity(withCustomer);   


#endif
        #endregion

        taggedTextRevealer.Start();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
#if DEBUG
        if (Input.Keyboard.WasKeyJustPressed(Keys.E)) {
            RandomeTargetCocktail();
        }
        if (Input.Keyboard.WasKeyJustPressed(Keys.R))
        {
            taggedTextRevealer.Start();
        }
        if (Input.Keyboard.WasKeyJustPressed(Keys.F))
        {
            taggedTextRevealer.Stop();
        }
        if (Input.Keyboard.WasKeyJustPressed(Keys.T))
        {
            taggedTextRevealer.Skip();
        }
        if(Input.Keyboard.WasKeyJustPressed(Keys.Y))
        {
            taggedTextRevealer.Reset();
        }
        taggedTextRevealer.Update(gameTime);

        withCustomer.Text = taggedTextRevealer.GetVisibleText();

#endif
        // TODO: Add your update logic here
        _slime.Update(gameTime);

        // Update timer
        Time.Update(gameTime);
        timer.UpdateTime();

        //check is time ups and show summary panel
        if (!timer.IsTimeUp()) { 
            
        }
        else
        {
            timer.Stop();
            _summaryPanelCataory.Enabled = true;
            _summaryPanelCataory.Visible = true;
            Disable_InGamePanel();
        }

            PanelInGameLogic();
        GameplayLogic();
        // Check if a cocktail is selected
        p_currentCocktailInfo.Text = _currentCocktail.Info();
        p_timer.Text = "Timer: " + timer.GetText();

        //update method
        UserInterface.Active.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        SpriteBatch.Begin(samplerState: SamplerState.PointWrap);


        _slime.Draw(SpriteBatch, _slimePosition);

        SpriteBatch.End();

        UserInterface.Active.Draw(SpriteBatch);
        base.Draw(gameTime);
    }

    protected void PanelInGameLogic()
    {
        if (!_isOpenAlcohol)
        {
            if (_inGame_Alcohol.Offset.X < width_ReciptPanel)
            {
                _inGame_Alcohol.Offset += new Vector2(50, 0);
            }
        }
        else
        {
            if (_inGame_Alcohol.Offset.X >= Padding_ReciptPanel)
            {
                _inGame_Alcohol.Offset -= new Vector2(50, 0);
            }
        }

        if (!_isOpenMixer)
        {
            if (_inGame_Mixer.Offset.X < width_ReciptPanel)
            {
                _inGame_Mixer.Offset += new Vector2(50, 0);
            }
        }
        else
        {
            if (_inGame_Mixer.Offset.X >= Padding_ReciptPanel)
            {
                _inGame_Mixer.Offset -= new Vector2(50, 0);
            }
        }

        if (!_isOpenMethod)
        {
            if (_inGame_MethodScreen.Offset.X < width_ReciptPanel)
                _inGame_MethodScreen.Offset += new Vector2(50, 0);
        }
        else
        {
            if (_inGame_MethodScreen.Offset.X >= 200 + 20)
                _inGame_MethodScreen.Offset -= new Vector2(50, 0);
        }
    }

    protected void GameplayLogic()
    {
        if (MixPartCount >= 10)
        {
            DisableBTNMakeCocktail();
        }
    }

    protected void RandomeTargetCocktail() {
        str_targetCocktail_Name = GetRandomCocktailName();
        CocktailDicMaker.CocktailDictionary.TryGetValue(str_targetCocktail_Name, out _targetCoctail);
        string CustomerNPCName = GetRandomNameNPC();
        _imgCustomerNPC.SourceRectangle = Atlas_CustomerNPC.GetRegion(CustomerNPCName).SourceRectangle;
        p_targetCockTailInfo.Text = "Target Cocktail: " + str_targetCocktail_Name + _targetCoctail.Info();
    }

    private string GetRandomCocktailName()
    {
        if (CocktailDicMaker.CocktailDictionary.Count == 0)
            return string.Empty;

        var random = new Random();
        int randomIndex = random.Next(CocktailDicMaker.CocktailDictionary.Count);
        return CocktailDicMaker.CocktailDictionary.Keys.ElementAt(randomIndex);
    }

    protected void DisableBTNMakeCocktail()
    {
        _BTN_Vodka.Enabled = false;
        _BTN_Gin.Enabled = false;
        _BTN_TripleSec.Enabled = false;
        _BTN_Vermoth.Enabled = false;

        _BTN_Lemon.Enabled = false;
        _BTN_Syrup.Enabled = false;
        _BTN_Soda.Enabled = false;
        _BTN_Grape.Enabled = false;
        _BTN_Soda.Enabled = false;
        _BTN_Cranberry.Enabled = false;
        _BTN_Perpermint.Enabled = false;
    }

    protected void EnableBTNALLInGamePanel()
    {
        //enable all buttons
        _BTN_Vodka.Enabled = true;
        _BTN_Gin.Enabled = true;
        _BTN_TripleSec.Enabled = true;
        _BTN_Vermoth.Enabled = true;
        _BTN_Lemon.Enabled = true;
        _BTN_Syrup.Enabled = true;
        _BTN_Soda.Enabled = true;
        _BTN_Grape.Enabled = true;
        _BTN_Soda.Enabled = true;
        _BTN_Cranberry.Enabled = true;
        _BTN_Perpermint.Enabled = true;

        _BTN_AddIce.Enabled = true;
        _BTN_Mix.Enabled = true;
        _BTN_Shake.Enabled = true;

        _BTN_Alcohol.Enabled = true;
        _BTN_Mixer.Enabled = true;
    }

    protected void DiableBTN_inGame()
    {
        _BTN_Alcohol.Enabled = false;
        _BTN_Mixer.Enabled = false;

        _isOpenAlcohol = false;
        _isOpenMixer = false;
    }

    protected void Enable_InGamePanel() { 
        _inGamePanelCatagoly.Enabled = true;
        _inGamePanelCatagoly.Visible = true;
        _inGamePanelLeft.Enabled = true;
        _inGamePanelLeft.Visible = true;
        _inGamePanelRight.Enabled = true;
        _inGamePanelRight.Visible = true;
    }

    protected void Disable_InGamePanel()
    {
        _inGamePanelCatagoly.Enabled = false;
        _inGamePanelCatagoly.Visible = false;
        _inGamePanelLeft.Enabled = false;
        _inGamePanelLeft.Visible = false;
        _inGamePanelRight.Enabled = false;
        _inGamePanelRight.Visible = false;
    }

    protected float CalcualatePrice(Cocktail _targetCocktail) {

        float _price = 0;
        _price = _targetCocktail.GetPrice();

        if (_targetCocktail.Equals(_currentCocktail)) { 
            _price = _price * 1.2f;        
        }
        else
        {
            if (!_targetCocktail.IsSameTypeOfCocktail(_currentCocktail)) {
                _price = 0;
                return _price;
            }
            if (!_targetCocktail.IsSameMethod(_currentCocktail))
            {
                _price = _price * 0.8f;
            }
            if (!_targetCocktail.IsAddIceBoth(_currentCocktail)) { 
                _price = _price * 0.8f;
            }
        }

        return _price;
    }

    protected string GetRandomNameNPC()
    {
        var random = new Random();
        int randomIndex = random.Next(1, Atlas_CustomerNPC.GetRegionCount());
        Debug.WriteLine($"Random NPC{randomIndex}");
        return "NPC" + randomIndex.ToString();
    }
}
