using System;

namespace Pantree.Core.Utilities.Measurement
{
    /// <summary>
    /// A measurement, as defined by a value of some unit
    /// </summary>
    public partial class Measurement<TUnit> where TUnit : Enum
    {
        /// <summary>
        /// The unit defining the measurement
        /// </summary>
        public TUnit Unit { get; set; }

        /// <summary>
        /// The quantity of the unit measured
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Construct a new <see cref="Measurement"/>
        /// </summary>
        /// <param name="value">The quantity of the unit measured</param>
        /// <param name="unit">The unit defining the measurement</param>
        public Measurement(double value, TUnit unit)
        {
            Unit = unit;
            Value = value;
        }
        
        /// <summary>
        /// Convert the <see cref="Measurement"/> into a string-based form, using the format "{Value} {Unit}"
        /// </summary>
        /// <returns>The string representation of the <see cref="Measurement"/></returns>
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }

    public partial class Measurement<TUnit> : IEquatable<Measurement<TUnit>>
    {
        /// <summary>
        /// Check if this <see cref="Measurement{T}"/> instance is equal to another
        /// </summary>
        /// <remarks>
        /// A <see cref="Measurement{T}"/> is equal to another if it has they share the same value (within some 
        /// tolerance; default 1e-6) and the same unit.
        /// </remarks>
        /// <param name="other">The other instance</param>
        /// <returns>The result of the comparison</returns>
        public bool Equals(Measurement<TUnit>? other) => Equals(other, tolerance: 1e-6);

        /// <summary>
        /// Check if this <see cref="Measurement{T}"/> instance is equal to another
        /// </summary>
        /// <remarks>
        /// A <see cref="Measurement{T}"/> is equal to another if it has they share the same value (within some 
        /// <paramref name="tolerance"/>) and the same unit.
        /// </remarks>
        /// <param name="other">The other instance</param>
        /// <param name="tolerance"/>
        /// <returns>The result of the comparison</returns>
        public bool Equals(Measurement<TUnit>? other, double tolerance) =>
            other is not null && Unit.Equals(other.Unit) && Math.Abs(Value - other.Value) < tolerance;

        /// <summary>
        /// Check if this <see cref="Measurement{T}"/> instance is equal to another
        /// </summary>
        /// <remarks>
        /// A <see cref="Measurement{T}"/> is equal to another if it has they share the same value (within some 
        /// tolerance; default 1e-6) and the same unit.
        /// </remarks>
        /// <param name="other">The other instance</param>
        /// <returns>The result of the comparison</returns>
        public override bool Equals(object? other) => Equals((other as Measurement<TUnit>));

        /// <summary>
        /// Check if two <see cref="Measurement{T}"/> instances are equal
        /// </summary>
        /// <remarks>
        /// A <see cref="Measurement{T}"/> is equal to another if it has they share the same value (within some 
        /// tolerance; default 1e-6) and the same unit.
        /// </remarks>
        /// <param name="lhs">The first instance</param>
        /// <param name="rhs">The second instance</param>
        /// <returns>The result of the comparison</returns>
        public static bool operator ==(Measurement<TUnit> lhs, Measurement<TUnit> rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Check if two <see cref="Measurement{T}"/> instances are not equal
        /// </summary>
        /// <remarks>
        /// A <see cref="Measurement{T}"/> is equal to another if it has they share the same value (within some 
        /// tolerance; default 1e-6) and the same unit.
        /// </remarks>
        /// <param name="lhs">The first instance</param>
        /// <param name="rhs">The second instance</param>
        /// <returns>The result of the comparison</returns>
        public static bool operator !=(Measurement<TUnit> lhs, Measurement<TUnit> rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Multiply a <paramref name="measurement"/> by some <paramref name="coefficient"/>
        /// </summary>
        /// <param name="measurement">The measurement whose value to multiply</param>
        /// <param name="coefficient">The coefficient to multiply by</param>
        /// <returns>The multiplied measurement</returns>
        public static Measurement<TUnit> operator *(Measurement<TUnit> measurement, double coefficient) =>
            new Measurement<TUnit>(measurement.Value * coefficient, measurement.Unit);
        /// <inheritdoc cref="operator *(Measurement{TUnit}, double)"/>
        public static Measurement<TUnit> operator *(double coefficient, Measurement<TUnit> measurement) =>
            new Measurement<TUnit>(measurement.Value * coefficient, measurement.Unit);

        /// <summary>
        /// Divide a <paramref name="measurement"/> by some <paramref name="coefficient"/>
        /// </summary>
        /// <param name="measurement">The measurement whose value to divide</param>
        /// <param name="coefficient">The coefficient to divide by</param>
        /// <returns>The multiplied measurement</returns>
        public static Measurement<TUnit> operator /(Measurement<TUnit> measurement, double coefficient) =>
            new Measurement<TUnit>(measurement.Value / coefficient, measurement.Unit);

        /// <summary>
        /// Generate a hash code identifying the <see cref="Measurement{T}"/>
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 14689;
                result ^= Unit.GetHashCode();
                result ^= Value.GetHashCode();
                return result;
            }
        }
    }
}
