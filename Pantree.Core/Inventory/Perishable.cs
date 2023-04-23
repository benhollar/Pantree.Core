using Pantree.Core.Cooking;
using Pantree.Core.Utilities.Interfaces;

namespace Pantree.Core.Inventory
{
    /// <summary>
    /// A <see cref="Perishable"/> describes some <see cref="Ingredient"/> -- a quantity of some food item -- that was
    /// purchased and will eventually expire.
    /// </summary>
    public record struct Perishable : Identifiable
    {
        /// <inheritdoc/>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// A food and quantity
        /// </summary>
        public Ingredient Ingredient { get; init; }

        /// <summary>
        /// The calendar date that the <see cref="Ingredient"/> was purhcased
        /// </summary>
        public DateOnly PurchaseDate { get; private init; }

        /// <summary>
        /// The estimated calendar date that the <see cref="Ingredient"/> is expected to expire
        /// </summary>
        public DateOnly ExpiryDate { get; private init; }

        /// <summary>
        /// Construct a new <see cref="Perishable"/>, computing an <see cref="ExpiryDate"/> given a known
        /// <paramref name="purchaseDate"/> and <paramref name="shelfLife"/>
        /// </summary>
        /// <param name="ingredient">The food and quantity</param>
        /// <param name="purchaseDate">The date the <paramref name="ingredient"/> was purchased</param>
        /// <param name="shelfLife">The typical shelf life of the <paramref name="ingredient"/></param>
        public Perishable(Ingredient ingredient, DateOnly purchaseDate, TimeSpan shelfLife)
        {
            Ingredient = ingredient;
            PurchaseDate = purchaseDate;
            ExpiryDate = purchaseDate.AddDays(shelfLife.Days);
        }

        /// <summary>
        /// Construct a new <see cref="Perishable"/>, computing an <see cref="ExpiryDate"/> given a known
        /// <paramref name="expirationDate"/> and some optional <paramref name="extendedShelfLife"/> modifier
        /// </summary>
        /// <param name="ingredient">The food and quantity</param>
        /// <param name="purchaseDate">The date the <paramref name="ingredient"/> was purchased</param>
        /// <param name="expirationDate">The known estimated expiration date</param>
        /// <param name="extendedShelfLife">
        /// An optional modifier of the expected <paramref name="expirationDate"/>, intended as a mechanism to express
        /// that you may personally view something as expired sooner or later than product packaging may indicate
        /// </param>
        public Perishable(
            Ingredient ingredient, DateOnly purchaseDate, DateOnly expirationDate, TimeSpan? extendedShelfLife = null)
        {
            Ingredient = ingredient;
            PurchaseDate = purchaseDate;

            if (extendedShelfLife is not null)
                expirationDate = expirationDate.AddDays(extendedShelfLife.Value.Days);
            ExpiryDate = expirationDate;
        }
    }
}
