using Pantree.Core.Utilities.Interfaces;
using Pantree.Core.Utilities.Measurement;

namespace Pantree.Core.Cooking
{
    /// <summary>
    /// A combination of a <see cref="Models.Food"/> and a <see cref="Measurement{FoodUnit}"/> that serves as the
    /// primary building block of a <see cref="Recipe"/>.
    /// </summary>
    public record struct Ingredient : Identifiable
    {
        /// <inheritdoc/>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// The food component of the <see cref="Ingredient"/>
        /// </summary>
        public Food Food { get; set; }

        /// <summary>
        /// The amount of the <see cref="Food"/> needed in this <see cref="Ingredient"/>
        /// </summary>
        public Measurement<FoodUnit> Quantity { get; set; }

        /// <summary>
        /// The nutritional information of the <see cref="Food"/> in the <see cref="Quantity"/> specified
        /// </summary>
        public Nutrition? Nutrition
        {
            get
            {
                if (Food.Nutrition is null || Food.Measurement is null)
                    return null;

                Measurement<FoodUnit> amountOfBaseNutrition = 
                    new FoodUnitConverter().Convert(Quantity, Food.Measurement.Unit) / Food.Measurement.Value;
                
                return Food.Nutrition * amountOfBaseNutrition.Value;
            }
        }

        /// <summary>
        /// Construct a new <see cref="Ingredient"/>
        /// </summary>
        /// <param name="food">The food used</param>
        /// <param name="quantity">The amount of the <paramref name="food"/> needed</param>
        public Ingredient(Food food, Measurement<FoodUnit> quantity)
        {
            Food = food;
            Quantity = quantity;
        }
    }
}
