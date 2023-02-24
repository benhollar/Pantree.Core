using Pantree.Core.Utilities.Interfaces;

namespace Pantree.Core.Cooking
{
    /// <summary>
    /// A collection of nutritional information, meant to mimic a nutritional label found on food packaging
    /// </summary>
    public partial record struct Nutrition : Identifiable
    {
        /// <inheritdoc/>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// The number of calories, if known (kCal)
        /// </summary>
        public uint? Calories { get; set; } = null;

        /// <summary>
        /// The amount of fat, if known (grams)
        /// </summary>
        public uint? TotalFat { get; set; } = null;

        /// <summary>
        /// The amount of saturated fat, if known (grams)
        /// </summary>
        public uint? SaturatedFat { get; set; } = null;

        /// <summary>
        /// The amount of trans-unsaturated fat, if known (grams)
        /// </summary>
        public uint? TransFat { get; set; } = null;

        /// <summary>
        /// The amount of cholesterol, if known (milligrams)
        /// </summary>
        public uint? Cholesterol { get; set; } = null;

        /// <summary>
        /// The amount of sodium, if known (milligrams)
        /// </summary>
        public uint? Sodium { get; set; } = null;

        /// <summary>
        /// The amount of carbohydrates, if known (grams)
        /// </summary>
        public uint? Carbohydrates { get; set; } = null;

        /// <summary>
        /// The amount of fiber, if known (grams)
        /// </summary>
        public uint? Fiber { get; set; } = null;

        /// <summary>
        /// The amount of sugar, if known (grams)
        /// </summary>
        public uint? Sugar { get; set; } = null;

        /// <summary>
        /// The amount of protein, if known (grams)
        /// </summary>
        public uint? Protein { get; set; } = null;

        /// <summary>
        /// Construct a new <see cref="Nutrition"/>
        /// </summary>
        public Nutrition() { }
        
        /// <summary>
        /// Add two <see cref="Nutrition"/> values together, treating missing values as 0
        /// </summary>
        /// <param name="lhs">The first value, treated as the left-hand-side of the arithmetic operation</param>
        /// <param name="rhs">The second value, treated as the right-hand-side of the arithmetic operation</param>
        /// <returns>The summed <see cref="Nutrition"/></returns>
        public static Nutrition? operator +(Nutrition? lhs, Nutrition? rhs)
        {
            if (lhs is null && rhs is null)
                return null;
            if (lhs is null)
                return rhs;
            if (rhs is null)
                return lhs;

            return new Nutrition
            {
                Calories = AddNullable(lhs?.Calories, rhs?.Calories),
                TotalFat = AddNullable(lhs?.TotalFat, rhs?.TotalFat),
                SaturatedFat = AddNullable(lhs?.SaturatedFat, rhs?.SaturatedFat),
                TransFat = AddNullable(lhs?.TransFat, rhs?.TransFat),
                Cholesterol = AddNullable(lhs?.Cholesterol, rhs?.Cholesterol),
                Sodium = AddNullable(lhs?.Sodium, rhs?.Sodium),
                Carbohydrates = AddNullable(lhs?.Carbohydrates, rhs?.Carbohydrates),
                Fiber = AddNullable(lhs?.Fiber, rhs?.Fiber),
                Sugar = AddNullable(lhs?.Sugar, rhs?.Sugar),
                Protein = AddNullable(lhs?.Protein, rhs?.Protein)
            };
        }
            
        
        /// <summary>
        /// Multiply a <see cref="Nutrition"/> by some <paramref name="coefficient"/>
        /// </summary>
        /// <param name="nutrition">The nutritional value to scale</param>
        /// <param name="coefficient">
        /// The coefficient to multiply by, expected to be non-negative. Fractional multipliers are allowed, but values
        /// will be rounded to the nearest whole number. Negative multipliers are disallowed due to ambiguous
        /// implications of negative nutritional values.
        /// </param>
        /// <returns>The multiplied <see cref="Nutrition"/></returns>
        public static Nutrition operator *(Nutrition nutrition, double coefficient)
        {
            if (coefficient < 0)
                throw new ArgumentException("The coefficient must be non-negative.", nameof(coefficient));

            return new Nutrition
            {
                Calories = (uint?)(nutrition.Calories * coefficient),
                TotalFat = (uint?)(nutrition.TotalFat * coefficient),
                SaturatedFat = (uint?)(nutrition.SaturatedFat * coefficient),
                TransFat = (uint?)(nutrition.TransFat * coefficient),
                Cholesterol = (uint?)(nutrition.Cholesterol * coefficient),
                Sodium = (uint?)(nutrition.Sodium * coefficient),
                Carbohydrates = (uint?)(nutrition.Carbohydrates * coefficient),
                Fiber = (uint?)(nutrition.Fiber * coefficient),
                Sugar = (uint?)(nutrition.Sugar * coefficient),
                Protein = (uint?)(nutrition.Protein * coefficient),
            };
        }
        /// <inheritdoc cref="operator *(Nutrition, double)"/>
        public static Nutrition operator *(double coefficient, Nutrition nutrition) => nutrition * coefficient;

