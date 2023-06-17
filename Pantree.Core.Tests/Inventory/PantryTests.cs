using System;
using System.Collections.Generic;
using System.Linq;
using Pantree.Core.Cooking;
using Pantree.Core.Inventory;
using Xunit;

namespace Pantree.Core.Tests.Inventory
{
    public class PantryTests
    {
        [Theory]
        [MemberData(nameof(GetItemsExpiringSoonTestData))]
        public void GetItemsExpiringSoonTest(Pantry pantry, uint thresholdDays, List<Perishable> expectedItems) 
        {
            List<Perishable> actualItems = pantry.GetItemsExpiringSoon(thresholdDays).ToList();
            Assert.Equal(expectedItems, actualItems);

            // Test equivalent overloads
            Assert.Equal(expectedItems, pantry.GetItemsExpiringSoon(TimeSpan.FromDays(thresholdDays)).ToList());
        }

        [Theory]
        [MemberData(nameof(GetExpiredItemsTestData))]
        public void GetExpiredItemsTest(Pantry pantry, List<Perishable> expectedItems)
        {
            List<Perishable> actualItems = pantry.GetExpiredItems().ToList();
            Assert.Equal(expectedItems, actualItems);
        }

        [Fact]
        public void PantryICollectionTest()
        {
            Pantry samplePantry = new(SamplePerishables);
            
            Assert.Equal(SamplePerishables.Count, samplePantry.Count);
            Assert.False(samplePantry.IsReadOnly);
            foreach (Perishable item in samplePantry)
                Assert.Contains(item, samplePantry);

            samplePantry.Clear();
            Assert.Empty(samplePantry);

            samplePantry.Add(SamplePerishables[0]);
            Assert.Single(samplePantry);
            Assert.DoesNotContain(SamplePerishables[1], samplePantry);

            samplePantry.AddRange(new List<Perishable>() { SamplePerishables[1], SamplePerishables[2] });
            Assert.Equal(3, samplePantry.Count);

            Assert.True(samplePantry.Remove(SamplePerishables[0]));
            Assert.Equal(2, samplePantry.Count);

            IEnumerator<Perishable> enumerator = samplePantry.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal(SamplePerishables[1], enumerator.Current);

            Perishable[] newArray = new Perishable[4];
            samplePantry.CopyTo(newArray, 2);
            Assert.Equal(new Perishable[] { default, default, SamplePerishables[1], SamplePerishables[2] }, newArray);
        }

        public static IEnumerable<object[]> GetItemsExpiringSoonTestData() => new List<object[]>
        {
            new object[] // normal case
            {
                new Pantry(SamplePerishables),
                3,
                new List<Perishable>() { SamplePerishables[1], SamplePerishables[2] }
            },
            new object[] // normal case
            {
                new Pantry(SamplePerishables),
                1000,
                SamplePerishables
            },
            new object[] // edge case: soon matches expiration date of item
            {
                new Pantry(SamplePerishables),
                7,
                SamplePerishables
            }
        };

        public static IEnumerable<object[]> GetExpiredItemsTestData() => new List<object[]>
        {
            new object[]
            {
                new Pantry(SamplePerishables),
                new List<Perishable>() { SamplePerishables[2] } 
            }
        };

        private static List<Perishable> SamplePerishables = new()
        {
            new Perishable(
                new Ingredient(new Food("Dummy food 1"), new(1, FoodUnit.Unit)),
                DateOnly.FromDateTime(DateTime.Today),
                TimeSpan.FromDays(7)
            ),
            new Perishable(
                new Ingredient(new Food("Dummy food 2"), new(1, FoodUnit.Unit)),
                DateOnly.FromDateTime(DateTime.Today),
                TimeSpan.FromDays(1)
            ),
            new Perishable(
                new Ingredient(new Food("Dummy food 3"), new(1, FoodUnit.Unit)),
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(20))
            ),
        };
    }
}
