using System.Collections;
using Pantree.Core.Utilities.Interfaces;

namespace Pantree.Core.Inventory
{
    /// <summary>
    /// A collection of <see cref="Perishable"/> items that, together, represent a household's or organizations entire
    /// supply of food. A <see cref="Pantry"/> offers functionality for inventory management (CRUD) as well as tracking
    /// the expiry of individual items.
    /// </summary>
    public partial class Pantry : Identifiable
    {
        /// <inheritdoc/>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// A read-only view of the items in this <see cref="Pantry"/>. Use other methods (e.g. <see cref="Add"/>, 
        /// <see cref="Remove"/>) to modify the collection
        /// </summary>
        public IReadOnlyList<Perishable> Items => _items.AsReadOnly();
        private List<Perishable> _items = new();

        /// <summary>
        /// Construct a new <see cref="Pantry"/>, optionally with some initial <paramref name="contents"/>
        /// </summary>
        /// <param name="contents">The initial contents of the <see cref="Pantry"/>, if any</param>
        public Pantry(IEnumerable<Perishable>? contents = null)
        {
            if (contents is not null)
                _items.AddRange(contents);
        }

        /// <summary>
        /// Find the pantry items expiring soon (i.e. within some number of days)
        /// </summary>
        /// <param name="thresholdDays">The threshold number of days defining "soon"</param>
        /// <returns>The soon-to-expire items</returns>
        public IEnumerable<Perishable> GetItemsExpiringSoon(uint thresholdDays = 3)
        {        
            int todayNumber = DateOnly.FromDateTime(DateTime.Today).DayNumber;
            return Items.Where(perishable => (perishable.ExpiryDate.DayNumber - todayNumber) <= thresholdDays);
        }

        /// <inheritdoc cref="GetItemsExpiringSoon(uint)"/>
        /// <param name="threshold">The threshold amount of time defining "soon"</param>
        public IEnumerable<Perishable> GetItemsExpiringSoon(TimeSpan threshold) =>
            GetItemsExpiringSoon(Convert.ToUInt32(threshold.Days));

        /// <summary>
        /// Find the pantry items that have expired
        /// </summary>
        /// <remarks>
        /// This is functionally equivalent to <see cref="GetItemsExpiringSoon(uint)"/> with a threshold of 0 days
        /// </remarks>
        /// <returns>The expired items</returns>
        public IEnumerable<Perishable> GetExpiredItems() => GetItemsExpiringSoon(thresholdDays: 0);
    }

    public partial class Pantry : ICollection<Perishable>
    {
        /// <summary> The number of items, including expired items, in the <see cref="Pantry"/> </summary>
        public int Count => Items.Count;

        /// <summary> Is the collection read-only? This is always false for a <see cref="Pantry"/> </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add an item to the <see cref="Pantry"/>
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(Perishable item) => _items.Add(item);

        /// <summary>
        /// Add multiple items to the <see cref="Pantry"/>
        /// </summary>
        /// <param name="items">The items to add</param>
        public void AddRange(IEnumerable<Perishable> items) => _items.AddRange(items);

        /// <summary>
        /// Remove an <paramref name="item"/> from this <see cref="Pantry"/>
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True when the item was found and removed, False otherwise</returns>
        public bool Remove(Perishable item) => _items.Remove(item);

        /// <summary> Empty the <see cref="Pantry"/> entirely </summary>
        public void Clear() => _items.Clear();

        /// <summary>
        /// Does the <see cref="Pantry"/> contain a specific <paramref name="item"/>?
        /// </summary>
        /// <param name="item">The item that may be contained in the <see cref="Pantry"/></param>
        /// <returns>The result of the search</returns>
        public bool Contains(Perishable item) => Items.Contains(item);

        /// <summary>
        /// Copy items from this <see cref="Pantry"/> into an existing <paramref name="array"/>
        /// </summary>
        /// <param name="array">The existing array to copy items into</param>
        /// <param name="arrayIndex">The starting index of <paramref name="array"/> to copy to</param>
        public void CopyTo(Perishable[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        
        /// <summary>
        /// Get an enumerator for this <see cref="Pantry"/>
        /// </summary>
        /// <returns>The enumerator</returns>

        public IEnumerator<Perishable> GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc cref="GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
