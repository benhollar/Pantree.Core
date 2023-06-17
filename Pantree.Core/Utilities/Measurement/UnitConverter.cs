using System;
using System.Collections.Generic;

namespace Pantree.Core.Utilities.Measurement
{
    /// <summary>
    /// A pair of functions used to define how a unit is converted to and from another (the base unit)
    /// </summary>
    public struct UnitConversion
    {
        /// <summary>
        /// A function that converts the given unit to the base unit
        /// </summary>
        public Func<double, double> ToBaseUnit { get; init; }

        /// <summary>
        /// A function that converts the base unit to the given unit
        /// </summary>
        public Func<double, double> FromBaseUnit { get; init; }

        /// <summary>
        /// Make a new <see cref="UnitConversion"/>
        /// </summary>
        /// <param name="toBaseUnit">A function that converts the given unit to the base unit</param>
        /// <param name="fromBaseUnit">A function that converts the base unit to the given unit</param>
        public UnitConversion(Func<double, double> toBaseUnit, Func<double, double> fromBaseUnit)
        {
            ToBaseUnit = toBaseUnit;
            FromBaseUnit = fromBaseUnit;
        }
    }
    
    /// <summary>
    /// A utility class for registering and performing conversions between units of a given type
    /// </summary>
    /// <remarks>
    /// Implementations of this class, to be functional, must set <see cref="BaseUnit"/> and register one or more unit
    /// conversions.
    /// </remarks>
    /// <typeparam name="TUnit">The enumeration of units that this converter works upon</typeparam>
    public abstract class UnitConverter<TUnit> where TUnit : Enum
    {
        // Silence a nullability warning here; we expect subclasses to set this value in a static constructor, but there
        // is no apparent way to _enforce_ that. If an implementation is negligent, that's not really our problem.
        #nullable disable
            /// <summary>
            /// The unit used for conversion arithmetic; other units will be converted to and from this unit as needed.
            /// </summary>
            protected static TUnit BaseUnit;
        #nullable enable

        /// <summary>
        /// The known (i.e. registered) unit conversions
        /// </summary>
        private static Dictionary<TUnit, UnitConversion> Conversions { get; set; } = new();

        /// <summary>
        /// Register a new unit conversion for a given <paramref name="unit"/>, or overwrite an existing one
        /// </summary>
        /// <param name="unit">The unit to register a conversion for</param>
        /// <param name="coefficient">A coefficient used to scale the unit to the base unit of the converter</param>
        public static void RegisterConversion(TUnit unit, double coefficient) =>
            RegisterConversion(unit, x => x * coefficient, x => x / coefficient);

        /// <inheritdoc cref="RegisterConversion(TUnit, double)"/>
        /// <param name="unit">The unit to register a conversion for</param>
        /// <param name="toBase">A function to convert the given <paramref name="unit"/> to the base unit</param>
        /// <param name="fromBase">A function to convert the base unit to the given <paramref name="unit"/></param>
        public static void RegisterConversion(TUnit unit, Func<double, double> toBase, Func<double, double> fromBase) =>
            Conversions[unit] = new UnitConversion(toBase, fromBase);

        /// <summary>
        /// Convert a <see cref="Measurement{TUnit}"/> to another unit, if possible
        /// </summary>
        /// <param name="measurement">The <see cref="Measurement{TUnit}"/> to convert</param>
        /// <param name="desiredUnit">The desired <typeparamref name="TUnit"/> for the output</param>
        /// <returns>The converted <see cref="Measurement{TUnit}"/></returns>
        /// <exception cref="ArgumentException">
        /// Thrown when a conversion mapping isn't available, which can be resolved by registering an appropriate 
        /// conversion and trying again.
        /// </exception>
        public Measurement<TUnit> Convert(Measurement<TUnit> measurement, TUnit desiredUnit)
        {
            if (measurement.Unit.Equals(desiredUnit))
                return measurement;

            if (!Conversions.ContainsKey(measurement.Unit))
                throw new ArgumentException(
                    "There is no conversion information about the unit of the provided measurement.", 
                    nameof(measurement));
            if (!Conversions.ContainsKey(desiredUnit))
                throw new ArgumentException(
                    "There is no conversion information about the desired unit.", 
                    nameof(desiredUnit));
            
            double valueInBaseUnit = measurement.Unit.Equals(BaseUnit)
                ? measurement.Value
                : Conversions[measurement.Unit].ToBaseUnit(measurement.Value);
            double valueInDesiredUnit = desiredUnit.Equals(BaseUnit)
                ? valueInBaseUnit
                : Conversions[desiredUnit].FromBaseUnit(valueInBaseUnit);
            return new Measurement<TUnit>(valueInDesiredUnit, desiredUnit);
        }
    }
}
