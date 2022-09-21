namespace Pantree.Core.Utilities.Interfaces
{
    /// <summary>
    /// An interface describing objects that contain a unique identifier
    /// </summary>
    public interface Identifiable
    {
        /// <summary>
        /// A unique identifier for the instance
        /// </summary>
        public Guid Id { get; }
    }
}
