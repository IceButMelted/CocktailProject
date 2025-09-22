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
        protected int _numberOfOrder;
        protected HashSet<Enum_TypeOfCocktail> _TypeOfCocktailsDrinked = new HashSet<Enum_TypeOfCocktail>();

        public NPC(string name, int numberOfOrder)
        {
            _Name = name;
            _numberOfOrder = numberOfOrder;
        }

        public string GetName()
        {
            return _Name;
        }

        public void AddTypeCocktailDrink(Enum_TypeOfCocktail type) {
            _TypeOfCocktailsDrinked.Add(type);
        }


    }
}
