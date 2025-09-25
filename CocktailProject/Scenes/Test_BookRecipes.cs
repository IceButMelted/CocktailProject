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
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;



namespace CocktailProject.Scenes
{
    class Test_BookRecipes : Scene
    {
        private const String COCKTAIL_TEXT = "Cocktail : 410";
        public Panel P_BGBookRecipes;
        public Image Img_BookRecipes;     public Texture2D T_BookRecipes;

        public TextureAtlas Recipes_Atlas;
        public Image Img_LeftPage;
        public Image Img_RightPage;

        public Button BTN_PreviousPage;
        public Button BTN_NextPage;

        public int CurrentPage = 1;
        public int TotalPages = 0;

        public override void Initialize()
        {


            base.Initialize();
        }

        public override void LoadContent()
        {

            T_BookRecipes = Content.Load<Texture2D>("images/UI/RecipeBook/Book_Base");
            Recipes_Atlas = TextureAtlas.FromFile(Content, "images/UI/RecipeBook/Recipes_Define.xml");
            TotalPages = Recipes_Atlas.GetRegionCount() / 2;

            UserInterface.Initialize(Content, BuiltinThemes.hd);
            P_BGBookRecipes = new Panel(new Vector2(1920, 1080), PanelSkin.Default, Anchor.Center);
            P_BGBookRecipes.FillColor = Color.Green * 0.5f;

            Img_BookRecipes = new Image(T_BookRecipes, new Vector2(1033, 755), ImageDrawMode.Stretch, Anchor.TopLeft);
            Img_BookRecipes.Offset = new Vector2(66, 63);

            Img_LeftPage = new Image(Recipes_Atlas.Texture, new Vector2(480, 700), ImageDrawMode.Stretch, Anchor.TopLeft);
            Img_LeftPage.SourceRectangle = Recipes_Atlas.GetRegion("Recipe_01_L").SourceRectangle;
            Img_LeftPage.Offset = new Vector2(11, 24);

            Img_RightPage = new Image(Recipes_Atlas.Texture, new Vector2(480, 700), ImageDrawMode.Stretch, Anchor.TopLeft);
            Img_RightPage.SourceRectangle = Recipes_Atlas.GetRegion("Recipe_01_R").SourceRectangle;
            Img_RightPage.Offset = new Vector2(491, 24);

            BTN_PreviousPage = new Button("<", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(50, 50));
            BTN_PreviousPage.OnClick += (Entity e) => { 
                ChangePage(Enum_Page.PreviousPage); 
                UpdatePageView(); 
            };

            BTN_NextPage = new Button(">", ButtonSkin.Default, Anchor.BottomRight, new Vector2(50, 50));
            BTN_NextPage.OnClick += (Entity e) => { 
                ChangePage(Enum_Page.NextPage); 
                UpdatePageView(); 
            };


            Img_BookRecipes.AddChild(Img_LeftPage);
            Img_BookRecipes.AddChild(Img_RightPage);
            Img_BookRecipes.AddChild(BTN_PreviousPage);
            Img_BookRecipes.AddChild(BTN_NextPage);
            P_BGBookRecipes.AddChild(Img_BookRecipes);

            UserInterface.Active.AddEntity(P_BGBookRecipes);




            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E))
                Core.ChangeScene(new GamePlay());

            if(Core.Input.Keyboard.WasKeyJustPressed(Keys.Up))
                Img_RightPage.Offset -= new Vector2(0,1);
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Down))
                Img_RightPage.Offset += new Vector2(0, 1);
            if(Core.Input.Keyboard.WasKeyJustPressed(Keys.Right))
                Img_RightPage.Offset += new Vector2(1, 0);
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Left))
                Img_RightPage.Offset -= new Vector2(1, 0);

            
            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);
        }

        public enum Enum_Page
        {
            LeftPage,
            RightPage,
            PreviousPage,
            NextPage
        }

        public void ChangePage(Enum_Page _Page) {
            if (_Page == Enum_Page.NextPage && CurrentPage < TotalPages) { 
                CurrentPage++;
                UpdatePageView();
            }
            if (_Page == Enum_Page.PreviousPage && CurrentPage > 1)
            {
                CurrentPage--;
                UpdatePageView();
            }
        }
        private void UpdatePageView()
        {
            string leftKey = $"Recipe_{CurrentPage:D2}_L";
            string rightKey = $"Recipe_{CurrentPage:D2}_R";
            Img_LeftPage.SourceRectangle = Recipes_Atlas.GetRegion(leftKey).SourceRectangle;
            Img_RightPage.SourceRectangle = Recipes_Atlas.GetRegion(rightKey).SourceRectangle;
        }

        public override void Draw(GameTime gameTime)
        {
            Core.GraphicsDevice.Clear(Color.CornflowerBlue);

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }

        public void EnableBookRecipes(bool _Enable)
        {
            P_BGBookRecipes.Visible = _Enable;
        }

        
    }
}
