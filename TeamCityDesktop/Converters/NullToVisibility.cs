using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeamCityDesktop.Converters
{
    public class NullToVisibility : IValueConverter
    {
        public Visibility OnNull { get; set; }
        public Visibility OnNotNull { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? OnNull : OnNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
