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
        Gram,
        Milligram,
        Ounce, 
        Pound,
        Milliliter,
        Liter,
        Teaspoon,
        Tablespoon,
        FluidOunce,
        Cup,
    }

    /// <summary>
    /// A <see cref="UnitConverter"/> for the known <see cref="FoodUnit"/> values
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
