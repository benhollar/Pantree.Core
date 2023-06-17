using System;
using Pantree.Core.Utilities.Interfaces;
using Pantree.Core.Utilities.Measurement;

namespace Pantree.Core.Cooking
{
    /// <summary>
    /// A unique food item
    /// </summary>
    /// <remarks>
    /// A <see cref="Food"/> is meant to serve as an analog to any food that could be used in a recipe, without any
    /// explicit quantity or other information that would be required in a recipe. A <see cref="Food"/> is "an onion",
    /// not "34 grams of diced onions."
    /// </remarks>
    public record struct Food : Identifiable
    {
        /// <inheritdoc/>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// The name of the food
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The nutritional value of the food, per the labelled <see cref="Measurement"/>, as would be found on a
        /// nutritional label
        /// </summary>
        public Nutrition? Nutrition { get; set; } = null;

        /// <summary>
        /// The basic measurement of the food, such as would be found on a standard nutritional label
        /// </summary>
        public Measurement<FoodUnit>? Measurement { get; set; } = null;

        /// <summary>
        /// Construct a new <see cref="Food"/>
        /// </summary>
        /// <param name="name">The food's name</param>
        public Food(string name)
        {
            Name = name;
        }

        /// <inheritdoc cref="Food(string)"/>
        /// <param name="name">The food's name</param>
        /// <param name="baseNutrition">
        /// The nutritional information for the food, such as found on a nutritional label
        /// </param>
        /// <param name="baseMeasurement">
        /// The unit used for the nutritional information given in <paramref name="baseNutrition"/>
        /// </param>
        public Food(string name, Nutrition baseNutrition, Measurement<FoodUnit> baseMeasurement) : this(name)
        {
            Nutrition = baseNutrition;
            Measurement = baseMeasurement;
        }
    }

}
