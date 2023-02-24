using Pantree.Core.Cooking;
using Xunit;

namespace Pantree.Core.Tests.Cooking
{
    public class NutritionTests
    {
        [Theory]
        [MemberData(nameof(AdditionTestData))]
        public void AdditionTest(Nutrition? lhs, Nutrition? rhs, Nutrition? expected)
        {
            // Check that addition is commutative, though it's unlikely not to be
            Assert.Equal(expected, lhs + rhs);
            Assert.Equal(expected.GetHashCode(), (lhs + rhs).GetHashCode());
            Assert.Equal(expected, rhs + lhs);
            Assert.Equal(expected.GetHashCode(), (rhs + lhs).GetHashCode());
        }

        [Theory]
        [MemberData(nameof(MultiplicationTestData))]
        public void MultiplicationTest(Nutrition? lhs, double rhs, Nutrition? expected, bool expectError = false)
        {
            if (expectError)
            {
                Assert.Throws<ArgumentException>(() => lhs * rhs);
                return;
            }

            // Check that multiplication is commutative
            Assert.Equal(expected, lhs * rhs);
            Assert.Equal(expected.GetHashCode(), (lhs * rhs).GetHashCode());
            Assert.Equal(expected, rhs * lhs);
            Assert.Equal(expected.GetHashCode(), (rhs * lhs).GetHashCode());
        }

        [Theory]
        [MemberData(nameof(DivisionTestData))]
        public void DivisionTest(Nutrition? lhs, double rhs, Nutrition? expected, bool expectError = false)
        {
            if (expectError)
            {
                Assert.Throws<ArgumentException>(() => lhs / rhs);
                return;
            }

            Assert.Equal(expected, lhs / rhs);
            Assert.Equal(expected.GetHashCode(), (lhs / rhs).GetHashCode());
        }

        public static IEnumerable<object?[]> AdditionTestData => new List<object?[]>
        {
            // Normal case, all fields provided
            new object[]
            {
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
                new Nutrition()
                {
                    Calories = 150,
                    TotalFat = 5,
                    SaturatedFat = 2,
                    TransFat = 1,
                    Cholesterol = 20,
                    Sodium = 610,
                    Carbohydrates = 22,
                    Fiber = 1,
                    Sugar = 5,
                    Protein = 6
                },
                new Nutrition()
                {
                    Calories = 690,
                    TotalFat = 17,
                    SaturatedFat = 7,
                    TransFat = 1,
                    Cholesterol = 110,
                    Sodium = 1290,
                    Carbohydrates = 106,
                    Fiber = 7,
                    Sugar = 25,
                    Protein = 34
                }
            },
            // Normal case, some fields omitted
            new object[]
            {
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = null,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = null
                },
                new Nutrition()
                {
                    Calories = 150,
                    TotalFat = 5,
                    SaturatedFat = 2,
                    TransFat = 1,
                    Cholesterol = 20,
                    Sodium = 610,
                    Carbohydrates = 22,
                    Fiber = null,
                    Sugar = 5,
                    Protein = null
                },
                new Nutrition()
                {
                    Calories = 690,
                    TotalFat = 17,
                    SaturatedFat = 7,
                    TransFat = 1,
                    Cholesterol = 110,
                    Sodium = 1290,
                    Carbohydrates = 106,
                    Fiber = 6,
                    Sugar = 25,
                    Protein = null
                }
            },
            // Normal case, no nutrition at all
            new object?[]
            {
                null,
                null,
                null,
            },
            // Normal case, subset is missing nutrition
            new object?[]
            {
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
                null,
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
            }
        };

        public static IEnumerable<object?[]> MultiplicationTestData => new List<object?[]>
        {
            // Normal case, all fields provided
            new object[]
            {
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
                2,
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
            // Normal case, all fields provided, scalar < 1
            new object[]
            {
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
                0.5,
                new Nutrition()
                {
                    Calories = 270,
                    TotalFat = 6,
                    SaturatedFat = 2,
                    TransFat = 0,
                    Cholesterol = 45,
                    Sodium = 340,
                    Carbohydrates = 42,
                    Fiber = 3,
                    Sugar = 10,
                    Protein = 14
                }
            },
            // Normal case, some fields omitted
            new object[]
            {
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = null,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = null
                },
                2,
                new Nutrition()
                {
                    Calories = 1080,
                    TotalFat = 24,
                    SaturatedFat = 10,
                    TransFat = null,
                    Cholesterol = 180,
                    Sodium = 1360,
                    Carbohydrates = 168,
                    Fiber = 12,
                    Sugar = 40,
                    Protein = null
                }
            },
            // Normal case, no nutrition
            new object?[]
            {
                null,
                1,
                null,
            },
            // Edge case, scalar of 0
            new object[]
            {
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = null,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = null
                },
                0,
                new Nutrition()
                {
                    Calories = 0,
                    TotalFat = 0,
                    SaturatedFat = 0,
                    TransFat = null,
                    Cholesterol = 0,
                    Sodium = 0,
                    Carbohydrates = 0,
                    Fiber = 0,
                    Sugar = 0,
                    Protein = null
                }
            },
            // Error case, negative scalar
            new object?[]
            {
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = null,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = null
                },
                -2,
                null, // won't get a return value
                true
            }
        };

        public static IEnumerable<object?[]> DivisionTestData => new List<object?[]>
        {
            // Normal case, all fields provided
            new object[]
            {
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
                2,
                new Nutrition()
                {
                    Calories = 270,
                    TotalFat = 6,
                    SaturatedFat = 2,
                    TransFat = 0,
                    Cholesterol = 45,
                    Sodium = 340,
                    Carbohydrates = 42,
                    Fiber = 3,
                    Sugar = 10,
                    Protein = 14
                }
            },
            // Normal case, all fields provided, divisor < 1
            new object[]
            {
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
                0.5,
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
            // Normal case, some fields omitted
            new object[]
            {
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = null,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = null
                },
                2,
                new Nutrition()
                {
                    Calories = 270,
                    TotalFat = 6,
                    SaturatedFat = 2,
                    TransFat = null,
                    Cholesterol = 45,
                    Sodium = 340,
                    Carbohydrates = 42,
                    Fiber = 3,
                    Sugar = 10,
                    Protein = null
                }
            },
            // Normal case, no nutrition
            new object?[]
            {
                null,
                1,
                null,
            },
            // Error case, divisor of 0
            new object?[]
            {
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = null,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = null
                },
                0,
                null,
                true
            },
            // Error case, negative divisor
            new object?[]
            {
                new Nutrition()
                {
                    Calories = 540,
                    TotalFat = 12,
                    SaturatedFat = 5,
                    TransFat = null,
                    Cholesterol = 90,
                    Sodium = 680,
                    Carbohydrates = 84,
                    Fiber = 6,
                    Sugar = 20,
                    Protein = null
                },
                -2,
                null, // won't get a return value
                true
            }
        };
    }
}
