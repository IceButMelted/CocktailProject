using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CocktailProject.ClassCocktail;

using MonoGameLibrary;
using MonoGameLibrary.Graphics;

// using GeonBit UI elements
using GeonBit.UI;
using GeonBit.UI.Entities;
using System;
using System.Diagnostics;
using System.Linq;

namespace CocktailProject;

public class Game1 : Core
{
    Panel _titlePanel;
    Button _startBTN;
    Button _exitBTN;

    Panel _inGamePanel;
    Panel _inGame_Alcohol;
    Panel _inGame_Mixer;
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
    #endregion  

    Paragraph currentCocktailInfo;


    int width_ReciptPanel = 500;
    int heigh_ReciptPanel = 250;

    Panel _summaryPanel;

    bool _isOpenAlcohol = false;
    bool _isOpenMixer = false;

    #region Cocktail
    private static string _targetCocktailName;
    private CocktailBuilder CurrentCocktail = new CocktailBuilder();
    private int MixPartCount = 0;

    #endregion


    private AnimatedSprite _slime;
    private Vector2 _slimePosition = new Vector2(100, 100);

    public Game1() : base("CocktialProject", 1280, 720, false)
    {
        
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        _slime = atlas.CreateAnimatedSprite("slime-animation");
        _slime.Scale = new Vector2(4.0f, 4.0f);



#region Init Ui
        UserInterface.Initialize(Content, BuiltinThemes.hd);
        _titlePanel = new Panel(new Vector2(500, 500), PanelSkin.Default, Anchor.Center);
        _startBTN = new Button("Start Game", ButtonSkin.Default,Anchor.AutoCenter, new Vector2(200,50));
        _startBTN.Padding = new Vector2(0, 25);
        _startBTN.ToolTipText = "Click to start the game!";
        _startBTN.OnClick = (Entity entity) =>
        {
            Debug.WriteLine($"Target Cocktail: {_targetCocktailName}");
            _titlePanel.Enabled = false;
            _titlePanel.Visible = false;
            _inGamePanel.Enabled = true;
            _inGamePanel.Visible = true;
            UserInterface.Active.SetCursor(CursorType.Default);
        };

        _exitBTN  = new Button("Exit Game", ButtonSkin.Default, Anchor.AutoCenter, new Vector2(200,50));
        _exitBTN.Padding = new Vector2(0, 25);
        _exitBTN.OnClick = (Entity entity) =>
        {
            Exit();
        };

        /// Create the in-game panel
        _inGamePanel = new Panel(new Vector2(500, 500), PanelSkin.Default, Anchor.TopRight);
        _inGamePanel.Padding = new Vector2(0, 0);
        _inGamePanel.Enabled = false;
        _inGamePanel.Visible = false;

        _BTN_AddIce = new Button("AddIce", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(100, 100), new Vector2(0, -100));
        _BTN_AddIce.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddIce(true);
            _BTN_AddIce.Enabled = false;
        };
        
