using GeonBit.UI.Entities;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailProject.ClassCocktail;
using MonoGameLibrary.Graphics;
using System.Diagnostics;

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
        #endregion


        #region UI Logic
        protected bool openAlcoholPanel = false;
        #endregion

        // Panel
        public Panel P_Ingredient;
            public Button BTN_Mixer;
            public Panel P_Mixer;
                public Button Mixer_CanberryJuice;
                public Button Mixer_GrapefruitJuice;
                public Button Mixer_LemonJuice;
                public Button Mixer_Soda;
                public Button Mixer_Syrup;
                public Button Mixer_PepperMint;
            public Button BTN_Alcohol;
            public Panel P_Alcohol;
                public Button Alcohol_Vodka;
                public Button Alcohol_Gin;
                public Button Alcohol_Triplesec;
                public Button Alcohol_Vermouth;
        public Button BTN_steering;
        public Button BTN_Shaking;
        public Button BTN_Reset;

        public void InitUI()
        {
            P_Ingredient = new Panel(new Vector2(800,600),anchor:Anchor.TopRight, offset: new Vector2(0,0));
            P_Ingredient.Padding = Vector2.Zero;

            BTN_Alcohol = new Button("Alcohol",skin: ButtonSkin.Default, anchor: Anchor.TopRight, size: new Vector2(200, 100), offset: Vector2.Zero);
            BTN_Alcohol.OnMouseDown = (Entity e) =>
            {
                openAlcoholPanel = true;
                Debug.WriteLine("BTN_Alcohol was clicked");
            };

            P_Alcohol = new Panel(new Vector2(800, 600), anchor:Anchor.TopRight, offset: new Vector2(-800, 0));
            P_Alcohol.Padding = Vector2.Zero;

            //add child
            P_Ingredient.AddChild(P_Alcohol);
            P_Ingredient.AddChild(BTN_Alcohol);
            // add Entity
            UserInterface.Active.AddEntity(P_Ingredient);
        }


        public override void Initialize()
        {
            //Add Code Here

            //Base DO NOT DELETE
            base.Initialize();
        }

        public override void LoadContent()
        {   //Base DO NOT DELETE
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            InitUI();

            //Add Code Here
            TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
            Atlas_CustomerNPC = TextureAtlas.FromFile(Content, "images/Customer/CustomerNPC_Define.xml");
            //Add Code Above

            //Base DO NOT DELETE
            base.LoadContent();
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

            UserInterface.Active.Draw(Core.SpriteBatch);
            base.Draw(gameTime);

        }

        // _____________________main funciton__________________
        protected void UpdateUILogic() {
            if (openAlcoholPanel) {
                if (SlidePanel_X_Axis(P_Alcohol, 0, 20, true))
                    openAlcoholPanel = false;
            }
        }   

        // ----------------------Fucntino-----------------------
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
        protected void BTNMethodActive(bool Enable)
        {
            if (Enable == false)
            {
                BTN_steering.Enabled = false;
                BTN_Shaking.Enabled = false;
            }
            if (Enable == true)
            {
                BTN_steering.Enabled = true;
                BTN_Shaking.Enabled = true;
            }
        }
        protected void BTNIngredeientActive(bool Enable) {
            if (Enable == false) { 
                Mixer_CanberryJuice.Enabled = false;
                Mixer_GrapefruitJuice.Enabled = false;
                Mixer_LemonJuice.Enabled = false;
                Mixer_Soda.Enabled = false;
                Mixer_Syrup.Enabled = false;
                Mixer_PepperMint.Enabled = false;
                Alcohol_Vodka.Enabled = false;
                Alcohol_Gin.Enabled = false;
                Alcohol_Triplesec.Enabled = false;
                Alcohol_Vermouth.Enabled = false;
            }
            if (Enable == true) { 
                Mixer_CanberryJuice.Enabled = true;
                Mixer_GrapefruitJuice.Enabled = true;
                Mixer_LemonJuice.Enabled = true;
                Mixer_Soda.Enabled = true;
                Mixer_Syrup.Enabled = true;
                Mixer_PepperMint.Enabled = true;
                Alcohol_Vodka.Enabled = true;
                Alcohol_Gin.Enabled = true;
                Alcohol_Triplesec.Enabled = true;
                Alcohol_Vermouth.Enabled = true;

            }
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
