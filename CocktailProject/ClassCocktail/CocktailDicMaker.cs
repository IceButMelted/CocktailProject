using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.ClassCocktail 
{
    public static class CocktailDicMaker
    {
        // Use readonly for immutable data and lazy initialization
        private static readonly Lazy<Dictionary<string, Cocktail>> _cocktailDictionary =
            new Lazy<Dictionary<string, Cocktail>>(InitializeCocktails);

        public static Dictionary<string, Cocktail> CocktailDictionary => _cocktailDictionary.Value;

        private static Dictionary<string, Cocktail> InitializeCocktails()
        {
            var cocktails = new Dictionary<string, Cocktail>();
            // Example cocktails    
            cocktails.Add("Vodka Martini", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Vodka, 50 }, { Enum_Alcohol.Vermouth, 10 } },
                new Dictionary<Enum_Mixer, int> { },
                Enum_Method.Shaking,
                Enum_Glass.Martini,
                true));

            //add more cocktails as needed
            cocktails.Add("Gin and Tonic", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Gin, 50 } },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.Soda, 100 } },
                Enum_Method.Mixing,
                Enum_Glass.Hi_ball,
                true));

            return cocktails;
        }

        // Alternative: If you need to add cocktails dynamically
        public static void AddCocktail(string name, Cocktail cocktail)
        {
            CocktailDictionary[name] = cocktail;
        }

        // Helper method for cleaner cocktail creation
        public static Cocktail CreateCocktail(
            Dictionary<Enum_Alcohol, int> alcohols,
            Dictionary<Enum_Mixer, int> mixers,
            Enum_Method method = Enum_Method.Shaking,
            Enum_Glass glass = Enum_Glass.NotFix,
            bool garnish = false)
        {
            return new Cocktail(alcohols, mixers, method, glass, garnish);
        }
    }
}
