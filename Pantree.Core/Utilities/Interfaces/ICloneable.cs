namespace Pantree.Core.Utilities.Interfaces
{
    /// <summary>
    /// A strongly-typed variant of <see cref="System.ICloneable"/>, with the explicit expectation that implementing
    /// types perform a deep copy
    /// </summary>
    public interface ICloneable<T> : System.ICloneable where T : notnull
    {
        /// <summary>
        /// Perform a deep copy of the current object
        /// </summary>
        /// <returns>The deep copy</returns>
        new T Clone();

        /// <inheritdoc cref="Clone"/>
        object System.ICloneable.Clone() => Clone();
    }
}
