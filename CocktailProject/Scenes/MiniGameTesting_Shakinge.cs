using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CocktailProject.MiniGame;

using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;

using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;



namespace CocktailProject.Scenes
{
    class MiniGameTesting_Shakinge : Scene
    {
        private const String COCKTAIL_TEXT = "Cocktail : 410";

        public Panel P_Thanks;
        public Panel BG_ProgressBar;
        public Panel ProgressBar;

        public Panel BG_TargetZone;
        public Panel TargetZone;
        public Panel Pointing;


        #region for setting minigame
        public Panel P_Setting;
        public TextInput TxtInput_TargetZone_MaxSize;
        public TextInput TxtInput_TargetZone_MinSize;
        public TextInput TxtInput_InitTargetZone_MinValue;
        public TextInput TxtInput_TargetZone_DecreasePerProgress;
        public TextInput TxtInput_CurrentValue_IncreaseRate;
        public TextInput TxtInput_CurrentValue_DecreaseRate;
        public TextInput TxtInput_ProgressBar_IncreaseRate;
        #endregion

        public int SizeBar = 500;

        public override void Initialize()
        {
            ShakingMinigame.StartGame();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            ShakingMinigame.Update(gameTime);

            UpdateMiniGame();
            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);
        }

        public void ForSettingMinigame() {
            P_Setting = new Panel(new Vector2(400, 600), PanelSkin.Default, Anchor.TopRight);

            Header Header_TargetZone = new Header("TragetZone");
            Header_TargetZone.Anchor = Anchor.AutoInline;
            P_Setting.AddChild(Header_TargetZone);

            Paragraph PR_TargetZone_Max = new Paragraph("TargetZone MaxSize");
            PR_TargetZone_Max.Anchor = Anchor.AutoInline;

            TxtInput_TargetZone_MaxSize =  new TextInput(false);
            TxtInput_TargetZone_MaxSize.Size = new Vector2(100, 30);
            TxtInput_TargetZone_MaxSize.Anchor = Anchor.AutoInline;
            TxtInput_TargetZone_MaxSize.PlaceholderText = "10-100";
            TxtInput_TargetZone_MaxSize.Validators.Add(new GeonBit.UI.Entities.TextValidators.NumbersOnly());
            TxtInput_TargetZone_MaxSize.Value = ShakingMinigame.TargetZone_MaxSize.ToString();
            TxtInput_TargetZone_MaxSize.OnValueChange = (Entity entity) => 
            {
                float parsedValue;
                if (float.TryParse(TxtInput_TargetZone_MaxSize.Value, out parsedValue))
                {
                    ShakingMinigame.TargetZone_MaxSize = parsedValue;
                    ShakingMinigame.Reset();
                }
                else
                {
                    Debug.WriteLine("Invalid input: " + TxtInput_TargetZone_MaxSize.Value);
                }

                // Optional: always print what the user entered
                Debug.WriteLine(TxtInput_TargetZone_MaxSize.Value);
            };

            P_Setting.AddChild(PR_TargetZone_Max);
            P_Setting.AddChild(TxtInput_TargetZone_MaxSize);

            Paragraph PR_TargetZone_Min = new Paragraph("TargetZone MinSize");
            PR_TargetZone_Min.Anchor = Anchor.AutoInline;

            TxtInput_TargetZone_MinSize = new TextInput(false);
            TxtInput_TargetZone_MinSize.Size = new Vector2(100, 30);
            TxtInput_TargetZone_MinSize.Anchor = Anchor.AutoInline;
            TxtInput_TargetZone_MinSize.PlaceholderText = "10-100";
            TxtInput_TargetZone_MinSize.Validators.Add(new GeonBit.UI.Entities.TextValidators.NumbersOnly());
            TxtInput_TargetZone_MinSize.Value = ShakingMinigame.TargetZone_MinSize.ToString();
            TxtInput_TargetZone_MinSize.OnValueChange = (Entity entity) =>
            {
                float parsedValue;
                if (float.TryParse(TxtInput_TargetZone_MinSize.Value, out parsedValue))
                {
                    ShakingMinigame.TargetZone_MinSize = parsedValue;
                    ShakingMinigame.Reset();
                }
                else
                {
                    Debug.WriteLine("Invalid input: " + TxtInput_TargetZone_MinSize.Value);
                }

                // Optional: always print what the user entered
                Debug.WriteLine(TxtInput_TargetZone_MinSize.Value);
            };

            P_Setting.AddChild(PR_TargetZone_Min);
            P_Setting.AddChild(TxtInput_TargetZone_MinSize);

            Paragraph PR_InitTargetZone_MinValue = new Paragraph("InitTargetZone MinValue");
            PR_InitTargetZone_MinValue.Anchor = Anchor.AutoInline;
            TxtInput_InitTargetZone_MinValue = new TextInput(false);
            TxtInput_InitTargetZone_MinValue.Size = new Vector2(100, 30);
            TxtInput_InitTargetZone_MinValue.Anchor = Anchor.AutoInline;
            TxtInput_InitTargetZone_MinValue.PlaceholderText = "10-100";
            TxtInput_InitTargetZone_MinValue.Validators.Add(new GeonBit.UI.Entities.TextValidators.NumbersOnly());
            TxtInput_InitTargetZone_MinValue.Value = ShakingMinigame.InitTargetZone_MinValue.ToString();
            TxtInput_InitTargetZone_MinValue.OnValueChange = (Entity entity) =>
            {
                float parsedValue;
                if (float.TryParse(TxtInput_InitTargetZone_MinValue.Value, out parsedValue))
                {
                    ShakingMinigame.InitTargetZone_MinValue = parsedValue;
                    ShakingMinigame.Reset();
                }
                else
                {
                    Debug.WriteLine("Invalid input: " + TxtInput_InitTargetZone_MinValue.Value);
                }

                // Optional: always print what the user entered
                Debug.WriteLine(TxtInput_InitTargetZone_MinValue.Value);
            };

            P_Setting.AddChild(PR_InitTargetZone_MinValue);
            P_Setting.AddChild(TxtInput_InitTargetZone_MinValue);

            Paragraph PR_TargetZone_DecreasePerProgress = new Paragraph("TargetZone DecreasePerProgress");
            PR_TargetZone_DecreasePerProgress.Anchor = Anchor.AutoInline;

            TxtInput_TargetZone_DecreasePerProgress = new TextInput(false);
            TxtInput_TargetZone_DecreasePerProgress.Size = new Vector2(100, 30);
            TxtInput_TargetZone_DecreasePerProgress.Anchor = Anchor.AutoInline;
            TxtInput_TargetZone_DecreasePerProgress.PlaceholderText = "0.01-1";
            //TxtInput_TargetZone_DecreasePerProgress.Validators.Add(new GeonBit.UI.Entities.TextValidators.());
            TxtInput_TargetZone_DecreasePerProgress.Value = ShakingMinigame.TargetZone_DecreasePerProgress.ToString();
            TxtInput_TargetZone_DecreasePerProgress.OnValueChange = (Entity entity) =>
            {
                float parsedValue;
                if (float.TryParse(TxtInput_TargetZone_DecreasePerProgress.Value, out parsedValue))
                {
                    ShakingMinigame.TargetZone_DecreasePerProgress = parsedValue;
                    ShakingMinigame.Reset();
                }
                else
                {
                    Debug.WriteLine("Invalid input: " + TxtInput_TargetZone_DecreasePerProgress.Value);
                }
                // Optional: always print what the user entered
                Debug.WriteLine(TxtInput_TargetZone_DecreasePerProgress.Value);
            };

            P_Setting.AddChild(PR_TargetZone_DecreasePerProgress);
            P_Setting.AddChild(TxtInput_TargetZone_DecreasePerProgress);


            ///-----------------------------------------------------------------------------------------------///
            Header Header_CurrentValue = new Header("Pointing Value");
            Header_CurrentValue.Anchor = Anchor.AutoInline;
            P_Setting.AddChild(Header_CurrentValue);

            Paragraph PR_CurrentValue_IncreaseRate = new Paragraph("Pointing Increase Rate");
            PR_CurrentValue_IncreaseRate.Anchor = Anchor.AutoInline;

            TxtInput_CurrentValue_IncreaseRate = new TextInput(false);
            TxtInput_CurrentValue_IncreaseRate.Size = new Vector2(100, 30);
            TxtInput_CurrentValue_IncreaseRate.Anchor = Anchor.AutoInline;
            TxtInput_CurrentValue_IncreaseRate.PlaceholderText = "1-100";
            TxtInput_CurrentValue_IncreaseRate.Validators.Add(new GeonBit.UI.Entities.TextValidators.NumbersOnly());
            TxtInput_CurrentValue_IncreaseRate.Value = ShakingMinigame.CurrentValue_IncreaseRate.ToString();
            TxtInput_CurrentValue_IncreaseRate.OnValueChange = (Entity entity) =>
            {
                float parsedValue;
                if (float.TryParse(TxtInput_CurrentValue_IncreaseRate.Value, out parsedValue))
                {
                    ShakingMinigame.CurrentValue_IncreaseRate = parsedValue;
                    ShakingMinigame.Reset();
                }
                else
                {
                    Debug.WriteLine("Invalid input: " + TxtInput_CurrentValue_IncreaseRate.Value);
                }

                // Optional: always print what the user entered
                Debug.WriteLine(TxtInput_CurrentValue_IncreaseRate.Value);
            };

            P_Setting.AddChild(PR_CurrentValue_IncreaseRate);
            P_Setting.AddChild(TxtInput_CurrentValue_IncreaseRate);

            Paragraph PR_CurrentValue_DecreaseRate = new Paragraph("Pointing Decrease Rate");
            PR_CurrentValue_DecreaseRate.Anchor = Anchor.AutoInline;
            TxtInput_CurrentValue_DecreaseRate = new TextInput(false);
            TxtInput_CurrentValue_DecreaseRate.Size = new Vector2(100, 30);
            TxtInput_CurrentValue_DecreaseRate.Anchor = Anchor.AutoInline;
            TxtInput_CurrentValue_DecreaseRate.PlaceholderText = "1-100";
            TxtInput_CurrentValue_DecreaseRate.Validators.Add(new GeonBit.UI.Entities.TextValidators.NumbersOnly());
            TxtInput_CurrentValue_DecreaseRate.Value = ShakingMinigame.CurrentValue_DecreaseRate.ToString();
            TxtInput_CurrentValue_DecreaseRate.OnValueChange = (Entity entity) =>
            {
                float parsedValue;
                if (float.TryParse(TxtInput_CurrentValue_DecreaseRate.Value, out parsedValue))
                {
                    ShakingMinigame.CurrentValue_DecreaseRate = parsedValue;
                    ShakingMinigame.Reset();
                }
                else
                {
                    Debug.WriteLine("Invalid input: " + TxtInput_CurrentValue_DecreaseRate.Value);
                }

                // Optional: always print what the user entered
                Debug.WriteLine(TxtInput_CurrentValue_IncreaseRate.Value);
            };

            P_Setting.AddChild(PR_CurrentValue_DecreaseRate);
            P_Setting.AddChild(TxtInput_CurrentValue_DecreaseRate);

            Header Header_ProgressBar = new Header("Progress Bar");
            Header_ProgressBar.Anchor = Anchor.AutoInline;
            P_Setting.AddChild(Header_ProgressBar);

            Paragraph PR_ProgressBar_IncreaseRate = new Paragraph("ProgressBar Increase Rate");
            PR_ProgressBar_IncreaseRate.Anchor = Anchor.AutoInline;
            TxtInput_ProgressBar_IncreaseRate = new TextInput(false);
            TxtInput_ProgressBar_IncreaseRate.Size = new Vector2(100, 30);
            TxtInput_ProgressBar_IncreaseRate.Anchor = Anchor.AutoInline;
            TxtInput_ProgressBar_IncreaseRate.PlaceholderText = "1-100";
            TxtInput_ProgressBar_IncreaseRate.Validators.Add(new GeonBit.UI.Entities.TextValidators.NumbersOnly());
            TxtInput_ProgressBar_IncreaseRate.Value = ShakingMinigame.ProgressBar_IncreaseRate.ToString();
            TxtInput_ProgressBar_IncreaseRate.OnValueChange = (Entity entity) =>
            {
                float parsedValue;
                if (float.TryParse(TxtInput_ProgressBar_IncreaseRate.Value, out parsedValue))
                {
                    ShakingMinigame.ProgressBar_IncreaseRate = parsedValue;
                    ShakingMinigame.Reset();
                }
                else
                {
                    Debug.WriteLine("Invalid input: " + TxtInput_ProgressBar_IncreaseRate.Value);
                }
                // Optional: always print what the user entered
                Debug.WriteLine("-------" + ShakingMinigame.ProgressBar_IncreaseRate);
                Debug.WriteLine(TxtInput_ProgressBar_IncreaseRate.Value);
                
            };

            P_Setting.AddChild(PR_ProgressBar_IncreaseRate);
            P_Setting.AddChild(TxtInput_ProgressBar_IncreaseRate);

            UserInterface.Active.AddEntity(P_Setting);
        }