        _BTN_Shake = new Button("Shake", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(100, 100), new Vector2(100, -100));
        _BTN_Shake.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.UseMethod(Enum_Method.Shaking);
            _BTN_Shake.Enabled = false;
            _BTN_Mix.Enabled = false;
        };

        _BTN_Mix = new Button("Mix", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(100, 100), new Vector2(200, -100));
        _BTN_Mix.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.UseMethod(Enum_Method.Mixing);
            _BTN_Mix.Enabled = false;
            _BTN_Shake.Enabled = false;
        };
        _BTN_Serve = new Button("Serve", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(100, 100), new Vector2(300, -100));
        _BTN_Serve.OnMouseDown = (Entity e) =>
        {
            // Logic for Serve button click
            Debug.WriteLine("Serve button clicked!");
        };

        _BTN_Reset = new Button("Reset", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(100, 100), new Vector2(400, -100));
        _BTN_Reset.OnMouseDown = (Entity e) =>
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

            CurrentCocktail.ClearAllIngredients();
            MixPartCount = 0;

            Debug.WriteLine("Reset button clicked!");
        };

        /// Create the in-game alcohol and mixer panels
        /// Alchol
        _inGame_Alcohol = new Panel(new Vector2(width_ReciptPanel, heigh_ReciptPanel), PanelSkin.Default, Anchor.TopLeft,new Vector2(width_ReciptPanel,0));
        _inGame_Alcohol.Padding = new Vector2(20, 10);
        _BTN_Alcohol = new Button("Alchol", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(50, 100), new Vector2(-70,0));
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
            CurrentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Gin, 1);
            MixPartCount++;
            // Logic for Gin button click
            Debug.WriteLine("Gin button clicked!");
        };

        _BTN_Vodka = new Button("Vodka", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Vodka.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vodka, 1);
            MixPartCount++;
            Debug.WriteLine("Vodka button clicked!");
        };

        HorizontalLine hz = new HorizontalLine(Anchor.Center);

        _BTN_TripleSec = new Button("Triple Sec", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_TripleSec.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Triplesec, 1);
            MixPartCount++;
            Debug.WriteLine("Triple Sec button clicked!");
        };

        _BTN_Vermoth = new Button("Vermouth", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Vermoth.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddOrUpdateAlcohol(Enum_Alcohol.Vermouth, 1);
            MixPartCount++;
            Debug.WriteLine("Vermouth button clicked!");
        };

        _inGame_Alcohol.AddChild(_BTN_Gin);
        _inGame_Alcohol.AddChild(_BTN_Vodka);
        _inGame_Alcohol.AddChild(hz);  
        _inGame_Alcohol.AddChild(_BTN_TripleSec);
        _inGame_Alcohol.AddChild(_BTN_Vermoth);
        _inGame_Alcohol.AddChild(_BTN_Alcohol);
        

        /// Mixer
        _inGame_Mixer = new Panel(new Vector2(width_ReciptPanel, heigh_ReciptPanel), PanelSkin.Default, Anchor.TopLeft, new Vector2(width_ReciptPanel,heigh_ReciptPanel));
        _inGame_Mixer.Padding = new Vector2(20, 10);
        _BTN_Mixer = new Button("Mixer", ButtonSkin.Default, Anchor.TopLeft, new Vector2(50, 100), new Vector2(-70, 0));
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
            CurrentCocktail.AddOrUpdateMixer(Enum_Mixer.LemonJuice, 1);
            MixPartCount++;
            Debug.WriteLine("Lemon button clicked!");
        };

        _BTN_Syrup = new Button("Syrup", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Syrup.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddOrUpdateMixer(Enum_Mixer.Syrup, 1);
            MixPartCount++;
            Debug.WriteLine("Syrup button clicked!");
        };

        _BTN_Soda = new Button("Soda", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Soda.OnMouseDown = (Entity e) => 
        {
            CurrentCocktail.AddOrUpdateMixer(Enum_Mixer.Soda, 1);
            MixPartCount++;
            Debug.WriteLine("Soda button clicked!");
        };

        HorizontalLine hz1 = new HorizontalLine(Anchor.Center);

        _BTN_Grape = new Button("Grapefruit juice", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Grape.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddOrUpdateMixer(Enum_Mixer.GrapefruitJuice, 1);
            MixPartCount++;
            Debug.WriteLine("Grapefruit juice button clicked!");
        };

        _BTN_Cranberry = new Button("Cranberry juice", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Cranberry.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddOrUpdateMixer(Enum_Mixer.CanberryJuice, 1);
            MixPartCount++;
            Debug.WriteLine("Cranberry juice button clicked!");
        };

        _BTN_Perpermint = new Button("Paper mint", ButtonSkin.Default, Anchor.AutoInline, new Vector2(150, 100), new Vector2(0, 0));
        _BTN_Perpermint.OnMouseDown = (Entity e) =>
        {
            CurrentCocktail.AddOrUpdateMixer(Enum_Mixer.Soda, 1);
            MixPartCount++;
            Debug.WriteLine("Paper mint button clicked!");
        };


        _inGame_Mixer.AddChild(_BTN_Lemon);
        _inGame_Mixer.AddChild(_BTN_Syrup);
        _inGame_Mixer.AddChild(_BTN_Soda);
        _inGame_Mixer.AddChild(hz1);
        _inGame_Mixer.AddChild(_BTN_Grape);
        _inGame_Mixer.AddChild(_BTN_Cranberry);
        _inGame_Mixer.AddChild(_BTN_Perpermint);
        _inGame_Mixer.AddChild(_BTN_Mixer);

        currentCocktailInfo = new Paragraph(CurrentCocktail.Info(), Anchor.TopLeft, new Vector2(500, 500));
        currentCocktailInfo.FillColor = Color.White;

        /// Add Chiildren to the panels
        _titlePanel.AddChild(_startBTN);
        _titlePanel.AddChild(_exitBTN);

        _inGamePanel.AddChild(_BTN_AddIce);
        _inGamePanel.AddChild(_BTN_Shake);
        _inGamePanel.AddChild(_BTN_Mix);
        _inGamePanel.AddChild(_BTN_Serve);
        _inGamePanel.AddChild(_BTN_Reset);

        _inGamePanel.AddChild(_inGame_Alcohol);
        _inGamePanel.AddChild(_inGame_Mixer);

        UserInterface.Active.SetCursor(CursorType.Default);
        
        UserInterface.Active.AddEntity(currentCocktailInfo);
        UserInterface.Active.AddEntity(_titlePanel);
        UserInterface.Active.AddEntity(_inGamePanel);
#endregion

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _slime.Update(gameTime);

        PanelInGameLogic();
        GameplayLogic();
        // Check if a cocktail is selected
        currentCocktailInfo.Text = CurrentCocktail.Info();

        //update method
        UserInterface.Active.Update(gameTime);
        base.Update(gameTime);
    }

    protected void PanelInGameLogic() {
        if (!_isOpenAlcohol)
        {
            if (_inGame_Alcohol.Offset.X < width_ReciptPanel)
            {
                _inGame_Alcohol.Offset += new Vector2(25, 0);
                _BTN_Alcohol.Enabled = false;
                _BTN_Mixer.Enabled = false;
            }
        }
        else
        {
            if (_inGame_Alcohol.Offset.X >= 0)
            {
                _inGame_Alcohol.Offset -= new Vector2(25, 0);
                _BTN_Alcohol.Enabled = false;
                _BTN_Mixer.Enabled = false;
            }
        }

        if (!_isOpenMixer)
        {
            if (_inGame_Mixer.Offset.X < width_ReciptPanel)
            {
                _inGame_Mixer.Offset += new Vector2(25, 0);
                _BTN_Mixer.Enabled = false;
                _BTN_Alcohol.Enabled = false;
            }
        }
        else
        {
            if (_inGame_Mixer.Offset.X >= 0)
            {
                _inGame_Mixer.Offset -= new Vector2(25, 0);
                _BTN_Mixer.Enabled = false;
                _BTN_Alcohol.Enabled = false;
            }
        }
        _BTN_Alcohol.Enabled = true;
        _BTN_Mixer.Enabled = true;
    }

    protected void GameplayLogic() {
        if (MixPartCount >= 10) { 
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




    private string GetRandomCocktailName()
    {
        if (CocktailDicMaker.CocktailDictionary.Count == 0)
            return string.Empty;

        var random = new Random();
        int randomIndex = random.Next(CocktailDicMaker.CocktailDictionary.Count);
        return CocktailDicMaker.CocktailDictionary.Keys.ElementAt(randomIndex);
    }
}
