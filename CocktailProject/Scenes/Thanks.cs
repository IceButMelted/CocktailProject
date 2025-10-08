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
    class Thanks : Scene
    {
        private const String COCKTAIL_TEXT = "Cocktail : 410";

        public Panel P_Thanks;
        public Paragraph thank;

        public override void Initialize()
        {
            

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E))
            {
                Core.ChangeScene(new GamePlay());
                UserInterface.Active.Clear();
            }

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            P_Thanks = new Panel(new Vector2(600, 400), PanelSkin.Default, Anchor.Center);
            thank = new Paragraph("Thank you for playing my game !\nPress E to restart a new game.", Anchor.Center, new Vector2(0, 0));

            P_Thanks.AddChild(thank);   

            UserInterface.Active.AddEntity(P_Thanks);

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