        public override void LoadContent()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            P_Thanks = new Panel(new Vector2(400, 600), PanelSkin.Default, Anchor.Center);
            P_Thanks.Padding = Vector2.Zero;

            BG_TargetZone = new Panel(new Vector2(50, SizeBar), PanelSkin.Simple, Anchor.CenterLeft);
            BG_TargetZone.FillColor = Color.DarkGray;
            BG_TargetZone.Offset = new Vector2(0, 0);
            BG_TargetZone.Padding = Vector2.Zero;

            BG_ProgressBar = new Panel(new Vector2(50, SizeBar), PanelSkin.Simple, Anchor.CenterLeft);
            BG_ProgressBar.FillColor = Color.Gray;
            BG_ProgressBar.Offset = new Vector2(50, 0);
            BG_ProgressBar.Padding = Vector2.Zero;

            ProgressBar = new Panel(new Vector2(40, 10), PanelSkin.Simple, Anchor.BottomCenter);
            ProgressBar.FillColor = Color.Green;

            TargetZone = new Panel(new Vector2(40, 50), PanelSkin.Simple, Anchor.BottomCenter);
            TargetZone.FillColor = Color.Red;
            TargetZone.Opacity = 100;

            Pointing = new Panel(new Vector2(40, 10), PanelSkin.Simple, Anchor.BottomCenter);

