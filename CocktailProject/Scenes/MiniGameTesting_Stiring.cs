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
    class MiniGameTesting_Stiring : Scene
    {
        private const String COCKTAIL_TEXT = "Cocktail : 410";

        public static Panel P_Minigame_Stiring;
        public static CustomProgressBar PB_Stiring;
        public static Panel BG_Stiring_TargetZone;
        public static Panel Stiring_TargetZone;
        public static Panel Arrow_Stiring;

        public static int XSizeBar = 500;

        public override void Initialize()
        {
            StiringMinigame.InitTargetZone();
            StiringMinigame.StartGame();
            base.Initialize();
        }

        public override void LoadContent()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            P_Minigame_Stiring = new Panel(new Vector2(XSizeBar, 600), PanelSkin.Simple, Anchor.Center);
            PB_Stiring = new CustomProgressBar(0, (int)StiringMinigame.ProgressBar_SuccessCountToWin, new Vector2(XSizeBar - 100, 50), null, null, Anchor.Center);
            PB_Stiring.Value = (int)StiringMinigame.PointingArrow_CurrentValue;
            PB_Stiring.Locked = true;
            PB_Stiring.SliderSkin = SliderSkin.Default;
            PB_Stiring.ProgressFill.FillColor = Color.Yellow;
            

            BG_Stiring_TargetZone = new Panel(new Vector2(XSizeBar - 100, 50), PanelSkin.Simple, Anchor.BottomCenter);
            BG_Stiring_TargetZone.Offset = new Vector2(0,50);
            BG_Stiring_TargetZone.FillColor = Color.Gray;
            BG_Stiring_TargetZone.Padding = new Vector2(0, 0);

            Stiring_TargetZone = new Panel(new Vector2((StiringMinigame.TargetZone_CurrentSize / (StiringMinigame.MaxSize - StiringMinigame.MinSize)) * (XSizeBar - 100), 50), PanelSkin.Simple, Anchor.CenterLeft);
            Stiring_TargetZone.FillColor = Color.Red;
            Stiring_TargetZone.Padding = Vector2.Zero;
            Stiring_TargetZone.Opacity = 128;

            Arrow_Stiring = new Panel(new Vector2(2, 50), PanelSkin.Simple, Anchor.CenterLeft);
            Arrow_Stiring.Offset = new Vector2((StiringMinigame.PointingArrow_CurrentValue)-5 , 0);
            Arrow_Stiring.FillColor = Color.Blue;

            BG_Stiring_TargetZone.AddChild(Stiring_TargetZone);
            BG_Stiring_TargetZone.AddChild(Arrow_Stiring);


            P_Minigame_Stiring.AddChild(PB_Stiring);
            P_Minigame_Stiring.AddChild(BG_Stiring_TargetZone);

            UserInterface.Active.AddEntity(P_Minigame_Stiring);

            base.LoadContent();
        }

        public static void UpdateMiniGameShaking()
        {
            Stiring_TargetZone.Opacity = 128;

            float normalizedMin = (StiringMinigame.TargetZone_Init) - (StiringMinigame.TargetZone_CurrentSize/2);

            float normalizedWidth = (StiringMinigame.TargetZone_CurrentSize)
                                    / (StiringMinigame.MaxSize - StiringMinigame.MinSize);

            Stiring_TargetZone.Offset = new Vector2((XSizeBar - 100) * (normalizedMin/100), 0);
            Stiring_TargetZone.Size = new Vector2(normalizedWidth * (XSizeBar - 100), 50);

            float normalizedArrow = (StiringMinigame.PointingArrow_CurrentValue - StiringMinigame.MinSize)
                                    / (StiringMinigame.MaxSize - StiringMinigame.MinSize);

            Arrow_Stiring.Offset = new Vector2(normalizedArrow * (XSizeBar - 100) - (Arrow_Stiring.Size.X / 2), 0);

            PB_Stiring.Value = StiringMinigame.ProgressBar_Success;
        }


        public override void Update(GameTime gameTime)
        {

            UserInterface.Active.Update(gameTime);
            StiringMinigame.Update(gameTime);
            UpdateMiniGameShaking();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }
    }
}
