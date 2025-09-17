using CocktailProject.ClassCocktail;
using MonoGameLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.NPC
{
    public class NPC
    {
        private string _Name;
        public TextureAtlas _NPCAtlas;
        protected string _CocktailName;
        protected string _TextToOrder;
        protected int _numberOfOrder;
        protected HashSet<Enum_TypeOfCocktail> _TypeOfCocktailsDrinked = new HashSet<Enum_TypeOfCocktail>();

        public NPC(string name, TextureAtlas npcAtlas, string cocktailName, string textToOrder, int numberOfOrder)
        {
            _Name = name;
            _NPCAtlas = npcAtlas;
            _CocktailName = cocktailName;
            _TextToOrder = textToOrder;
            _numberOfOrder = numberOfOrder;
        }

        public string GetName()
        {
            return _Name;
        }

        public string GetCocktailName()
        {
            return _CocktailName;
        }

        public void SetCocktailName(string CocktailName)
        {
            _CocktailName = CocktailName;
        }

        public void AddTypeCocktailDrink(Enum_TypeOfCocktail type) {
            _TypeOfCocktailsDrinked.Add(type);
        }


    }
}
