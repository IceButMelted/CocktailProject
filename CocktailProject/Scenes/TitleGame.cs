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



namespace CocktailProject.Scenes
{
    class TitleGame : Scene
    {
        private const String COCKTAIL_TEXT = "Cocktail : 410";

        public Panel BTN_Start;

        public override void Initialize()
        {
            

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(Core.Input.Keyboard.WasKeyJustPressed(Keys.E))
                Core.ChangeScene(new GamePlay());

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            BTN_Start = new Panel(new Vector2(300, 100), PanelSkin.Default, Anchor.Center);


            UserInterface.Active.AddEntity(BTN_Start);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }
    }
}
