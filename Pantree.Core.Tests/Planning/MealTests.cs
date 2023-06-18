using System.Collections.Generic;
using Pantree.Core.Cooking;
using Pantree.Core.Planning;
using Pantree.Core.Utilities.Measurement;
using Xunit;

namespace Pantree.Core.Tests.Planning
{
    public class MealTests
    {
        [Theory]
        [MemberData(nameof(AddFoodTestData))]
        public void AddFoodTest(Meal seed, Ingredient food)
        {
            seed.AddFood(food);
            Assert.Contains(food, seed.Foods);
            Assert.Equal(food.Nutrition, seed.PlannedNutrition);
        }

        [Theory]
        [MemberData(nameof(AddRecipeTestData))]
        public void AddRecipeTest(Meal seed, Recipe recipe, Meal expected)
        {
            seed.AddRecipe(recipe);
            Assert.Equal(expected, seed);
        }

        [Theory]
        [MemberData(nameof(RemoveFoodTestData))]
        public void RemoveFoodTest(Meal seed, Ingredient toRemove, bool expected)
        {
            Assert.Equal(expected, seed.RemoveFood(toRemove));
        }

        [Fact]
        public void CloneTest()
        {
            Meal seed = new("Breakfast", 300);
            Ingredient food = new(new Food("Food 1", 
                                           new Nutrition() { Calories = 100 },
                                           new Measurement<FoodUnit>(1, FoodUnit.Unit)),
                                  new Measurement<FoodUnit>(2, FoodUnit.Unit));
            seed.AddFood(food);
            Meal clone = seed.Clone();

            // Check that the clone created a new object, but didn't change the values of the object
            Assert.NotSame(seed, clone);
            Assert.Equal(seed, clone);
            // Check that the clone was a proper deep copy in the context of the meal, but correctly did not copy each
            // referenced food
            Assert.NotSame(seed.Foods, clone.Foods);
            Assert.Same(seed.Foods[0], clone.Foods[0]);
        }

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsTest(Meal lhs, Meal rhs, bool expected)
        {
            if (expected)
                Assert.Equal(lhs, rhs);
            else
                Assert.NotEqual(lhs, rhs);
        }

        public static IEnumerable<object?[]> AddFoodTestData
        {
            get
            {
                object[] Test1()
                {
                    Meal seed = new("Breakfast", 300);
                    Ingredient food = new(new Food("Food 1", 
                                                   new Nutrition() { Calories = 100 },
                                                   new Measurement<FoodUnit>(1, FoodUnit.Unit)),
                                          new Measurement<FoodUnit>(2, FoodUnit.Unit));

                    return new object[] { seed, food };
                }

                return new List<object[]>() { Test1() };
            }
        }

        public static IEnumerable<object?[]> AddRecipeTestData
        {
            get
            {
                object[] Test1() // Simple case; new meal, one recipe
                {
                    Meal seed = new("Breakfast", 300);
                    Recipe recipe = new("Sample Recipe")
                    {
                       Ingredients = new()
                        {
                            new Ingredient(new Food("Food 1"), new(1, FoodUnit.Unit)),
                            new Ingredient(new Food("Food 2"), new(2, FoodUnit.Unit)),
                        } 
                    };

                    Meal expected = new("Breakfast", 300);
                    expected.AddFood(recipe.Ingredients[0]);
                    expected.AddFood(recipe.Ingredients[1]);

                    return new object[] { seed, recipe, expected };
                }

                return new List<object[]>() { Test1() };
            }
        }

        public static IEnumerable<object?[]> RemoveFoodTestData
        {
            get
            {
                Meal seed = new("Breakfast", 300);
                Ingredient food = new(new Food("Food 1", 
                                               new Nutrition() { Calories = 100 },
                                               new Measurement<FoodUnit>(1, FoodUnit.Unit)),
                                      new Measurement<FoodUnit>(2, FoodUnit.Unit));
                seed.AddFood(food);

                Ingredient unknownFood = new(new Food("Food 2"), new Measurement<FoodUnit>(1, FoodUnit.Unit));

                return new List<object[]>()
                {
                    new object[] { seed, food, true },
                    new object[] { seed, unknownFood, false }
                };
            }
        }

        public static IEnumerable<object?[]> EqualsTestData
        {
            get
            {
                Meal meal1 = new("Breakfast", 300);

                Meal meal2 = new("Breakfast", 300); 
                meal2.AddFood(new(new Food("Food 1", 
                                           new Nutrition() { Calories = 100 },
                                           new Measurement<FoodUnit>(1, FoodUnit.Unit)),
                                  new Measurement<FoodUnit>(2, FoodUnit.Unit)));

                Meal meal3 = new("Lunch", 800);

                Meal meal4 = new("Breakfast", 300);

                Meal meal5 = new("Breakfast", 300); 
                meal5.AddFood(meal2.Foods[0]);

                return new List<object[]>()
                {
                    new object[] { meal1, meal2, false },
                    new object[] { meal1, meal3, false },
                    new object[] { meal2, meal3, false },
                    new object[] { meal1, meal4, true },
                    new object[] { meal2, meal5, true },
                };
            }
        }
    }
}
