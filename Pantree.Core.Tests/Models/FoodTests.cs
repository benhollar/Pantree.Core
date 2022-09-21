using Pantree.Core.Models;
using Pantree.Core.Utilities.Measurement;
using Xunit;

namespace Pantree.Core.Tests.Models
{
    public class FoodTests
    {
        [Theory]
        [MemberData(nameof(SampleFoods))]
        public void ConstructionTest(string name, Nutrition? nutrition, Measurement<FoodUnit>? measurement)
        {
            Food food = (nutrition is not null && measurement is not null)
                ? new Food(name, nutrition.Value, measurement)
                : new Food(name);

            Assert.Equal(name, food.Name);
            Assert.Equal(nutrition, food.Nutrition);
            Assert.Equal(measurement, food.Measurement);
        }

        public static IEnumerable<object?[]> SampleFoods => new List<object?[]>
        {
            new object?[] { "apple", null, null },
            new object?[] { "pear", new Nutrition() { Calories = 50 }, new Measurement<FoodUnit>(1, FoodUnit.Gram) }
        };
    }
}
