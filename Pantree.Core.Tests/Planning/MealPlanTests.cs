using System;
using System.Collections.Generic;
using System.Linq;
using Pantree.Core.Cooking;
using Pantree.Core.Planning;
using Xunit;

namespace Pantree.Core.Tests.Planning
{
    public class MealPlanTests
    {
        [Theory]
        [MemberData(nameof(TemplateDayConstructorTestData))]
        public void TemplateDayConstructorTest(PlannedDay templateDay)
        {
            MealPlan plan = new(DateOnly.FromDateTime(DateTime.Today), templateDay);
            foreach ((_, PlannedDay day) in plan)
            {
                Assert.NotSame(templateDay, day);
                Assert.Equal(templateDay, day);
            }
        }

        [Theory]
        [MemberData(nameof(EnumerableDaysTestData))]
        public void EnumerableDaysTest(DateOnly startDate, List<DayOfWeek> expected)
        {
            MealPlan mealPlan = new(startDate);
            List<(DayOfWeek day, PlannedDay plan)> actual = mealPlan.ToList();

            Assert.Equal(expected, actual.Select(x => x.day).ToList());
            foreach ((DayOfWeek day, PlannedDay plan) in actual)
                Assert.Same(mealPlan[day], plan);
        }

        public static IEnumerable<object?[]> TemplateDayConstructorTestData
        {
            get
            {
                object[] Test1()
                {
                    PlannedDay day = new();
                    day.Add(new Meal("Breakfast", 300));
                    day.Add(new Meal("Lunch", 700));
                    day.Add(new Meal("Dinner", 800));
                    day[0].AddFood(new Ingredient(new Food("Egg"), new(2, FoodUnit.Unit)));
                    return new object[] { day };
                }

                return new List<object[]>() { Test1() };
            }
        }

        public static IEnumerable<object?[]> EnumerableDaysTestData => new List<object?[]>
        {
            new object[]
            {
                new DateOnly(2023, 6, 11), // Sunday,
                new List<DayOfWeek>()
                {
                    DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                    DayOfWeek.Friday, DayOfWeek.Saturday
                }
            },
            new object[]
            {
                new DateOnly(2023, 6, 12), // Monday,
                new List<DayOfWeek>()
                {
                    DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday,
                    DayOfWeek.Saturday, DayOfWeek.Sunday
                }
            },
            new object[]
            {
                new DateOnly(2023, 6, 16), // Friday,
                new List<DayOfWeek>()
                {
                    DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday, DayOfWeek.Thursday, 
                }
            }
        };
    }
}
