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
    }
}
