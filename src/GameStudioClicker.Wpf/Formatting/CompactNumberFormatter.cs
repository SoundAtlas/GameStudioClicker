using System.Globalization;

namespace GameStudioClicker.Wpf.Formatting
{
    public static class CompactNumberFormatter
    {
        private static readonly (long divisor, string suffix)[] Scales =
            {
            (1_000_000_000_000_000_000, "Qi"),
            (1_000_000_000_000_000, "Qa"),
            (1_000_000_000_000, "T"),
            (1_000_000_000, "B"),
            (1_000_000, "M"),
            (1_000, "K"),
        };
        public static string Format(long value)
        {
            foreach ((long divisor, string suffix) in Scales)
            {
                if (value >= divisor)
                {
                    decimal shortenedValue = value / (decimal)divisor;

                    return shortenedValue.ToString("0.#", CultureInfo.InvariantCulture) + suffix;
                }
            }

            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}