            BG_TargetZone.AddChild(Pointing);
            BG_TargetZone.AddChild(TargetZone);
            P_Thanks.AddChild(BG_TargetZone);

            BG_ProgressBar.AddChild(ProgressBar);
            BG_TargetZone.AddChild(BG_ProgressBar);
            UserInterface.Active.AddEntity(P_Thanks);


            ForSettingMinigame();

            base.LoadContent();
        }

        public void UpdateMiniGame()
        {
            if(ShakingMinigame.CurrentValue != 0)
                Pointing.Size = new Vector2(40, (int)(SizeBar * (ShakingMinigame.CurrentValue / 100)) );
            else
                Pointing.Size = new Vector2(40, 1);

            TargetZone.Size = new Vector2(40, (int)(SizeBar * (ShakingMinigame.TargetZone_CurrentSize / 100)));

            //cal offset targetZone
            int offsetX = (int)((ShakingMinigame.InitTargetZone) - (ShakingMinigame.TargetZone_CurrentSize/2));

            TargetZone.Offset = new Vector2(0, (int)(SizeBar * (offsetX)/100 ));

            ProgressBar.Size = new Vector2(40, (int)(SizeBar * (ShakingMinigame.ProgressBar_CurrentValue / 100)));

            if(ShakingMinigame.IsComplete())
                ShakingMinigame.Reset();

            //Debug.WriteLine(ShakingMinigame.CurrentValue);
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }
    }
}
