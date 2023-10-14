using System;

namespace Pantree.Core.Cooking
{
    /// <summary>
    /// A collection of nutritional information, meant to mimic a nutritional label found on food packaging
    /// </summary>
    public partial record struct Nutrition
    {
        /// <summary>
        /// The number of calories, if known (kCal)
        /// </summary>
        public double? Calories { readonly get => _calories; set => _calories = ValidateNonnegative(value); }
        private double? _calories = null;

        /// <summary>
        /// The amount of fat, if known (grams)
        /// </summary>
        public double? TotalFat { readonly get => _totalFat; set => _totalFat = ValidateNonnegative(value); }
        private double? _totalFat = null;

        /// <summary>
        /// The amount of saturated fat, if known (grams)
        /// </summary>
        public double? SaturatedFat
        { 
            readonly get => _saturatedFat;
            set => _saturatedFat = ValidateNonnegative(value);
        }
        private double? _saturatedFat = null;

        /// <summary>
        /// The amount of trans-unsaturated fat, if known (grams)
        /// </summary>
        public double? TransFat { readonly get => _transFat; set => _transFat = ValidateNonnegative(value); }
        private double? _transFat = null;

        /// <summary>
        /// The amount of cholesterol, if known (milligrams)
        /// </summary>
        public double? Cholesterol { readonly get => _cholesterol; set => _cholesterol = ValidateNonnegative(value); }
        private double? _cholesterol = null;

        /// <summary>
        /// The amount of sodium, if known (milligrams)
        /// </summary>
        public double? Sodium { readonly get => _sodium; set => _sodium = ValidateNonnegative(value); }
        private double? _sodium = null;

        /// <summary>
        /// The amount of carbohydrates, if known (grams)
        /// </summary>
        public double? Carbohydrates
        { 
            readonly get => _carbohydrates;
            set => _carbohydrates = ValidateNonnegative(value);
        }
        private double? _carbohydrates = null;

        /// <summary>
        /// The amount of fiber, if known (grams)
        /// </summary>
        public double? Fiber { readonly get => _fiber; set => _fiber = ValidateNonnegative(value); }
        private double? _fiber = null;

        /// <summary>
        /// The amount of sugar, if known (grams)
        /// </summary>
        public double? Sugar { readonly get => _sugar; set => _sugar = ValidateNonnegative(value); }
        private double? _sugar = null;

        /// <summary>
        /// The amount of protein, if known (grams)
        /// </summary>
        public double? Protein { readonly get => _protein; set => _protein = ValidateNonnegative(value); }
        private double? _protein = null;

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
                Calories = nutrition.Calories * coefficient,
                TotalFat = nutrition.TotalFat * coefficient,
                SaturatedFat = nutrition.SaturatedFat * coefficient,
                TransFat = nutrition.TransFat * coefficient,
                Cholesterol = nutrition.Cholesterol * coefficient,
                Sodium = nutrition.Sodium * coefficient,
                Carbohydrates = nutrition.Carbohydrates * coefficient,
                Fiber = nutrition.Fiber * coefficient,
                Sugar = nutrition.Sugar * coefficient,
                Protein = nutrition.Protein * coefficient,
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
        private static double? AddNullable(double? lhs, double? rhs)
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

        private static double? ValidateNonnegative(double? value)
        {
            if (value is not null && value < 0)
                throw new ArgumentException($"Nutritional values must be nonnegative; got {value} instead.");
            return value;
        }
    }
}
