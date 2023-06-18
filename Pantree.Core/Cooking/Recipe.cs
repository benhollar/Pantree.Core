using System;
using System.Collections.Generic;
using System.Linq;
using Pantree.Core.Utilities.Interfaces;

namespace Pantree.Core.Cooking
{
    /// <summary>
    /// A collection of <see cref="Ingredient"/>s and instructions that, together, create a full demonstration of how
    /// to prepare a given dish.
    /// </summary>
    public record class Recipe : Identifiable 
    {
        /// <inheritdoc/>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// The name of the recipe
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Additional descriptive text about the recipe, beyond its <see cref="Name"/>.
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// The instructions needed to prepare this <see cref="Recipe"/>
        /// </summary>
        public List<string> Instructions { get; set; } = new();

        /// <summary>
        /// The ingredients needed to prepare this <see cref="Recipe"/>
        /// </summary>
        public List<Ingredient> Ingredients { get; set; } = new();
        
        /// <summary>
        /// The number of servings this <see cref="Recipe"/> makes
        /// </summary>
        public uint Servings { get; set; } = 1;

        /// <summary>
        /// The amount of hands-on time spent preparing the <see cref="Recipe"/>
        /// </summary>
        public TimeSpan? PreparationTime { get; set; } = null;

        /// <summary>
        /// The amount of hands-off time spent preparing the <see cref="Recipe"/>, such as time spent in an oven
        /// </summary>
        public TimeSpan? CookingTime { get; set; } = null;

        /// <summary>
        /// The total amount of time needed to prepare the <see cref="Recipe"/> -- the sum of 
        /// <see cref="PreparationTime"/> and <see cref="CookingTime"/>
        /// </summary>
        public TimeSpan? TotalTime => PreparationTime is null && CookingTime is null
            ? null
            : (PreparationTime ?? TimeSpan.Zero) + (CookingTime ?? TimeSpan.Zero);

        /// <summary>
        /// The aggregate nutritional information of the <see cref="Ingredients"/> used in the <see cref="Recipe"/>
        /// </summary>
        /// <remarks>
        /// If there are no ingredients or if all ingredients do not specify nutritional information, this value is 
        /// null. Otherwise, this value reflects the sum of the non-null nutritional information available, which may
        /// be less than the real total if some ingredients omit nutritional information.
        /// </remarks>
        public Nutrition? TotalNutrition => GetAggregateNutrition(Ingredients);

        /// <summary>
        /// The portion of the <see cref="TotalNutrition"/> in one serving (see: <see cref="Servings"/>)
        /// </summary>
        public Nutrition? NutritionPerServing => TotalNutrition is not null
            ? TotalNutrition / Servings
            : null;

        /// <summary>
        /// Construct a new <see cref="Recipe"/>
        /// </summary>
        /// <param name="name">The name of the recipe</param>
        public Recipe(string name = "New Recipe")
        {
            Name = name;
        }

        /// <summary>
        /// Compute the aggregate nutritional information of a collection of ingredients
        /// </summary>
        /// <param name="ingredients">The ingredients</param>
        /// <returns>The summed nutritional information, or null if none is available</returns>
        private static Nutrition? GetAggregateNutrition(List<Ingredient> ingredients)
        {
            if (ingredients.Count == 0)
                return null;

            List<Nutrition> componentNutritionalInfo = ingredients
                .Select(x => x.Nutrition)
                .Where(x => x is not null)
                .Cast<Nutrition>()
                .ToList();
            
            return componentNutritionalInfo.Count > 0
                ? componentNutritionalInfo.Aggregate((sum, next) => (sum + next)!.Value)
                : null;
        }
    }
}   
