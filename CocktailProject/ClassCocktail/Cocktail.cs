using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.ClassCocktail
{
    /// <summary>
    /// Represents a cocktail composed of alcohols, mixers, and preparation instructions.
    /// </summary>
    public class Cocktail : IEquatable<Cocktail>
    {
        protected Dictionary<Enum_Alcohol, int> _alcoholWithQuantity;
        protected Dictionary<Enum_Mixer, int> _mixerWithQuantity;
        protected Enum_Method method;
        protected Enum_Glass glass;
        protected int _CountPart = 0;
        protected bool _AddIce = false;

        public Cocktail()
        {
            _alcoholWithQuantity = new Dictionary<Enum_Alcohol, int>();
            _mixerWithQuantity = new Dictionary<Enum_Mixer, int>();
        }

        /// <summary>
        /// Constructor for cocktails with multiple alcohols and mixers.
        /// </summary>
        /// <param name="alcoholWithQuantity">A dictionary of <see cref="Enum_Alcohol"/> and their quantities in ml.</param>
        /// <param name="mixerWithQuantity">A dictionary of <see cref="Enum_Mixer"/> and their quantities in ml.</param>
        /// <param name="method">The method used to prepare the cocktail.</param>
        /// <param name="glass">The type of glass used to serve the cocktail.</param>
        /// <param name="addIce">Indicates whether ice is added to the cocktail.</param>
        public Cocktail(Dictionary<Enum_Alcohol, int> alcoholWithQuantity,
                        Dictionary<Enum_Mixer, int> mixerWithQuantity,
                        Enum_Method method,
                        Enum_Glass glass,
                        bool addIce)
        {
            _alcoholWithQuantity = alcoholWithQuantity ?? new Dictionary<Enum_Alcohol, int>();
            _mixerWithQuantity = mixerWithQuantity ?? new Dictionary<Enum_Mixer, int>();
            this.method = method;
            this.glass = glass;
            _AddIce = addIce;
        }


        /// <summary>
        /// Method to display the cocktail's ingredients and preparation details.
        /// </summary>
        /// <remarks>
        /// This will give you a text output of the cocktail's ingredients, preparation method, glass type, and whether ice is added.
        /// </remarks>
        public string Info()
        {
            string info = "Cocktail Details:\n";
            info += "Alcohols:\n";
            foreach (var item in _alcoholWithQuantity)
            {
                info += $"    {item.Key}: {item.Value} ml\n";
            }
            info += "Mixers:\n";
            foreach (var item in _mixerWithQuantity)
            {
                info += $"    {item.Key}: {item.Value} ml\n";
            }
            info += $"Method: {method}\n";
            info += $"Glass: {glass}\n";
            info += $"Add Ice: {_AddIce}\n";
            info += $"Total Parts: {_CountPart} / 10\n";

            //Debug.WriteLine(info); // For debugging purposes
            return info;
        }

        // Equality implementation
        public bool Equals(Cocktail other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return DictionariesEqual(_alcoholWithQuantity, other._alcoholWithQuantity) &&
                   DictionariesEqual(_mixerWithQuantity, other._mixerWithQuantity) &&
                   method == other.method &&
                   (glass == other.glass || glass == Enum_Glass.NotFix) &&
                   _AddIce == other._AddIce;
        }

        /// <summary>
        /// Check is this cocktail part is not > 10;
        /// </summary>
        public bool IsPartFull()
        {
            return _CountPart >= 10;
        }

        public Enum_Method Getmethod()
        {
            return method;
        }


        // Helper method to compare dictionaries
        protected bool DictionariesEqual<TKey, TValue>(Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
        {
            if (dict1 == null && dict2 == null) return true;
            if (dict1 == null || dict2 == null) return false;
            if (dict1.Count != dict2.Count) return false;

            foreach (var kvp in dict1)
            {
                if (!dict2.ContainsKey(kvp.Key) || !dict2[kvp.Key].Equals(kvp.Value))
                    return false;
            }
            return true;
        }
    }
}
