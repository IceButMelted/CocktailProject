using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.ClassCocktail
{
    /// <summary>
    /// Provides a way to incrementally build a cocktail.
    /// </summary>
    public class CocktailBuilder : Cocktail
    {
        /// <summary>
        /// Adds or updates the amount of alcohol in the cocktail.
        /// </summary>
        /// <param name="type">The alcohol type to add.</param>
        /// <param name="quantity">The amount to add in ml.</param>
        public void AddOrUpdateAlcohol(Enum_Alcohol type, int quantity)
        {
            if (_alcoholWithQuantity.ContainsKey(type))
            {
                _alcoholWithQuantity[type] += quantity; // Add to existing amount
                _CountPart += quantity; 
            }
            else
            {
                _alcoholWithQuantity[type] = quantity; // New entry
                _CountPart += quantity; 
            }
        }

        /// <summary>
        /// Adds or updates the amount of mixer in the cocktail.
        /// </summary>
        /// <param name="type">The mixer type to add.</param>
        /// <param name="quantity">The amount to add in ml.</param>
        public void AddOrUpdateMixer(Enum_Mixer type, int quantity)
        {
            if (_mixerWithQuantity.ContainsKey(type))
            {
                _mixerWithQuantity[type] += quantity;
                _CountPart += quantity; 
            }
            else
            {
                _mixerWithQuantity[type] = quantity;
                _CountPart += quantity; 
            }
        }

        /// <summary>
        /// Sets the cocktail preparation method.
        /// </summary>
        /// <param name="method">The preparation method (e.g. shaking or mixing).</param>
        public void UseMethod(Enum_Method method)
        {
            this.method = method;
        }

        /// <summary>
        /// Sets the type of glass used for serving the cocktail.
        /// </summary>
        /// <param name="glass">The glass type.</param>
        public void UseGlass(Enum_Glass glass)
        {
            this.glass = glass;
        }
        /// <summary>
        /// Adds ice to the cocktail if desired.
        /// </summary>
        /// <param name="_bool"></param>
        public void AddIce(bool _bool)
        {
            this._AddIce = _bool;
        }

        /// <summary>
        /// Use to Clear all ingredients and settings in the cocktail builder.
        /// </summary>
        public void ClearAllIngredients()
        {
            _alcoholWithQuantity.Clear();
            _mixerWithQuantity.Clear();
            method = Enum_Method.None;
            glass = Enum_Glass.None;
            _AddIce = false;
            _CountPart = 0;
            typeOfCocktail = Enum_TypeOfCocktail.None;
        }

        /// <summary>
        /// Sets the type of cocktail based on the ingredients added to the builder.
        /// </summary>
        public void SetTypeOfCocktailBySearch()
        {
            typeOfCocktail = Enum_TypeOfCocktail.None;

            foreach (var c in CocktailDicMaker.CocktailDictionary.Values)
            {
                if (c.GetDicAlcohol().Count != _alcoholWithQuantity.Count ||
                    c.GetDicMixer().Count != _mixerWithQuantity.Count)
                    continue;

                if (c.GetDicAlcohol().All(a => _alcoholWithQuantity.TryGetValue(a.Key, out var qtyA) && qtyA == a.Value) &&
                    c.GetDicMixer().All(m => _mixerWithQuantity.TryGetValue(m.Key, out var qtyM) && qtyM == m.Value))
                {
                    typeOfCocktail = c.GetTypeOfCocktail();
                    break; // Exit as soon as match is found
                }
            }
        }


        public void IsSameIngredient()
        {
            
        }

        /// <summary>
        /// Converts the builder to a regular Cocktail object.
        /// </summary>
        /// <returns>A new Cocktail instance with the same properties.</returns>
        public Cocktail Build()
        {
            return new Cocktail(
                new Dictionary<Enum_Alcohol, int>(_alcoholWithQuantity),
                new Dictionary<Enum_Mixer, int>(_mixerWithQuantity),
                method,
                glass,
                _AddIce
            );
        }
    }
}
