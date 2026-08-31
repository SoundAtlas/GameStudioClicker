using System.Globalization;

namespace GameStudioClicker.Wpf.Formatting
{
    public static class CompactNumberFormatter
    {
        public static string Format(long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}
