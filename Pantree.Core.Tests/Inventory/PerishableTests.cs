using System;
using System.Collections.Generic;
using Pantree.Core.Cooking;
using Pantree.Core.Inventory;
using Xunit;

namespace Pantree.Core.Tests.Inventory
{
    public class PerishableTests
    {
        [Theory]
        [MemberData(nameof(ConstructFromShelfLifeTestData))]
        public void ConstructFromShelfLifeTest(
            Ingredient ingredient, DateOnly purchaseDate, TimeSpan shelfLife, DateOnly expectedExpiry)
        {
            Perishable actual = new(ingredient, purchaseDate, shelfLife);
            Assert.Equal(ingredient, actual.Ingredient);
            Assert.Equal(purchaseDate, actual.PurchaseDate);
            Assert.Equal(expectedExpiry, actual.ExpiryDate);
        }

        [Theory]
        [MemberData(nameof(ConstructFromExpirationDateTestData))]
        public void ConstructFromExpirationDateTest(
            Ingredient ingredient, DateOnly purchaseDate, DateOnly expirationDate, TimeSpan? extendedShelfLife, 
            DateOnly expectedExpiry)
        {
            Perishable actual = new(ingredient, purchaseDate, expirationDate, extendedShelfLife);
            Assert.Equal(ingredient, actual.Ingredient);
            Assert.Equal(purchaseDate, actual.PurchaseDate);
            Assert.Equal(expectedExpiry, actual.ExpiryDate);
        }

        public static IEnumerable<object[]> ConstructFromShelfLifeTestData() => new List<object[]>
        {
            new object[] // normal case: expires in 7 days
            {
                new Ingredient(new Food("Dummy food"), new(1, FoodUnit.Unit)),
                new DateOnly(2023, 4, 23),
                TimeSpan.FromDays(7),
                new DateOnly(2023, 4, 30)
            },
            new object[] // edge case: expires in 0.5 days (i.e., expires within same day)
            {
                new Ingredient(new Food("Dummy food"), new(1, FoodUnit.Unit)),
                new DateOnly(2023, 4, 23),
                TimeSpan.FromDays(0.5),
                new DateOnly(2023, 4, 23)
            },
        };

        public static IEnumerable<object?[]> ConstructFromExpirationDateTestData() => new List<object?[]>
        {
            new object?[] // normal case (known expiration)
            {
                new Ingredient(new Food("Dummy food"), new(1, FoodUnit.Unit)),
                new DateOnly(2023, 4, 23),
                new DateOnly(2023, 4, 30),
                null,
                new DateOnly(2023, 4, 30),
            },
            new object?[] // normal case (flexed expiration)
            {
                new Ingredient(new Food("Dummy food"), new(1, FoodUnit.Unit)),
                new DateOnly(2023, 4, 23),
                new DateOnly(2023, 4, 30),
                TimeSpan.FromDays(2),
                new DateOnly(2023, 5, 2)
            },
            new object?[] // edge case: negative flexed expiration
            {
                new Ingredient(new Food("Dummy food"), new(1, FoodUnit.Unit)),
                new DateOnly(2023, 4, 23),
                new DateOnly(2023, 4, 30),
                TimeSpan.FromDays(-2),
                new DateOnly(2023, 4, 28)
            }
        };
    }
}
