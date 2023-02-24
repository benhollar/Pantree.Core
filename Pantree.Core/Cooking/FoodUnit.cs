using Pantree.Core.Utilities.Measurement;

namespace Pantree.Core.Cooking
{
    /// <summary>
    /// Common units used to describe a quantity of food
    /// </summary>
    public enum FoodUnit
    {
        /// <summary>
        /// An abstract, ill-defined dummy unit. Some foods are best expressed in plain quantities rather than by weight
        /// or volume (i.e. "1 egg" vs. "50g egg")
        /// </summary>
        Unit,
        /// <summary>The SI gram</summary>
        Gram,
        /// <summary>The SI milligram</summary>
        Milligram,
        /// <summary>The United States customary ounce</summary>
        Ounce, 
        /// <summary>The United States customary pound</summary>
        Pound,
        /// <summary>The SI-accepted milliliter</summary>
        Milliliter,
        /// <summary>The SI-accepted liter</summary>
        Liter,
        /// <summary>The United States customary teaspoon</summary>
        Teaspoon,
        /// <summary>The United States customary tablespoon</summary>
        Tablespoon,
        /// <summary>The United States customary fluid ounce</summary>
        FluidOunce,
        /// <summary>The United States customary cup</summary>
        Cup,
    }

    /// <summary>
    /// A <see cref="UnitConverter{T}"/> for the known <see cref="FoodUnit"/> values
    /// </summary>
    public class FoodUnitConverter : UnitConverter<FoodUnit>
    {
        /// <summary>
        /// Configure the <see cref="FoodUnitConverter"/>
        /// </summary>
        static FoodUnitConverter()
        {
            BaseUnit = FoodUnit.Gram;
            // Conversions given as coefficients from unit provided to base unit, i.e. grams
            RegisterConversion(FoodUnit.Unit, 1);
            RegisterConversion(FoodUnit.Gram, 1);
            RegisterConversion(FoodUnit.Milligram, 0.001);
            RegisterConversion(FoodUnit.Ounce, 28.3495);
            RegisterConversion(FoodUnit.Pound, 453.59237);
            // Volumetric units are converted to mass-based units assuming the density of water
            RegisterConversion(FoodUnit.Milliliter, 1);
            RegisterConversion(FoodUnit.Liter, 1000);
            RegisterConversion(FoodUnit.Teaspoon, 4.92892);
            RegisterConversion(FoodUnit.Tablespoon, 14.7868);
            RegisterConversion(FoodUnit.FluidOunce, 29.574);
            RegisterConversion(FoodUnit.Cup, 8 * 29.574);
        }
    }
}
