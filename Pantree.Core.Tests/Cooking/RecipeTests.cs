using System;
using System.Collections.Generic;
using Pantree.Core.Cooking;
using Pantree.Core.Utilities.Measurement;
using Xunit;

namespace Pantree.Core.Tests.Cooking
{
    public class RecipeTests
    {
        [Theory]
        [MemberData(nameof(NutritionCalculationTestData))]
        public void NutritionCalculationTest(Recipe given, Nutrition? expected)
        {
            Assert.Equal(expected, given.TotalNutrition);
            Assert.Equal(expected / given.Servings, given.NutritionPerServing);
        }

        [Theory]
        [MemberData(nameof(TotalTimeCalculationTestData))]
        public void TotalTimeCalculationTest(Recipe given, TimeSpan? expected)
        {
            Assert.Equal(expected, given.TotalTime);
        }

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsTest(Recipe lhs, Recipe? rhs, bool expected)
        {
            bool equal = lhs.Equals(rhs);
            Assert.Equal(expected, equal);

            // Further, evaluate the custom `GetHashCode` adheres to the expected properties
            int lhsHash = lhs.GetHashCode();
            int? rhsHash = rhs?.GetHashCode();
            //  Equal objects have equal hashes
            if (expected)
                Assert.Equal(lhsHash, rhsHash);
            //  Repeated calls to `GetHashCode` return the same result if the object is unmodified
            Assert.Equal(lhsHash, lhs.GetHashCode());
            //  And likewise, modifying the object modifies its hash code
            string? originalName = lhs.Name;
            lhs.Name = "A new name";
            Assert.NotEqual(lhsHash, lhs.GetHashCode());
            lhs.Name = originalName;
        }

        public static IEnumerable<object?[]> NutritionCalculationTestData => new List<object?[]>
        {
            // 1x of a base serving
            new object?[]
            {
                new Recipe("Sample Recipe")
                {
                    Instructions = new() { "Make the dish." },
                    Ingredients = new()
                    {
                        new Ingredient(
                            new Food(
                                "Sample Food",
                                new Nutrition()
                                {
                                    Calories = 540,
                                    TotalFat = 12,
                                    SaturatedFat = 5,
                                    TransFat = 0,
                                    Cholesterol = 90,
                                    Sodium = 680,
                                    Carbohydrates = 84,
                                    Fiber = 6,
                                    Sugar = 20,
                                    Protein = 28
                                },
                                new Measurement<FoodUnit>(1, FoodUnit.Unit)
                            ),
                            new Measurement<FoodUnit>(1, FoodUnit.Unit)
                        )
                    },
                    Servings = 4
                },
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = 0,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = 28
                }
            },
            // 2x of a base serving
            new object?[]
            {
                new Recipe("Sample Recipe")
                {
                    Instructions = new() { "Make the dish." },
                    Ingredients = new()
                    {
                        new Ingredient(
                            new Food(
                                "Sample Food",
                                new Nutrition()
                                {
                                    Calories = 540,
                                    TotalFat = 12,
                                    SaturatedFat = 5,
                                    TransFat = 0,
                                    Cholesterol = 90,
                                    Sodium = 680,
                                    Carbohydrates = 84,
                                    Fiber = 6,
                                    Sugar = 20,
                                    Protein = 28
                                },
                                new Measurement<FoodUnit>(1, FoodUnit.Unit)
                            ),
                            new Measurement<FoodUnit>(2, FoodUnit.Unit)
                        )
                    },
                    Servings = 1
                },
                new Nutrition()
                {
                    Calories = 1080,
                    TotalFat = 24,
                    SaturatedFat = 10,
                    TransFat = 0,
                    Cholesterol = 180,
                    Sodium = 1360,
                    Carbohydrates = 168,
                    Fiber = 12,
                    Sugar = 40,
                    Protein = 56
                }
            },
            // No nutritional info
            new object?[]
            {
                new Recipe("Sample Recipe")
                {
                    Instructions = new() { "Make the dish." },
                    Ingredients = new()
                    {
                        new Ingredient(new Food("Sample Food"), new Measurement<FoodUnit>(1, FoodUnit.Unit))
                    },
                    Servings = 1
                },
                null
            },
            // No ingredients
            new object?[]
            {
                new Recipe("Sample Recipe"),
                null
            }
        };

        public static IEnumerable<object?[]> TotalTimeCalculationTestData => new List<object?[]>
        {
            new object[]
            {
                new Recipe("Sample Recipe")
                { 
                    PreparationTime = TimeSpan.FromMinutes(5),
                    CookingTime = TimeSpan.FromMinutes(20)
                },
                TimeSpan.FromMinutes(25)
            },
            new object[]
            {
                new Recipe("Sample Recipe")
                { 
                    PreparationTime = TimeSpan.FromMinutes(5),
                    CookingTime = null
                },
                TimeSpan.FromMinutes(5)
            },
            new object?[]
            {
                new Recipe("Sample Recipe"),
                null
            }
        };

        public static IEnumerable<object?[]> EqualsTestData()
        {
            Recipe example1 = new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Recipe",
                Description = "Lorem ipsum...",
                Instructions = new() { "1", "2", "3" },
                Ingredients = new()
                {
                    new Ingredient(
                        new Food("Test Food 1")
                        {
                            Id = Guid.NewGuid(),
                            Nutrition = null,
                            Measurement = new Measurement<FoodUnit>(1, FoodUnit.Gram)
                        },
                        new Measurement<FoodUnit>(1, FoodUnit.Gram)
                    )
                    {
                        Id = Guid.NewGuid()
                    },
                    new Ingredient(
                        new Food("Test Food 2")
                        {
                            Id = Guid.NewGuid(),
                            Nutrition = null,
                            Measurement = new Measurement<FoodUnit>(1, FoodUnit.Gram)
                        },
                        new Measurement<FoodUnit>(1, FoodUnit.Gram)
                    )
                    {
                        Id = Guid.NewGuid()
                    },
                },
                Servings = 1,
                PreparationTime = TimeSpan.FromMinutes(30),
                CookingTime = TimeSpan.FromMinutes(40),
            };

            Recipe example2 = new();

            yield return new object?[] { example1, example1, true };
            yield return new object?[] { example1, example2, false };
            yield return new object?[] { example1, null, false };
            yield return new object?[] { example2, null, false };
        }
    }
}
