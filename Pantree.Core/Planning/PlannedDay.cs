using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pantree.Core.Cooking;
using Pantree.Core.Utilities.Interfaces;

namespace Pantree.Core.Planning
{
    /// <summary>
    /// A singular day of meals, possibly part of a broader <see cref="MealPlan"/>. Each <see cref="PlannedDay"/> is a
    /// collection of some number of <see cref="Meal"/> instances (e.g. breakfast, lunch, dinner, snacks).
    /// </summary>
    public partial class PlannedDay
    {
        /// <summary>
        /// The <see cref="Meal"/> instances currently describing this day
        /// </summary>
        private List<Meal> _meals = new();

        /// <summary>
        /// The aggregate calorie goal for this day, based upon the <see cref="Meal.CalorieGoal"/> values of the
        /// meals comprising this day
        /// </summary>
        public uint CalorieGoal => Convert.ToUInt32(_meals.Sum(x => x.CalorieGoal));

        /// <summary>
        /// The aggregate nutritional information for the planned meals on this day
        /// </summary>
        public Nutrition PlannedNutrition => _meals
            .SelectMany(x => x.Foods)
            .Aggregate(null, (Nutrition? sum, Ingredient ingredient) => sum + ingredient.Nutrition) ?? new();

        /// <summary>
        /// Construct a new <see cref="PlannedDay"/>
        /// </summary>
        /// <param name="meals">The meals the day consists of</param>
        public PlannedDay(IEnumerable<Meal>? meals = null)
        {
            if (meals is not null)
                _meals.AddRange(meals);
        }
    }

    public partial class PlannedDay : IList<Meal>
    {
        /// <summary>
        /// Access the <see cref="Meal"/> at the given <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index to access; must be strictly less than <see cref="Count"/></param>
        public Meal this[int index]
        {
            get => _meals[index];
            set => _meals[index] = value;
        }

        /// <summary> The number of <see cref="Meal"/> instances on this day </summary>
        public int Count => _meals.Count;

        /// <summary> Is the collection read-only? This is always false for a <see cref="PlannedDay"/> </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add a <see cref="Meal"/> to this <see cref="PlannedDay"/>
        /// </summary>
        /// <param name="meal">The meal to add</param>
        public void Add(Meal meal) => _meals.Add(meal);

        /// <summary>
        /// Add multiple meals to the <see cref="PlannedDay"/>
        /// </summary>
        /// <param name="meals">The meals to add</param>
        public void AddRange(IEnumerable<Meal> meals) => _meals.AddRange(meals);

        /// <summary>
        /// Remove all <see cref="Meal"/> instances from this <see cref="PlannedDay"/>
        /// </summary>
        public void Clear() => _meals.Clear();

        /// <summary>
        /// Does this <see cref="PlannedDay"/> contain the given <paramref name="meal"/>?
        /// </summary>
        /// <param name="meal">The <see cref="Meal"/> to search for</param>
        /// <returns>True if the meal is in this <see cref="PlannedDay"/>, false otherwise</returns>
        public bool Contains(Meal meal) => _meals.Contains(meal);

        /// <summary>
        /// Copy meals from this <see cref="PlannedDay"/> into an existing <paramref name="array"/>
        /// </summary>
        /// <param name="array">The existing array to copy meals into</param>
        /// <param name="arrayIndex">The starting index of <paramref name="array"/> to copy to</param>
        public void CopyTo(Meal[] array, int arrayIndex) => _meals.CopyTo(array, arrayIndex);

        /// <summary>
        /// Determine the index of a <see cref="Meal"/> contained in this <see cref="PlannedDay"/>
        /// </summary>
        /// <param name="meal">The meal to locate</param>
        /// <returns>The index of the meal if found, otherwise -1</returns>
        public int IndexOf(Meal meal) => _meals.IndexOf(meal);

        /// <summary>
        /// Insert a <see cref="Meal"/> into this <see cref="PlannedDay"/> at the given <paramref name="index"/>
        /// </summary>
        /// <param name="index">The position to insert the new <see cref="Meal"/></param>
        /// <param name="meal">The <see cref="Meal"/> to insert</param>
        public void Insert(int index, Meal meal) => _meals.Insert(index, meal);

        /// <summary>
        /// Remove a given <paramref name="meal"/> from this <see cref="PlannedDay"/>
        /// </summary>
        /// <param name="meal">The meal to remove</param>
        /// <returns>True if the meal was found and successfully removed, false otherwise</returns>
        public bool Remove(Meal meal) => _meals.Remove(meal);

        /// <summary>
        /// Remove a <see cref="Meal"/> from this <see cref="PlannedDay"/> at the given <paramref name="index"/>
        /// </summary>
        /// <param name="index">The position to remove the <see cref="Meal"/> from</param>
        public void RemoveAt(int index) => _meals.RemoveAt(index);

        /// <inheritdoc/>
        public IEnumerator<Meal> GetEnumerator() => _meals.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public partial class PlannedDay : ICloneable<PlannedDay>
    {
        /// <inheritdoc/>
        public PlannedDay Clone()
        {
            PlannedDay clone = new();
            foreach (Meal meal in this)
                clone.Add(meal.Clone());
            return clone;
        }
    }

    public partial class PlannedDay : IEquatable<PlannedDay>
    {
        /// <inheritdoc/>
        public bool Equals(PlannedDay? other)
        {
            if (other is null || other.Count != Count)
                return false;

            bool equal = CalorieGoal == other.CalorieGoal;
            foreach ((Meal lhs, Meal rhs) in this.Zip(other))
                equal = equal && lhs.Equals(rhs);
            
            return equal;
        }
    }
}
