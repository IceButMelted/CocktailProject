using CocktailProject.Class_DialogLogic;
using CocktailProject.ClassCocktail;
using MonoGameLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.NPC
{
    public class NPCBase
    {
        private string _Name;
        public TextureAtlas _NPCAtlas;
        protected string _CocktailName;
        protected string _TextToOrder;
        protected int _numberOfOrder;
        protected HashSet<Enum_TypeOfCocktail> _TypeOfCocktailsDrinked = new HashSet<Enum_TypeOfCocktail>();

        //  New: dialogue script for this NPC
        public Dictionary<ConversationPhase, List<string>> DialogueScripts { get; private set; }

        public NPCBase(
            string name,
            TextureAtlas npcAtlas,
            string cocktailName,
            string textToOrder,
            int numberOfOrder,
            Dictionary<ConversationPhase, List<string>> dialogueScripts)
        {
            _Name = name;
            _NPCAtlas = npcAtlas;
            _CocktailName = cocktailName;
            _TextToOrder = textToOrder;
            _numberOfOrder = numberOfOrder;
            DialogueScripts = dialogueScripts;
        }

        public string GetName() => _Name;

        public string GetCocktailName() => _CocktailName;

        public void SetCocktailName(string cocktailName) => _CocktailName = cocktailName;

        public void AddTypeCocktailDrink(Enum_TypeOfCocktail type)
        {
            _TypeOfCocktailsDrinked.Add(type);
        }


    }
}
