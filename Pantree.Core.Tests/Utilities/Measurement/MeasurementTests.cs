using Pantree.Core.Cooking;
using Pantree.Core.Utilities.Measurement;
using Xunit;

namespace Pantree.Core.Tests.Utilities.Measurement
{
    public class MeasurementTests
    {
        [Theory]
        [MemberData(nameof(EqualityTestData))]
        public void EqualityTest(Measurement<FoodUnit> lhs, Measurement<FoodUnit> rhs, bool expected)
        {
            Assert.Equal(expected, lhs == rhs);
            Assert.Equal(expected, lhs.Equals((object)rhs));
            Assert.Equal(!expected, lhs != rhs);
        }

        [Theory]
        [MemberData(nameof(MultiplyTestData))]
        public void MultiplyTest(Measurement<FoodUnit> lhs, double coefficient, Measurement<FoodUnit> expected)
        {
            Assert.Equal(expected, lhs * coefficient);
            Assert.Equal(expected, coefficient * lhs);
        }

        [Theory]
        [MemberData(nameof(DivideTestData))]
        public void DivideTest(Measurement<FoodUnit> lhs, double coefficient, Measurement<FoodUnit> expected)
        {
            Assert.Equal(expected, lhs / coefficient);
        }

        public static IEnumerable<object?[]> EqualityTestData => new List<object?[]>
        {
            new object?[]
            {
                new Measurement<FoodUnit>(1, FoodUnit.Gram), 
                new Measurement<FoodUnit>(1, FoodUnit.Gram), 
                true
            },
            new object?[]
            {
                new Measurement<FoodUnit>(2, FoodUnit.Gram), 
                new Measurement<FoodUnit>(1, FoodUnit.Gram), 
                false
            },
            // Null is not equal
            new object?[]
            {
                new Measurement<FoodUnit>(1, FoodUnit.Gram), 
                null,
                false
            },
            // Tolerant equality
            new object?[]
            {
                new Measurement<FoodUnit>(0.9999999999, FoodUnit.Gram), 
                new Measurement<FoodUnit>(1, FoodUnit.Gram), 
                true
            }, 
            // Units differ, but amount is same (TODO: should this return `true` instead?)
            new object?[]
            {
                new Measurement<FoodUnit>(16, FoodUnit.Ounce), 
                new Measurement<FoodUnit>(1, FoodUnit.Pound), 
                false
            },    
        };

        public static IEnumerable<object?[]> MultiplyTestData => new List<object?[]>
        {
            new object?[]
            {
                new Measurement<FoodUnit>(1, FoodUnit.Gram), 
                2,
                new Measurement<FoodUnit>(2, FoodUnit.Gram)
            }    
        };

        public static IEnumerable<object?[]> DivideTestData => new List<object?[]>
        {
            new object?[]
            {
                new Measurement<FoodUnit>(1, FoodUnit.Gram), 
                2,
                new Measurement<FoodUnit>(0.5, FoodUnit.Gram)
            }    
        };
    }
}
