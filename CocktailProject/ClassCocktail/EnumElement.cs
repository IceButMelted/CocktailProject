using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.ClassCocktail
{
    /// <summary>
    /// List of alcohol types.
    /// </summary>
    public enum Enum_Alcohol
    {
        Vodka,
        Gin,
        Triplesec,
        Vermouth
    }

    /// <summary>
    /// List of mixer types.
    /// </summary>
    public enum Enum_Mixer
    {
        CanberryJuice,
        GrapefruitJuice,
        LemonJuice,

        Soda,

        Syrup,
        PepperMint
    }


    /// <summary>
    /// List of glass types used for serving cocktails.
    /// </summary>
    public enum Enum_Glass : byte
    {
        None,
        Hi_ball,
        Rocks,
        Martini,
        Cocktail,
        LongDrink,
        NotFix
    }

    /// <summary>
    /// Methods used to prepare cocktails.
    /// </summary>
    public enum Enum_Method : byte
    {
        None,
        Mixing,
        Shaking
    }

    public enum Enum_TypeOfCocktail : byte
    {
        None,
        HighAlcohol,
        LowAlcohol,
        NonAlcoholic,
        NotMatch
    }
}
