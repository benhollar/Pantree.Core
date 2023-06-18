using System;
using System.Collections;
using System.Collections.Generic;

namespace Pantree.Core.Planning
{
    /// <summary>
    /// A strictly one week-long collection of <see cref="PlannedDay"/> instances that, together, describe a weekly meal
    /// plan for an individual person.
    /// </summary>
    public partial class MealPlan
    {
        /// <summary>
        /// The underlying container describing each weekday and its associated <see cref="PlannedDay"/> instance
        /// </summary>
        private Dictionary<DayOfWeek, PlannedDay> PlannedDays { get; } = new()
        {
            { DayOfWeek.Sunday, new() },
            { DayOfWeek.Monday, new() },
            { DayOfWeek.Tuesday, new() },
            { DayOfWeek.Wednesday, new() },
            { DayOfWeek.Thursday, new() },
            { DayOfWeek.Friday, new() },
            { DayOfWeek.Saturday, new() },
        };

        /// <summary>
        /// The starting date for this weekly meal plan
        /// </summary>
        public DateOnly StartDate { get; init; }

        /// <summary>
        /// Construct a new <see cref="MealPlan"/> beginning at the provied <paramref name="startDate"/>
        /// </summary>
        /// <param name="startDate">The starting date for this weekly meal plan</param>
        public MealPlan(DateOnly startDate)
        {
            StartDate = startDate;
        }

        /// <summary>
        /// Construct a new <see cref="MealPlan"/> beginning at the provied <paramref name="startDate"/>, using the
        /// provided template for each day of the plan
        /// </summary>
        /// <param name="startDate">The starting date for this weekly meal plan</param>
        /// <param name="templateDay">
        /// A <see cref="PlannedDay"/> to use as the base value for each day of the plan
        /// </param>
        public MealPlan(DateOnly startDate, PlannedDay templateDay) : this(startDate)
        {
            foreach (DayOfWeek day in PlannedDays.Keys)
                PlannedDays[day] = templateDay.Clone();
        }

        /// <summary>
        /// Get the <see cref="PlannedDay"/> describing the given <see cref="DayOfWeek"/>
        /// </summary>
        /// <param name="key">The <see cref="DayOfWeek"/> to retrieve the plan for</param>
        public PlannedDay this[DayOfWeek key] => PlannedDays[key];
    }

    public partial class MealPlan : IEnumerable<(DayOfWeek, PlannedDay)>
    {
        /// <summary>
        /// Create an enumerable collection of the meal plan's days starting from its <see cref="StartDate"/> and
        /// progressing in proper chronological order
        /// </summary>
        /// <returns>The enumerable of the meal plan's days</returns>
        private IEnumerable<(DayOfWeek, PlannedDay)> OrderedDays()
        {
            // The meal plan's starting day is determined by the StartDate, but the week that follows the starting date
            // can be easily constructed using a modulus; weeks are always 7 days, so we count up by 7 and use the
            // modulus to find the correct DayOfWeek.
            for (int iDay = 0; iDay < 7; ++iDay)
            {
                DayOfWeek day = (DayOfWeek)(((int)StartDate.DayOfWeek + iDay) % 7);
                yield return (day, this[day]);
            }
        }

        /// <innheritdoc/>
        public IEnumerator<(DayOfWeek, PlannedDay)> GetEnumerator() => OrderedDays().GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
