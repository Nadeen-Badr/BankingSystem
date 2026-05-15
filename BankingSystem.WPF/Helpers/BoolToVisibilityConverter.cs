using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BankingSystem.WPF.Helpers
{
    /// <summary>
    /// Converts boolean values to WPF Visibility values.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            // Prevent invalid cast exceptions from null or unexpected bindings
            if (value is bool isVisible)
            {
                return isVisible
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value is Visibility visibility &&
                   visibility == Visibility.Visible;
        }
    }
}