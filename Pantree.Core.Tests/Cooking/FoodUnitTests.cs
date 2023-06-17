using System.Collections.Generic;
using Pantree.Core.Cooking;
using Pantree.Core.Utilities.Measurement;
using Xunit;

namespace Pantree.Core.Tests.Cooking
{
    public class FoodUnitTests
    {
        [Theory]
        [MemberData(nameof(ConversionTestData))]
        public void ConversionTest(Measurement<FoodUnit> given, FoodUnit desiredUnit, Measurement<FoodUnit> expected)
        {
            FoodUnitConverter converter = new();
            // Base assertion
            Measurement<FoodUnit> actual = converter.Convert(given, desiredUnit);
            Assert.Equal(expected, actual);
            // Conversions should be reversible
            Measurement<FoodUnit> givenDeduced = converter.Convert(actual, given.Unit);
            Assert.Equal(given, givenDeduced);
        }

        public static IEnumerable<object?[]> ConversionTestData => new List<object?[]>
        {
            // Identity Property
            GenerateConversionTestData(1, FoodUnit.Unit, 1, FoodUnit.Unit),
            // Equivalence of "unit" -> "gram"
            GenerateConversionTestData(1, FoodUnit.Unit, 1, FoodUnit.Gram),
            // Conversions involving base unit
            GenerateConversionTestData(  3, FoodUnit.Gram,      3000, FoodUnit.Milligram),
            GenerateConversionTestData(  1, FoodUnit.Gram, 0.0352733, FoodUnit.Ounce),
            GenerateConversionTestData(  1, FoodUnit.Gram, 0.0022046, FoodUnit.Pound),
            GenerateConversionTestData(  1, FoodUnit.Gram,         1, FoodUnit.Milliliter),
            GenerateConversionTestData(  1, FoodUnit.Gram,     0.001, FoodUnit.Liter),
            GenerateConversionTestData(  1, FoodUnit.Gram, 0.2028842, FoodUnit.Teaspoon),
            GenerateConversionTestData(  1, FoodUnit.Gram, 0.0676278, FoodUnit.Tablespoon),
            GenerateConversionTestData(  1, FoodUnit.Gram, 0.0338134, FoodUnit.FluidOunce),
            GenerateConversionTestData(4.5, FoodUnit.Gram, 0.0190200, FoodUnit.Cup),
            // Conversions without base unit
            GenerateConversionTestData(1, FoodUnit.Ounce,   28349.5, FoodUnit.Milligram),
            GenerateConversionTestData(8, FoodUnit.Ounce,       0.5, FoodUnit.Pound),
            GenerateConversionTestData(1, FoodUnit.Ounce,   28.3495, FoodUnit.Milliliter),
            GenerateConversionTestData(1, FoodUnit.Ounce, 0.0283495, FoodUnit.Liter),
            GenerateConversionTestData(1, FoodUnit.Ounce, 5.7516656, FoodUnit.Teaspoon),
            GenerateConversionTestData(1, FoodUnit.Ounce, 1.9172167, FoodUnit.Tablespoon),
            GenerateConversionTestData(1, FoodUnit.Ounce, 0.9585953, FoodUnit.FluidOunce),
            GenerateConversionTestData(1, FoodUnit.Ounce, 0.1198244, FoodUnit.Cup),
        };

        private static object?[] GenerateConversionTestData(double v1, FoodUnit u1, double v2, FoodUnit u2) =>
            new object?[]
            {
                new Measurement<FoodUnit>(v1, u1),
                u2,
                new Measurement<FoodUnit>(v2, u2)
            };
    }
}
