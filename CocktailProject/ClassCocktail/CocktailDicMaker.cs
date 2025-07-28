﻿using System;
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
            var _cocktails = new Dictionary<string, Cocktail>();

            // Hight Alcohol
            _cocktails.Add("Cosmopolitan", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Vodka, 4 },
                                                    { Enum_Alcohol.Triplesec, 2 } },
                new Dictionary<Enum_Mixer, int> { {Enum_Mixer.CanberryJuice,2 },
                                                { Enum_Mixer.LemonJuice, 1} },
                Enum_Method.Shaking,
                Enum_Glass.NotFix,
                false,
                Enum_TypeOfCocktail.HighAlcohol));

            _cocktails.Add("Martini", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Gin, 7 },
                                                    { Enum_Alcohol.Vermouth, 1} },
                new Dictionary<Enum_Mixer, int> { },
                Enum_Method.Mixing,
                Enum_Glass.NotFix,
                false,
                Enum_TypeOfCocktail.HighAlcohol));

            _cocktails.Add("White lady", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Gin, 5 },
                                                    { Enum_Alcohol.Triplesec, 3 } },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.LemonJuice, 2 } },
                Enum_Method.Shaking,
                Enum_Glass.NotFix,
                false,
                Enum_TypeOfCocktail.HighAlcohol));

            // Low Alcohol
            _cocktails.Add("Gin fizz", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Gin, 4 } },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.LemonJuice, 3 },
                                                  { Enum_Mixer.Syrup, 2 },
                                                  { Enum_Mixer.Soda, 1 } },
                Enum_Method.Shaking,
                Enum_Glass.NotFix,
                true,
                Enum_TypeOfCocktail.LowAlcohol));

            _cocktails.Add("Greyhound", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Vodka, 4 } },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.GrapefruitJuice, 6 } },
                Enum_Method.Mixing,
                Enum_Glass.NotFix,
                true,
                Enum_TypeOfCocktail.LowAlcohol));

            _cocktails.Add("Sea breeze", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { { Enum_Alcohol.Vodka, 4 } },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.CanberryJuice, 4 },
                                                  { Enum_Mixer.GrapefruitJuice, 2 } },
                Enum_Method.Shaking,
                Enum_Glass.NotFix,
                true,
                Enum_TypeOfCocktail.LowAlcohol));

            /// None Alcohol
            _cocktails.Add("Nojito", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.LemonJuice, 3 },
                                                  { Enum_Mixer.Syrup, 2 },
                                                  { Enum_Mixer.PepperMint, 1 },
                                                  { Enum_Mixer.Soda, 4 }},
                Enum_Method.Mixing,
                Enum_Glass.NotFix,
                true,
                Enum_TypeOfCocktail.NonAlcoholic));

            _cocktails.Add("Cranberry Fizz", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.CanberryJuice, 5 },
                                                  { Enum_Mixer.LemonJuice, 2 },
                                                  { Enum_Mixer.Syrup, 1 },
                                                  { Enum_Mixer.Soda, 2 }},
                Enum_Method.Shaking,
                Enum_Glass.NotFix,
                true,
                Enum_TypeOfCocktail.NonAlcoholic));

            _cocktails.Add("Grapefruit Spritz", CreateCocktail(
                new Dictionary<Enum_Alcohol, int> { },
                new Dictionary<Enum_Mixer, int> { { Enum_Mixer.GrapefruitJuice, 4 },
                                                  { Enum_Mixer.CanberryJuice, 3},
                                                  { Enum_Mixer.Soda, 3 },},
                Enum_Method.Mixing,
                Enum_Glass.NotFix,
                true,
                Enum_TypeOfCocktail.NonAlcoholic));

            return _cocktails;
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
            bool garnish = false,
            Enum_TypeOfCocktail typeOfCocktail = Enum_TypeOfCocktail.None) 
        {
            return new Cocktail(alcohols, mixers, method, glass, garnish, typeOfCocktail);
        }
    }
}
