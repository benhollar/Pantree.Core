using System;
using System.Collections.Generic;
using System.Linq;
using Pantree.Core.Cooking;
using Pantree.Core.Utilities.Interfaces;

namespace Pantree.Core.Planning
{
    /// <summary>
    /// A meal, such as "breakfast" or "lunch", describes the foods planned to be consumed in a given sitting
    /// </summary>
    public partial class Meal
    {
        /// <summary>
        /// The name of the meal (e.g. "breakfast", "lunch", "dinner", "snacks", etc.)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The individual foods planned for this meal
        /// </summary>
        public IReadOnlyList<Ingredient> Foods => _foods.AsReadOnly();
        private List<Ingredient> _foods = new();

        /// <summary>
        /// The ideal caloric consumption for this meal as part of a broader calorie management plan
        /// </summary>
        public uint CalorieGoal { get; set; }

        /// <summary>
        /// The nutritional information of the <see cref="Foods"/> planned for this meal
        /// </summary>
        public Nutrition PlannedNutrition => Foods
            .Aggregate(null, (Nutrition? sum, Ingredient ingredient) => sum + ingredient.Nutrition) ?? new();

        /// <summary>
        /// Construct a new <see cref="Meal"/>
        /// </summary>
        /// <param name="name">The name of the category</param>
        /// <param name="calorieGoal">The ideal calorie goal for the meal</param>
        /// <param name="foods">Foods already planned for this meal, if any</param>
        public Meal(string name, uint calorieGoal, IEnumerable<Ingredient>? foods = null)
        {
            Name = name;
            CalorieGoal = calorieGoal;
            if (foods is not null)
                _foods.AddRange(foods);
        }

        /// <summary>
        /// Add all of the components of a <paramref name="recipe"/> to this meal
        /// </summary>
        /// <param name="recipe">The <see cref="Recipe"/> to plan</param>
        public void AddRecipe(Recipe recipe) => _foods.AddRange(recipe.Ingredients);

        /// <summary>
        /// Add a given food to this meal
        /// </summary>
        /// <param name="food">The food and its quantity to add</param>
        public void AddFood(Ingredient food) => _foods.Add(food);

        /// <summary>
        /// Remove a food from this meal
        /// </summary>
        /// <param name="food">The food to remove</param>
        /// <returns>True if the food was found and successfully removed, false otherwise</returns>
        public bool RemoveFood(Ingredient food) => _foods.Remove(food);
    }

    public partial class Meal : ICloneable<Meal>
    {
        /// <inheritdoc/>
        /// <remarks>
        /// The <see cref="Foods"/> contained in this <see cref="Meal"/> will not be cloned; all other values are
        /// properly deeply copied. The contained foods are not copied because of their nature; they are not owned by
        /// the meal, so their duplication is unnecessary.
        /// </remarks>
        public Meal Clone()
        {
            Meal clone = new(Name, CalorieGoal);
            foreach (Ingredient food in Foods)
                clone.AddFood(food);
            return clone;
        }
    }

    public partial class Meal : IEquatable<Meal>
    {
        /// <inheritdoc/>
        public bool Equals(Meal? other)
        {
            if (other is null || Foods.Count != other.Foods.Count)
                return false;

            bool equal = Name == other.Name && CalorieGoal == other.CalorieGoal;
            foreach ((Ingredient lhs, Ingredient rhs) in Foods.Zip(other.Foods))
                equal = equal && lhs.Equals(rhs);
            
            return equal;
        }
    }
}
