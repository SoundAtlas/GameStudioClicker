using GameStudioClicker.Wpf.Formatting;
using System.Globalization;
using System.Windows.Data;

namespace GameStudioClicker.Wpf.Converters
{
    public class CompactNumberConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long number)
            {
                return CompactNumberFormatter.Format(number);
            }
            else if (value is int intValue)
            {
                return CompactNumberFormatter.Format((long)intValue);

            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
