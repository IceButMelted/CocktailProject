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
            CocktailDicMaker.CocktailDictionary
                .Where(c => c.Value.GetDicAlcohol().Count == _alcoholWithQuantity.Count &&
                            c.Value.GetDicMixer().Count == _mixerWithQuantity.Count)
                .ToList()
                .ForEach(c =>
                {
                    if (c.Value.GetDicAlcohol().All(a => _alcoholWithQuantity.ContainsKey(a.Key) && _alcoholWithQuantity[a.Key] == a.Value) &&
                        c.Value.GetDicMixer().All(m => _mixerWithQuantity.ContainsKey(m.Key) && _mixerWithQuantity[m.Key] == m.Value))
                    {
                        typeOfCocktail = c.Value.GetTypeOfCocktail();
                    }
                    else {
                        typeOfCocktail = Enum_TypeOfCocktail.None;
                    }
                });
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
