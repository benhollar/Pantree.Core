using System.Collections.Generic;
using System.Linq;
using Pantree.Core.Cooking;
using Pantree.Core.Planning;
using Pantree.Core.Utilities.Measurement;
using Xunit;

namespace Pantree.Core.Tests.Planning
{
    public class PlannedDayTests
    {
        [Fact]
        public void NutritionTest()
        {
            PlannedDay sample = new(SampleMeals.Take(3));
            Assert.Equal(1900u, sample.CalorieGoal);
            Assert.Equal(new Nutrition() { Calories = 1790 }, sample.PlannedNutrition);
        }

        [Fact]
        public void PlannedDayIListTest()
        {
            PlannedDay sample = new(SampleMeals);
            
            Assert.Equal(SampleMeals.Count, sample.Count);
            Assert.False(sample.IsReadOnly);
            foreach (Meal item in sample)
                Assert.Contains(item, sample);

            sample.Clear();
            Assert.Empty(sample);

            sample.Add(SampleMeals[0]);
            Assert.Single(sample);
            Assert.DoesNotContain(SampleMeals[1], sample);

            sample.AddRange(new List<Meal>() { SampleMeals[1], SampleMeals[2] });
            Assert.Equal(3, sample.Count);

            Assert.True(sample.Remove(SampleMeals[0]));
            Assert.Equal(2, sample.Count);

            IEnumerator<Meal> enumerator = sample.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(SampleMeals[1], enumerator.Current);

            Meal[] newArray = new Meal[4];
            sample.CopyTo(newArray, 2);
            Assert.Equal(new Meal?[] { default, default, SampleMeals[1], SampleMeals[2] }, newArray);
        }

        [Fact]
        public void CloneTest()
        {
            PlannedDay original = new(SampleMeals);
            PlannedDay clone = original.Clone();

            Assert.NotSame(original, clone);
            Assert.Equal(original, clone);
            foreach ((Meal originalMeal, Meal cloneMeal) in original.Zip(clone))
            {
                Assert.NotSame(originalMeal, cloneMeal);
                Assert.Equal(originalMeal, cloneMeal);
            }
        }

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsTest(PlannedDay lhs, PlannedDay rhs, bool expected)
        {
            if (expected)
                Assert.Equal(lhs, rhs);
            else
                Assert.NotEqual(lhs, rhs);
        }

        public static IEnumerable<object?[]> EqualsTestData => new List<object[]>()
        {
            new object[] { new PlannedDay(SampleMeals), new PlannedDay(SampleMeals), true },
            new object[] { new PlannedDay(), new PlannedDay(SampleMeals), false },
            new object[] { new PlannedDay(SampleMeals.Take(1)), new PlannedDay(SampleMeals), false },
        };

        private static List<Meal> SampleMeals = new()
        {
            new("Breakfast", 300, new List<Ingredient>()
            {
                new Ingredient(
                    new Food(
                        "Sample Food 1",
                        new Nutrition()
                        {
                            Calories = 240,
                        },
                        new Measurement<FoodUnit>(1, FoodUnit.Unit)
                    ),
                    new Measurement<FoodUnit>(1, FoodUnit.Unit)
                )
            }),
            new("Lunch", 800, new List<Ingredient>()
            {
                new Ingredient(
                    new Food(
                        "Sample Food 2",
                        new Nutrition()
                        {
                            Calories = 250,
                        },
                        new Measurement<FoodUnit>(1, FoodUnit.Unit)
                    ),
                    new Measurement<FoodUnit>(3, FoodUnit.Unit)
                )
            }),
            new("Dinner", 800, new List<Ingredient>()
            {
                new Ingredient(
                    new Food(
                        "Sample Food 3",
                        new Nutrition()
                        {
                            Calories = 800,
                        },
                        new Measurement<FoodUnit>(1, FoodUnit.Unit)
                    ),
                    new Measurement<FoodUnit>(1, FoodUnit.Unit)
                )
            }),
        };
    }
}
