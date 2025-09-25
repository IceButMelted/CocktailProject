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
using System.Xml.Serialization;

namespace CocktailProject
{
    class Game2 : Core
    {
        public Game2() : base("CocktialProject", 1920, 1080, false)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            ChangeScene(new Scenes.Test_BookRecipes());
        }

        protected override void LoadContent() {
            base.LoadContent();


        }

        
    }
}