        /// <summary>
        /// Divide a <see cref="Nutrition"/> by some <paramref name="divisor"/>
        /// </summary>
        /// <param name="nutrition">The nutritional value to divide</param>
        /// <param name="divisor">
        /// The strictly positive and nonzero divisor. Division by zero, like in purely mathematical situations, is
        /// undefined here and will result in an <see cref="ArgumentException"/>. Division by negative divisors is
        /// disallowed due to ambiguous implications of negative nutritional values.
        /// </param>
        /// <returns>The divided <see cref="Nutrition"/></returns>
        public static Nutrition operator /(Nutrition nutrition, double divisor)
        {
            if (divisor <= 0)
                throw new ArgumentException("The divisor must be non-negative and nonzero.", nameof(divisor));

            return nutrition * (1d / divisor);
        }

        /// <summary>
        /// Add two nullable unsigned integers, preserving non-null values
        /// </summary>
        /// <remarks>
        /// C#'s default behavior, called "lifted operators", is incorrect in this situation. While adding null to
        /// a number is in many cases nonsensical, we're instead simply looking to treat null as 0, unless both items
        /// happen to be null.
        /// </remarks>
        /// <param name="lhs">The first value</param>
        /// <param name="rhs">The second value</param>
        /// <returns>The sum</returns>
        private static uint? AddNullable(uint? lhs, uint? rhs)
        {
            if (lhs is null && rhs is null)
                return null;
            else if (lhs is null)
                return rhs;
            else if (rhs is null)
                return lhs;
            else
                return lhs + rhs;
        }
    }

    public partial record struct Nutrition : IEquatable<Nutrition>
    {
        /// <summary>
        /// Compare this <see cref="Nutrition"/> object with another
        /// </summary>
        /// <remarks>
        /// Note that while <see cref="Nutrition"/> is a record struct, this operation has been overridden to ignore
        /// the <see cref="Id"/> property. <see cref="Id"/> is a GUID and will be unique even between otherwise
        /// equivalent nutritional information, which defeats the main use case of comparing two of these objects.
        /// </remarks>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public bool Equals(Nutrition rhs)
        {
            return Calories == rhs.Calories &&
                    TotalFat == rhs.TotalFat &&
                    SaturatedFat == rhs.SaturatedFat &&
                    TransFat == rhs.TransFat &&
                    Cholesterol == rhs.Cholesterol &&
                    Sodium == rhs.Sodium &&
                    Carbohydrates == rhs.Carbohydrates &&
                    Fiber == rhs.Fiber &&
                    Sugar == rhs.Sugar &&
                    Protein == rhs.Protein;
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 37;
                result *= 397;
                if (Calories != null)
                    result += Calories.GetHashCode();
                result *= 397;
                if (TotalFat != null)
                    result += TotalFat.GetHashCode();
                result *= 397;
                if (SaturatedFat != null)
                    result += SaturatedFat.GetHashCode();
                result *= 397;
                if (TransFat != null)
                    result += TransFat.GetHashCode();
                result *= 397;
                if (Cholesterol != null)
                    result += Cholesterol.GetHashCode();
                result *= 397;
                if (Sodium != null)
                    result += Sodium.GetHashCode();
                result *= 397;
                if (Carbohydrates != null)
                    result += Carbohydrates.GetHashCode();
                result *= 397;
                if (Fiber != null)
                    result += Fiber.GetHashCode();
                result *= 397;
                if (Sugar != null)
                    result += Sugar.GetHashCode();
                result *= 397;
                if (Protein != null)
                    result += Protein.GetHashCode();
                return result;
            }
        }
    }

}
